using Golf.GamePlay;
using Golf.States.Interfaces;
using Golf.UI;
using UnityEngine;

namespace Golf.States
{
	public class SettingsState : IGameState
	{
		private StateMachine m_stateMachine;
		private SettingsView m_settingsView;
		private GameSettings m_gameSettings;
		private FirstPersonCamera m_firstPersonCamera;
		private EnemySpawner m_enemySpawner;
		private AudioPlayer m_audioPlayer;
		private bool m_returnToPause;

		public SettingsState(
			StateMachine stateMachine,
			SettingsView settingsView,
			GameSettings gameSettings,
			FirstPersonCamera firstPersonCamera,
			EnemySpawner enemySpawner,
			AudioPlayer audioPlayer)
		{
			m_stateMachine = stateMachine;
			m_settingsView = settingsView;
			m_gameSettings = gameSettings;
			m_firstPersonCamera = firstPersonCamera;
			m_enemySpawner = enemySpawner;
			m_audioPlayer = audioPlayer;
		}

		public void SetReturnToPause(bool returnToPause)
		{
			m_returnToPause = returnToPause;
		}

		public void Enter()
		{
			if (m_returnToPause)
			{
				Time.timeScale = 0f;
			}

			m_settingsView.onSensitivityChanged += OnSensitivityChangedHandler;
			m_settingsView.onDifficultyChanged += OnDifficultyChangedHandler;
			m_settingsView.onSoundVolumeChanged += OnSoundVolumeChangedHandler;
			m_settingsView.onBackClicked += OnBackClickedHandler;

			m_settingsView.Show(
				m_gameSettings.cameraSensitivity,
				m_gameSettings.botDifficulty,
				m_gameSettings.soundVolume);
		}

		public void Exit()
		{
			m_settingsView.onSensitivityChanged -= OnSensitivityChangedHandler;
			m_settingsView.onDifficultyChanged -= OnDifficultyChangedHandler;
			m_settingsView.onSoundVolumeChanged -= OnSoundVolumeChangedHandler;
			m_settingsView.onBackClicked -= OnBackClickedHandler;

			m_settingsView.Hide();
		}

		private void OnSensitivityChangedHandler(float value)
		{
			m_gameSettings.SetCameraSensitivity(value);
			m_firstPersonCamera.SetSensitivity(m_gameSettings.cameraSensitivity);
		}

		private void OnDifficultyChangedHandler(BotDifficulty difficulty)
		{
			m_gameSettings.SetBotDifficulty(difficulty);
			m_enemySpawner.SetDifficulty(m_gameSettings.botDifficulty);
		}

		private void OnSoundVolumeChangedHandler(float value)
		{
			m_gameSettings.SetSoundVolume(value);
			m_audioPlayer.SetVolume(m_gameSettings.soundVolume);
		}

		private void OnBackClickedHandler()
		{
			if (m_returnToPause)
			{
				m_stateMachine.ChangeState<PauseState>();
			}
			else
			{
				m_stateMachine.ChangeState<MainMenuState>();
			}
		}
	}
}