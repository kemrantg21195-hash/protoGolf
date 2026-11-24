using UnityEngine;

namespace Golf.GamePlay
{
	public class EnemyShip
	{
		public bool isSinking => m_isSinking;
		public bool isDestroyed => m_isDestroyed;

		private GameObject m_gameObject;
		private Transform m_transform;
		private Collider[] m_colliders;
		private Transform m_targetShip;
		private GameObject m_cannonballPrefab;
		private Collider m_seaCollider;
		private int m_health;
		private float m_moveSpeed;
		private float m_turnSpeed;
		private float m_combatDistance;
		private float m_shootRange;
		private float m_shootInterval;
		private float m_shootTimer;
		private float m_hullOffset;
		private float m_rockingTimer;
		private float m_baseHeight;
		private Quaternion m_courseRotation;
		private bool m_isSinking;
		private bool m_isDestroyed;
		private float m_sinkTimer;

		public EnemyShip(
			GameObject gameObject,
			Transform targetShip,
			GameObject cannonballPrefab,
			int maxHealth,
			float moveSpeed,
			float turnSpeed,
			float combatDistance,
			float shootRange,
			float shootInterval,
			Collider seaCollider = null)
		{
			m_gameObject = gameObject;
			m_transform = gameObject.transform;
			m_colliders = gameObject.GetComponentsInChildren<Collider>();
			m_targetShip = targetShip;
			m_cannonballPrefab = cannonballPrefab;
			m_seaCollider = seaCollider;
			m_health = maxHealth;
			m_moveSpeed = moveSpeed;
			m_turnSpeed = turnSpeed;
			m_combatDistance = combatDistance;
			m_shootRange = shootRange;
			m_shootInterval = shootInterval;
			m_shootTimer = Random.Range(1.2f, shootInterval * 0.75f);
			m_baseHeight = m_transform.position.y;
			m_courseRotation = m_transform.rotation;
			m_hullOffset = CalculateHullOffset();
		}

		public void TakeDamage(int damage)
		{
			if (m_isSinking || m_isDestroyed)
			{
				return;
			}

			m_health -= damage;

			if (m_health <= 0)
			{
				m_isSinking = true;
				m_sinkTimer = 2.5f;
			}
		}

		public bool CheckHit(Vector3 point, float hitRadius)
		{
			if (m_isSinking || m_isDestroyed || m_gameObject == null)
			{
				return false;
			}

			float sqrRadius = hitRadius * hitRadius;

			if (m_colliders != null && m_colliders.Length > 0)
			{
				for (int i = 0; i < m_colliders.Length; i++)
				{
					Collider col = m_colliders[i];

					if (col != null && col.enabled && col.bounds.SqrDistance(point) <= sqrRadius)
					{
						return true;
					}
				}

				return false;
			}

			return (m_transform.position - point).sqrMagnitude <= 9f;
		}

		public Cannonball TryShoot(float deltaTime)
		{
			if (m_isSinking || m_isDestroyed || m_targetShip == null || m_cannonballPrefab == null)
			{
				return null;
			}

			Vector3 flatToPlayer = new Vector3(
				m_targetShip.position.x - m_transform.position.x,
				0f,
				m_targetShip.position.z - m_transform.position.z);

			float distanceToPlayer = flatToPlayer.magnitude;

			if (distanceToPlayer > m_shootRange || distanceToPlayer <= m_hullOffset + 2f)
			{
				return null;
			}

			m_shootTimer -= deltaTime;

			if (m_shootTimer > 0f)
			{
				return null;
			}

			m_shootTimer = m_shootInterval;

			Vector3 flatDirection = flatToPlayer / distanceToPlayer;
			Vector3 muzzlePosition = m_transform.position + Vector3.up * 2f + flatDirection * m_hullOffset;

			Vector3 spread = new Vector3(Random.Range(-2.5f, 2.5f), 0f, Random.Range(-2.5f, 2.5f));
			Vector3 aimTarget = m_targetShip.position + Vector3.up * 1.2f + spread;

			Vector3 displacement = aimTarget - muzzlePosition;
			Vector3 horizontalDisplacement = new Vector3(displacement.x, 0f, displacement.z);
			float horizontalDistance = horizontalDisplacement.magnitude;

			if (horizontalDistance < 1f)
			{
				return null;
			}

			Vector3 shotDirection = horizontalDisplacement / horizontalDistance;
			float angleDeg = Mathf.Clamp(horizontalDistance * 0.45f, 12f, 26f);
			float angleRad = angleDeg * Mathf.Deg2Rad;
			float gravity = Mathf.Abs(Physics.gravity.y);
			float cosAngle = Mathf.Cos(angleRad);
			float denominator = 2f * cosAngle * cosAngle * (horizontalDistance * Mathf.Tan(angleRad) - displacement.y);

			if (denominator <= 0.1f)
			{
				return null;
			}

			float launchSpeed = Mathf.Sqrt((gravity * horizontalDistance * horizontalDistance) / denominator);
			Vector3 launchVelocity = shotDirection * (launchSpeed * cosAngle) + Vector3.up * (launchSpeed * Mathf.Sin(angleRad));

			GameObject ballObject = Object.Instantiate(
				m_cannonballPrefab,
				muzzlePosition,
				Quaternion.LookRotation(shotDirection));

			IgnoreOwnColliders(ballObject);

			Rigidbody rb = ballObject.GetComponentInChildren<Rigidbody>();

			if (rb != null)
			{
				rb.useGravity = true;
				rb.AddForce(launchVelocity, ForceMode.VelocityChange);
			}

			return new Cannonball(ballObject, rb, m_seaCollider);
		}

