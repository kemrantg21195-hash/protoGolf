using System;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.UI
{
	public class EndGameView
	{
		public event Action onMainMenuClicked;

		private GameObject m_panel;
		private Button m_mainMenuButton;

		public EndGameView(GameObject panel, Button mainMenuButton)
		{
			m_panel = panel;
			m_mainMenuButton = mainMenuButton;
		}

		public void Show()
		{
			m_panel.SetActive(true);
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