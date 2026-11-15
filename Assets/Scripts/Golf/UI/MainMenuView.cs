using System;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.UI
{
	public class MainMenuView
	{
		public event Action onPlayClicked;

		private readonly GameObject m_menuPanel;
		private readonly Button m_playButton;

		public MainMenuView(GameObject menuPanel, Button playButton)
		{
			m_menuPanel = menuPanel;
			m_playButton = playButton;
		}

		public void Show()
		{
			m_menuPanel.SetActive(true);
			m_playButton.onClick.AddListener(OnPlayButtonClickedHandler);
		}

		public void Hide()
		{
			m_playButton.onClick.RemoveListener(OnPlayButtonClickedHandler);
			m_menuPanel.SetActive(false);
		}

		private void OnPlayButtonClickedHandler()
		{
			onPlayClicked?.Invoke();
		}
	}
}