		public void FixedUpdate(float fixedDeltaTime)
		{
			if (m_isDestroyed || m_gameObject == null)
			{
				return;
			}

			if (m_isSinking)
			{
				UpdateSinking(fixedDeltaTime);
				return;
			}

			if (m_targetShip != null)
			{
				Vector3 currentPos = m_transform.position;
				Vector3 targetPos = new Vector3(m_targetShip.position.x, m_baseHeight, m_targetShip.position.z);
				Vector3 flatCurrent = new Vector3(currentPos.x, m_baseHeight, currentPos.z);
				Vector3 toPlayer = targetPos - flatCurrent;
				float distance = toPlayer.magnitude;

				if (distance > 0.1f)
				{
					Vector3 direction = toPlayer / distance;
					Vector3 broadside = Vector3.Cross(Vector3.up, direction).normalized;
					Vector3 desiredFacing;

					if (distance > m_combatDistance)
					{
						flatCurrent += direction * (m_moveSpeed * fixedDeltaTime);
						desiredFacing = direction;
					}
					else if (distance < m_combatDistance * 0.85f)
					{
						Vector3 retreatDirection = (-direction + broadside * 0.5f).normalized;
						flatCurrent += retreatDirection * (m_moveSpeed * fixedDeltaTime);
						desiredFacing = retreatDirection;
					}
					else
					{
						flatCurrent += broadside * (m_moveSpeed * 0.35f * fixedDeltaTime);
						desiredFacing = broadside;
					}

					Quaternion targetRotation = Quaternion.LookRotation(desiredFacing);
					m_courseRotation = Quaternion.Slerp(m_courseRotation, targetRotation, m_turnSpeed * fixedDeltaTime);
				}

				m_rockingTimer += fixedDeltaTime * 2f;
				float heightOffset = Mathf.Sin(m_rockingTimer) * 0.12f;
				m_transform.position = new Vector3(flatCurrent.x, m_baseHeight + heightOffset, flatCurrent.z);

				float pitch = Mathf.Cos(m_rockingTimer * 0.9f) * 1.5f;
				float roll = Mathf.Sin(m_rockingTimer * 1.1f) * 2.5f;
				m_transform.rotation = m_courseRotation * Quaternion.Euler(pitch, 0f, roll);
			}
		}

		private float CalculateHullOffset()
		{
			float maxExtent = 2.5f;

			if (m_colliders != null)
			{
				for (int i = 0; i < m_colliders.Length; i++)
				{
					Collider col = m_colliders[i];

					if (col != null)
					{
						Vector3 extents = col.bounds.extents;
						float horizontalExtent = Mathf.Max(extents.x, extents.z);

						if (horizontalExtent > maxExtent)
						{
							maxExtent = horizontalExtent;
						}
					}
				}
			}

			return maxExtent + 1f;
		}

		private void IgnoreOwnColliders(GameObject ballObject)
		{
			if (m_colliders == null || m_colliders.Length == 0)
			{
				return;
			}

			Collider[] ballColliders = ballObject.GetComponentsInChildren<Collider>();

			for (int i = 0; i < ballColliders.Length; i++)
			{
				for (int j = 0; j < m_colliders.Length; j++)
				{
					if (ballColliders[i] != null && m_colliders[j] != null)
					{
						Physics.IgnoreCollision(ballColliders[i], m_colliders[j], true);
					}
				}
			}
		}

		private void UpdateSinking(float fixedDeltaTime)
		{
			m_sinkTimer -= fixedDeltaTime;
			m_transform.position += Vector3.down * (1.8f * fixedDeltaTime);
			m_transform.Rotate(Vector3.forward, 18f * fixedDeltaTime, Space.Self);

			if (m_sinkTimer <= 0f)
			{
				Destroy();
			}
		}

		public void Destroy()
		{
			m_isDestroyed = true;

			if (m_gameObject != null)
			{
				Object.Destroy(m_gameObject);
			}
		}
	}
}