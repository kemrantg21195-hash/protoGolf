using System;
using System.Collections.Generic;
using Golf.States.Interfaces;
using UnityEngine;

namespace Golf.GamePlay
{
	public class EnemySpawner : IUpdatable
	{
		public event Action onEnemyShot;
		public event Action<float> onEnemyHit;
		public event Action onEnemySunk;
		public event Action onPlayerHit;

		private GameObject m_enemyPrefab;
		private Transform[] m_spawnPoints;
		private bool[] m_spawnedFlags;
		private Transform m_playerShipTransform;
		private Collider[] m_playerColliders;
		private GameObject m_cannonballPrefab;
		private Collider m_seaCollider;
		private float m_spawnTriggerDistance;
		private int m_baseEnemyHealth;
		private float m_enemyMoveSpeed;
		private float m_enemyTurnSpeed;
		private float m_enemyCombatDistance;
		private float m_baseEnemyShootRange;
		private float m_baseEnemyShootInterval;
		private BotDifficulty m_difficulty;
		private int m_hardExtraHealth;
		private float m_hardShootRangeMultiplier;
		private float m_hardShootIntervalMultiplier;
		private List<EnemyShip> m_enemies = new();
		private List<Cannonball> m_enemyCannonballs = new();

		public EnemySpawner(
			GameObject enemyPrefab,
			Transform[] spawnPoints,
			Transform playerShipTransform,
			GameObject cannonballPrefab,
			float spawnTriggerDistance,
			int enemyHealth,
			float enemyMoveSpeed,
			float enemyTurnSpeed,
			float enemyCombatDistance,
			float enemyShootRange,
			float enemyShootInterval,
			BotDifficulty difficulty,
			int hardExtraHealth,
			float hardShootRangeMultiplier,
			float hardShootIntervalMultiplier,
			Collider seaCollider = null)
		{
			m_enemyPrefab = enemyPrefab;
			m_spawnPoints = spawnPoints;
			m_spawnedFlags = spawnPoints != null ? new bool[spawnPoints.Length] : Array.Empty<bool>();
			m_playerShipTransform = playerShipTransform;
			m_playerColliders = playerShipTransform.GetComponentsInChildren<Collider>();
			m_cannonballPrefab = cannonballPrefab;
			m_seaCollider = seaCollider;
			m_spawnTriggerDistance = spawnTriggerDistance;
			m_baseEnemyHealth = enemyHealth;
			m_enemyMoveSpeed = enemyMoveSpeed;
			m_enemyTurnSpeed = enemyTurnSpeed;
			m_enemyCombatDistance = enemyCombatDistance;
			m_baseEnemyShootRange = enemyShootRange;
			m_baseEnemyShootInterval = enemyShootInterval;
			m_difficulty = difficulty;
			m_hardExtraHealth = hardExtraHealth;
			m_hardShootRangeMultiplier = hardShootRangeMultiplier;
			m_hardShootIntervalMultiplier = hardShootIntervalMultiplier;
		}

		public void SetDifficulty(BotDifficulty difficulty)
		{
			m_difficulty = difficulty;
		}

		public void Update()
		{
			UpdateSpawning();
			UpdateEnemyShooting();
			UpdateEnemyCannonballs();
		}

		public void FixedUpdate(IReadOnlyList<Cannonball> playerCannonballs)
		{
			for (int i = m_enemies.Count - 1; i >= 0; i--)
			{
				EnemyShip enemy = m_enemies[i];
				enemy.FixedUpdate(Time.fixedDeltaTime);

				if (enemy.isDestroyed)
				{
					m_enemies.RemoveAt(i);
				}
			}

			CheckPlayerCannonballHits(playerCannonballs);
			CheckEnemyCannonballHits();
		}

		public void ClearAll()
		{
			for (int i = 0; i < m_enemies.Count; i++)
			{
				m_enemies[i].Destroy();
			}

			m_enemies.Clear();

			for (int i = 0; i < m_enemyCannonballs.Count; i++)
			{
				m_enemyCannonballs[i].Destroy();
			}

			m_enemyCannonballs.Clear();

			if (m_spawnedFlags.Length > 0)
			{
				Array.Clear(m_spawnedFlags, 0, m_spawnedFlags.Length);
			}
		}

		private void UpdateSpawning()
		{
			if (m_enemyPrefab == null || m_spawnPoints == null || m_playerShipTransform == null)
			{
				return;
			}

			Vector3 playerPosition = m_playerShipTransform.position;
			float sqrTriggerDistance = m_spawnTriggerDistance * m_spawnTriggerDistance;

			for (int i = 0; i < m_spawnPoints.Length; i++)
			{
				if (m_spawnedFlags[i])
				{
					continue;
				}

				Transform spawnPoint = m_spawnPoints[i];

				if (spawnPoint == null)
				{
					continue;
				}

				float dx = playerPosition.x - spawnPoint.position.x;
				float dz = playerPosition.z - spawnPoint.position.z;

				if (dx * dx + dz * dz <= sqrTriggerDistance)
				{
					m_spawnedFlags[i] = true;
					SpawnEnemyAt(spawnPoint);
				}
			}
		}

