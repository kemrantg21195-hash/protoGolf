using TMPro;

namespace Golf.UI
{
	public class ScoreView
	{
		private TMP_Text m_scoreText;

		public ScoreView(TMP_Text scoreText)
		{
			m_scoreText = scoreText;
		}

		public void Show(int score)
		{
			if (m_scoreText != null)
			{
				m_scoreText.gameObject.SetActive(true);
				UpdateScore(score);
			}
		}

		public void Hide()
		{
			if (m_scoreText != null)
			{
				m_scoreText.gameObject.SetActive(false);
			}
		}

		public void UpdateScore(int score)
		{
			if (m_scoreText != null)
			{
				m_scoreText.text = $"Счёт: {score}";
			}
		}
	}
}