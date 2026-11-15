using System;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.UI
{
	public class PauseView
	{
		public event Action onPauseClicked;
		public event Action onResumeClicked;
		public event Action onMainMenuClicked;

		private Button m_pauseButton;
		private GameObject m_pausePanel;
		private Button m_resumeButton;
		private Button m_mainMenuButton;

		public PauseView(
			Button pauseButton,
			GameObject pausePanel,
			Button resumeButton,
			Button mainMenuButton)
		{
			m_pauseButton = pauseButton;
			m_pausePanel = pausePanel;
			m_resumeButton = resumeButton;
			m_mainMenuButton = mainMenuButton;
		}

		public void ShowPauseButton()
		{
			m_pauseButton.gameObject.SetActive(true);
			m_pauseButton.onClick.AddListener(OnPauseClickedHandler);
		}

		public void HidePauseButton()
		{
			m_pauseButton.onClick.RemoveListener(OnPauseClickedHandler);
			m_pauseButton.gameObject.SetActive(false);
		}

		public void ShowPauseMenu()
		{
			m_pausePanel.SetActive(true);
			m_resumeButton.onClick.AddListener(OnResumeClickedHandler);
			m_mainMenuButton.onClick.AddListener(OnMainMenuClickedHandler);
		}

		public void HidePauseMenu()
		{
			m_resumeButton.onClick.RemoveListener(OnResumeClickedHandler);
			m_mainMenuButton.onClick.RemoveListener(OnMainMenuClickedHandler);
			m_pausePanel.SetActive(false);
		}

		private void OnPauseClickedHandler()
		{
			onPauseClicked?.Invoke();
		}

		private void OnResumeClickedHandler()
		{
			onResumeClicked?.Invoke();
		}

		private void OnMainMenuClickedHandler()
		{
			onMainMenuClicked?.Invoke();
		}
	}
}