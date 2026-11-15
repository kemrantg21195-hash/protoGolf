using Golf.States.Interfaces;
using UnityEngine;

namespace Golf.GamePlay
{
	public class StoneSpawner : IUpdatable
	{
		private const float MillisecondsInSecond = 1000f;

		private readonly GameObject m_stonePrefab;
		private readonly Transform m_spawnPoint;
		private readonly float m_minSpawnDelay;
		private readonly float m_initialMaxSpawnDelay;
		private readonly float m_decreaseStepSeconds;

		private float m_currentMaxSpawnDelay;
		private float m_timer;
		private bool m_isActive;

		public StoneSpawner(
			GameObject stonePrefab,
			Transform spawnPoint,
			float minSpawnDelay,
			float maxSpawnDelay,
			float decreaseStepMs)
		{
			m_stonePrefab = stonePrefab;
			m_spawnPoint = spawnPoint;
			m_minSpawnDelay = minSpawnDelay;
			m_initialMaxSpawnDelay = maxSpawnDelay;
			m_decreaseStepSeconds = decreaseStepMs / MillisecondsInSecond;
		}

		public void StartSpawning()
		{
			m_currentMaxSpawnDelay = m_initialMaxSpawnDelay;
			m_isActive = true;
			ScheduleNextSpawn();
		}

		public void StopSpawning()
		{
			m_isActive = false;
		}

		public void Update()
		{
			if (!m_isActive)
			{
				return;
			}

			m_timer -= Time.deltaTime;

			if (m_timer <= 0f)
			{
				SpawnStone();
				DecreaseMaxDelay();
				ScheduleNextSpawn();
			}
		}

		private void SpawnStone()
		{
			Object.Instantiate(m_stonePrefab, m_spawnPoint.position, m_spawnPoint.rotation);
		}

		private void DecreaseMaxDelay()
		{
			m_currentMaxSpawnDelay = Mathf.Max(
				m_minSpawnDelay,
				m_currentMaxSpawnDelay - m_decreaseStepSeconds);
		}

		private void ScheduleNextSpawn()
		{
			m_timer = Random.Range(m_minSpawnDelay, m_currentMaxSpawnDelay);
		}
	}
}