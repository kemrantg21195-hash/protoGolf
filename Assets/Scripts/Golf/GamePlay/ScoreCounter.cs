using System;
using UnityEngine;

namespace Golf.GamePlay
{
	public class ScoreCounter
	{
		private const string BestScoreKey = "BestScore";

		public event Action<int> onScoreChanged;

		public int currentScore => m_currentScore;
		public int bestScore => m_bestScore;

		private int m_baseHitPoints;
		private float m_pointsPerMeter;
		private int m_destroyBonusPoints;
		private int m_currentScore;
		private int m_bestScore;

		public ScoreCounter(int baseHitPoints, float pointsPerMeter, int destroyBonusPoints)
		{
			m_baseHitPoints = baseHitPoints;
			m_pointsPerMeter = pointsPerMeter;
			m_destroyBonusPoints = destroyBonusPoints;
			m_bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
		}

		public void AddHitScore(float distance)
		{
			int distancePoints = Mathf.Max(0, Mathf.RoundToInt(distance * m_pointsPerMeter));
			m_currentScore += m_baseHitPoints + distancePoints;
			UpdateBestScore();
			onScoreChanged?.Invoke(m_currentScore);
		}

		public void AddDestroyScore()
		{
			m_currentScore += m_destroyBonusPoints;
			UpdateBestScore();
			onScoreChanged?.Invoke(m_currentScore);
		}

		public void ResetScore()
		{
			m_currentScore = 0;
			onScoreChanged?.Invoke(m_currentScore);
		}

		private void UpdateBestScore()
		{
			if (m_currentScore > m_bestScore)
			{
				m_bestScore = m_currentScore;
				PlayerPrefs.SetInt(BestScoreKey, m_bestScore);
				PlayerPrefs.Save();
			}
		}
	}
}