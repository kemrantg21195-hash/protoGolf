using System.Collections.Generic;
using Golf.States.Interfaces;
using UnityEngine;

namespace Golf.GamePlay
{
	public class StoneSpawner : IUpdatable
	{
		private GameObject m_stonePrefab;
		private Transform m_spawnPoint;
		private float m_minDelay;
		private float m_startMaxDelay;
		private float m_currentMaxDelay;
		private float m_decreaseStep;
		private float m_timer;
		private bool m_isActive;
		private List<GameObject> m_stones = new();

		public StoneSpawner(
			GameObject stonePrefab,
			Transform spawnPoint,
			float minDelay,
			float maxDelay,
			float decreaseStepMs)
		{
			m_stonePrefab = stonePrefab;
			m_spawnPoint = spawnPoint;
			m_minDelay = minDelay;
			m_startMaxDelay = maxDelay;
			m_decreaseStep = decreaseStepMs / 1000f;
		}

		public void StartSpawning()
		{
			if (m_stones.Count == 0)
			{
				m_currentMaxDelay = m_startMaxDelay;
				SetNextTimer();
			}

			m_isActive = true;
		}

		public void StopSpawning()
		{
			m_isActive = false;
		}

		public void ClearStones()
		{
			foreach (GameObject stone in m_stones)
			{
				if (stone != null)
				{
					Object.Destroy(stone);
				}
			}

			m_stones.Clear();
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
				m_currentMaxDelay = Mathf.Max(m_minDelay, m_currentMaxDelay - m_decreaseStep);
				SetNextTimer();
			}
		}

		private void SpawnStone()
		{
			GameObject stone = Object.Instantiate(m_stonePrefab, m_spawnPoint.position, m_spawnPoint.rotation);
			m_stones.Add(stone);
		}

		private void SetNextTimer()
		{
			m_timer = Random.Range(m_minDelay, m_currentMaxDelay);
		}
	}
}