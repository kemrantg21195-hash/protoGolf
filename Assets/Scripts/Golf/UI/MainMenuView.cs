using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.UI
{
	public class MainMenuView
	{
		public event Action onPlayClicked;
		public event Action onSettingsClicked;

		private GameObject m_panel;
		private Button m_playButton;
		private Button m_settingsButton;
		private TMP_Text m_bestScoreText;

		public MainMenuView(
			GameObject panel,
			Button playButton,
			Button settingsButton,
			TMP_Text bestScoreText = null)
		{
			m_panel = panel;
			m_playButton = playButton;
			m_settingsButton = settingsButton;
			m_bestScoreText = bestScoreText;
		}

		public void Show(int bestScore)
		{
			m_panel.SetActive(true);

			if (m_bestScoreText != null)
			{
				m_bestScoreText.text = $"Лучший счёт: {bestScore}";
			}

			m_playButton.onClick.AddListener(OnPlayButtonClick);
			m_settingsButton.onClick.AddListener(OnSettingsButtonClick);
		}

		public void Hide()
		{
			m_playButton.onClick.RemoveListener(OnPlayButtonClick);
			m_settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
			m_panel.SetActive(false);
		}

		private void OnPlayButtonClick()
		{
			onPlayClicked?.Invoke();
		}

		private void OnSettingsButtonClick()
		{
			onSettingsClicked?.Invoke();
		}
	}
}