		private void SpawnEnemyAt(Transform spawnPoint)
		{
			Vector3 spawnPosition = new Vector3(
				spawnPoint.position.x,
				m_playerShipTransform.position.y,
				spawnPoint.position.z);

			Vector3 toPlayer = new Vector3(
				m_playerShipTransform.position.x - spawnPosition.x,
				0f,
				m_playerShipTransform.position.z - spawnPosition.z);

			Quaternion spawnRotation = toPlayer.sqrMagnitude > 0.01f
				? Quaternion.LookRotation(toPlayer.normalized)
				: spawnPoint.rotation;

			GameObject enemyObject = UnityEngine.Object.Instantiate(
				m_enemyPrefab,
				spawnPosition,
				spawnRotation);

			GetDifficultyParameters(
				out int health,
				out float shootRange,
				out float shootInterval);

			var enemyShip = new EnemyShip(
				enemyObject,
				m_playerShipTransform,
				m_cannonballPrefab,
				health,
				m_enemyMoveSpeed,
				m_enemyTurnSpeed,
				m_enemyCombatDistance,
				shootRange,
				shootInterval,
				m_seaCollider);

			m_enemies.Add(enemyShip);
		}

		private void GetDifficultyParameters(
			out int health,
			out float shootRange,
			out float shootInterval)
		{
			if (m_difficulty == BotDifficulty.Hard)
			{
				health = m_baseEnemyHealth + m_hardExtraHealth;
				shootRange = m_baseEnemyShootRange * m_hardShootRangeMultiplier;
				shootInterval = m_baseEnemyShootInterval * m_hardShootIntervalMultiplier;
			}
			else
			{
				health = m_baseEnemyHealth;
				shootRange = m_baseEnemyShootRange;
				shootInterval = m_baseEnemyShootInterval;
			}
		}

		private void UpdateEnemyShooting()
		{
			for (int i = 0; i < m_enemies.Count; i++)
			{
				Cannonball ball = m_enemies[i].TryShoot(Time.deltaTime);

				if (ball != null)
				{
					m_enemyCannonballs.Add(ball);
					onEnemyShot?.Invoke();
				}
			}
		}

		private void UpdateEnemyCannonballs()
		{
			for (int i = m_enemyCannonballs.Count - 1; i >= 0; i--)
			{
				Cannonball ball = m_enemyCannonballs[i];
				ball.UpdateLifeTime(Time.deltaTime);

				if (ball.isExpired)
				{
					ball.Destroy();
					m_enemyCannonballs.RemoveAt(i);
				}
			}
		}

		private void CheckPlayerCannonballHits(IReadOnlyList<Cannonball> playerCannonballs)
		{
			if (playerCannonballs == null)
			{
				return;
			}

			for (int b = 0; b < playerCannonballs.Count; b++)
			{
				Cannonball ball = playerCannonballs[b];

				if (!ball.isValid)
				{
					continue;
				}

				for (int e = 0; e < m_enemies.Count; e++)
				{
					EnemyShip enemy = m_enemies[e];

					if (enemy.CheckHit(ball.position, 0.6f))
					{
						bool wasSinking = enemy.isSinking;
						float dx = ball.position.x - ball.launchPosition.x;
						float dz = ball.position.z - ball.launchPosition.z;
						float shotDistance = Mathf.Sqrt(dx * dx + dz * dz);

						enemy.TakeDamage(ball.damage);
						ball.Expire();
						onEnemyHit?.Invoke(shotDistance);

						if (!wasSinking && enemy.isSinking)
						{
							onEnemySunk?.Invoke();
						}

						break;
					}
				}
			}
		}

		private void CheckEnemyCannonballHits()
		{
			for (int b = 0; b < m_enemyCannonballs.Count; b++)
			{
				Cannonball ball = m_enemyCannonballs[b];

				if (!ball.isValid)
				{
					continue;
				}

				if (IsPointHittingPlayer(ball.position, 0.6f))
				{
					ball.Expire();
					onPlayerHit?.Invoke();
				}
			}
		}

		private bool IsPointHittingPlayer(Vector3 point, float hitRadius)
		{
			if (m_playerShipTransform == null)
			{
				return false;
			}

			float sqrRadius = hitRadius * hitRadius;

			if (m_playerColliders != null && m_playerColliders.Length > 0)
			{
				for (int i = 0; i < m_playerColliders.Length; i++)
				{
					Collider col = m_playerColliders[i];

					if (col != null && col.enabled && col.bounds.SqrDistance(point) <= sqrRadius)
					{
						return true;
					}
				}

				return false;
			}

			return (m_playerShipTransform.position - point).sqrMagnitude <= 9f;
		}
	}
}