using UnityEngine;

namespace Golf.GamePlay
{
	public class Cannonball
	{
		public bool isExpired => m_lifeTime <= 0f;
		public bool isValid => m_gameObject != null && m_lifeTime > 0f;
		public Vector3 position => m_gameObject != null ? m_gameObject.transform.position : Vector3.zero;
		public Vector3 launchPosition => m_launchPosition;

		private GameObject m_gameObject;
		private Rigidbody m_rigidbody;
		private Collider m_seaCollider;
		private Vector3 m_launchPosition;
		private float m_radius;
		private float m_lifeTime = 5f;

		public Cannonball(GameObject gameObject, Rigidbody rigidbody, Collider seaCollider = null)
		{
			m_gameObject = gameObject;
			m_rigidbody = rigidbody;
			m_seaCollider = seaCollider;
			m_launchPosition = gameObject != null ? gameObject.transform.position : Vector3.zero;
			m_radius = CalculateRadius(gameObject);
		}

		public void Launch(Vector3 impulse)
		{
			if (m_gameObject != null)
			{
				m_launchPosition = m_gameObject.transform.position;
			}

			if (m_rigidbody != null)
			{
				m_rigidbody.AddForce(impulse, ForceMode.Impulse);
			}
		}

		public void UpdateLifeTime(float deltaTime)
		{
			m_lifeTime -= deltaTime;

			if (m_gameObject != null && IsTouchingSea())
			{
				Expire();
			}
		}

		public void Expire()
		{
			m_lifeTime = 0f;
		}

		public void Destroy()
		{
			if (m_gameObject != null)
			{
				Object.Destroy(m_gameObject);
			}
		}

		private bool IsTouchingSea()
		{
			if (m_seaCollider == null || !m_seaCollider.enabled)
			{
				return false;
			}

			Vector3 point = m_gameObject.transform.position;
			float sqrRadius = m_radius * m_radius;

			if (m_seaCollider is BoxCollider ||
				m_seaCollider is SphereCollider ||
				m_seaCollider is CapsuleCollider ||
				(m_seaCollider is MeshCollider meshCollider && meshCollider.convex))
			{
				Vector3 closestPoint = m_seaCollider.ClosestPoint(point);
				return (closestPoint - point).sqrMagnitude <= sqrRadius;
			}

			return m_seaCollider.bounds.SqrDistance(point) <= sqrRadius;
		}

		private float CalculateRadius(GameObject gameObject)
		{
			if (gameObject == null)
			{
				return 0.25f;
			}

			Collider col = gameObject.GetComponentInChildren<Collider>();

			if (col != null)
			{
				return Mathf.Max(0.15f, col.bounds.extents.y);
			}

			return 0.25f;
		}
	}
}