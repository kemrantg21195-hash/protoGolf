using System;
using UnityEngine;

namespace Golf.GamePlay
{
	public class PlayerShipHealth
	{
		public event Action<int, int> onHealthChanged;
		public event Action onDied;

		public int currentHealth => m_currentHealth;
		public int maxHealth => m_maxHealth;
		public bool isDead => m_currentHealth <= 0;

		private int m_maxHealth;
		private int m_currentHealth;

		public PlayerShipHealth(int maxHealth)
		{
			m_maxHealth = Mathf.Max(1, maxHealth);
			m_currentHealth = m_maxHealth;
		}

		public void TakeDamage(int damage)
		{
			if (isDead)
			{
				return;
			}

			m_currentHealth = Mathf.Max(0, m_currentHealth - damage);
			onHealthChanged?.Invoke(m_currentHealth, m_maxHealth);

			if (m_currentHealth <= 0)
			{
				onDied?.Invoke();
			}
		}

		public void ResetHealth()
		{
			m_currentHealth = m_maxHealth;
			onHealthChanged?.Invoke(m_currentHealth, m_maxHealth);
		}
	}
}