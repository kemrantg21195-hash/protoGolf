using System;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.UI
{
	public class PauseView
	{
		public event Action onPauseClicked;
		public event Action onResumeClicked;
		public event Action onSettingsClicked;
		public event Action onMainMenuClicked;

		private Button m_pauseButton;
		private GameObject m_pausePanel;
		private Button m_resumeButton;
		private Button m_settingsButton;
		private Button m_mainMenuButton;

		public PauseView(
			Button pauseButton,
			GameObject pausePanel,
			Button resumeButton,
			Button settingsButton,
			Button mainMenuButton)
		{
			m_pauseButton = pauseButton;
			m_pausePanel = pausePanel;
			m_resumeButton = resumeButton;
			m_settingsButton = settingsButton;
			m_mainMenuButton = mainMenuButton;
		}

		public void ShowPauseButton()
		{
			m_pauseButton.gameObject.SetActive(true);
			m_pauseButton.onClick.AddListener(OnPauseButtonClick);
		}

		public void HidePauseButton()
		{
			m_pauseButton.onClick.RemoveListener(OnPauseButtonClick);
			m_pauseButton.gameObject.SetActive(false);
		}

		public void ShowPauseMenu()
		{
			m_pausePanel.SetActive(true);
			m_resumeButton.onClick.AddListener(OnResumeButtonClick);
			m_settingsButton.onClick.AddListener(OnSettingsButtonClick);
			m_mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
		}

		public void HidePauseMenu()
		{
			m_resumeButton.onClick.RemoveListener(OnResumeButtonClick);
			m_settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
			m_mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);
			m_pausePanel.SetActive(false);
		}

		private void OnPauseButtonClick()
		{
			onPauseClicked?.Invoke();
		}

		private void OnResumeButtonClick()
		{
			onResumeClicked?.Invoke();
		}

		private void OnSettingsButtonClick()
		{
			onSettingsClicked?.Invoke();
		}

		private void OnMainMenuButtonClick()
		{
			onMainMenuClicked?.Invoke();
		}
	}
}