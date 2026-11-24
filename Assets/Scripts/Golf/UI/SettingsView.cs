using System;
using Golf.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.UI
{
	public class SettingsView
	{
		public event Action<float> onSensitivityChanged;
		public event Action<BotDifficulty> onDifficultyChanged;
		public event Action<float> onSoundVolumeChanged;
		public event Action onBackClicked;

		private GameObject m_panel;
		private Slider m_sensitivitySlider;
		private Button m_normalButton;
		private Button m_hardButton;
		private Button m_backButton;
		private Slider m_soundVolumeSlider;

		public SettingsView(
			GameObject panel,
			Slider sensitivitySlider,
			Button normalButton,
			Button hardButton,
			Button backButton,
			Slider soundVolumeSlider = null)
		{
			m_panel = panel;
			m_sensitivitySlider = sensitivitySlider;
			m_normalButton = normalButton;
			m_hardButton = hardButton;
			m_backButton = backButton;
			m_soundVolumeSlider = soundVolumeSlider;
		}

		public void Show(float currentSensitivity, BotDifficulty currentDifficulty, float currentSoundVolume)
		{
			m_panel.SetActive(true);

			m_sensitivitySlider.SetValueWithoutNotify(currentSensitivity);
			UpdateDifficultyButtons(currentDifficulty);

			m_sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderChanged);
			m_normalButton.onClick.AddListener(OnNormalButtonClick);
			m_hardButton.onClick.AddListener(OnHardButtonClick);
			m_backButton.onClick.AddListener(OnBackButtonClick);

			if (m_soundVolumeSlider != null)
			{
				m_soundVolumeSlider.SetValueWithoutNotify(currentSoundVolume);
				m_soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeSliderChanged);
			}
		}

		public void Hide()
		{
			m_sensitivitySlider.onValueChanged.RemoveListener(OnSensitivitySliderChanged);
			m_normalButton.onClick.RemoveListener(OnNormalButtonClick);
			m_hardButton.onClick.RemoveListener(OnHardButtonClick);
			m_backButton.onClick.RemoveListener(OnBackButtonClick);

			if (m_soundVolumeSlider != null)
			{
				m_soundVolumeSlider.onValueChanged.RemoveListener(OnSoundVolumeSliderChanged);
			}

			m_panel.SetActive(false);
		}

		private void OnSensitivitySliderChanged(float value)
		{
			onSensitivityChanged?.Invoke(value);
		}

		private void OnSoundVolumeSliderChanged(float value)
		{
			onSoundVolumeChanged?.Invoke(value);
		}

		private void OnNormalButtonClick()
		{
			SelectDifficulty(BotDifficulty.Normal);
		}

		private void OnHardButtonClick()
		{
			SelectDifficulty(BotDifficulty.Hard);
		}

		private void SelectDifficulty(BotDifficulty difficulty)
		{
			UpdateDifficultyButtons(difficulty);
			onDifficultyChanged?.Invoke(difficulty);
		}

		private void UpdateDifficultyButtons(BotDifficulty difficulty)
		{
			m_normalButton.interactable = difficulty != BotDifficulty.Normal;
			m_hardButton.interactable = difficulty != BotDifficulty.Hard;
		}

		private void OnBackButtonClick()
		{
			onBackClicked?.Invoke();
		}
	}
}