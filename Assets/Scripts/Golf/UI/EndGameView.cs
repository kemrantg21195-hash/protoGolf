using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.UI
{
	public class EndGameView
	{
		public event Action onMainMenuClicked;

		private GameObject m_panel;
		private Button m_mainMenuButton;
		private TMP_Text m_finalScoreText;

		public EndGameView(GameObject panel, Button mainMenuButton, TMP_Text finalScoreText = null)
		{
			m_panel = panel;
			m_mainMenuButton = mainMenuButton;
			m_finalScoreText = finalScoreText;
		}

		public void Show(int finalScore)
		{
			m_panel.SetActive(true);

			if (m_finalScoreText != null)
			{
				m_finalScoreText.text = $"Итоговый счёт: {finalScore}";
			}

			m_mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
		}

		public void Hide()
		{
			m_mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);
			m_panel.SetActive(false);
		}

		private void OnMainMenuButtonClick()
		{
			onMainMenuClicked?.Invoke();
		}
	}
}