using Golf.GamePlay;
using Golf.States.Interfaces;
using Golf.UI;
using UnityEngine;

namespace Golf.States
{
	public class PauseState : IGameState
	{
		private StateMachine m_stateMachine;
		private PauseView m_pauseView;
		private SettingsState m_settingsState;
		private FirstPersonCamera m_firstPersonCamera;
		private PlayerCannon m_playerCannon;
		private ShipMover m_shipMover;
		private EnemySpawner m_enemySpawner;
		private PlayerShipHealth m_playerHealth;
		private ScoreCounter m_scoreCounter;

		public PauseState(
			StateMachine stateMachine,
			PauseView pauseView,
			SettingsState settingsState,
			FirstPersonCamera firstPersonCamera,
			PlayerCannon playerCannon,
			ShipMover shipMover,
			EnemySpawner enemySpawner,
			PlayerShipHealth playerHealth,
			ScoreCounter scoreCounter)
		{
			m_stateMachine = stateMachine;
			m_pauseView = pauseView;
			m_settingsState = settingsState;
			m_firstPersonCamera = firstPersonCamera;
			m_playerCannon = playerCannon;
			m_shipMover = shipMover;
			m_enemySpawner = enemySpawner;
			m_playerHealth = playerHealth;
			m_scoreCounter = scoreCounter;
		}

		public void Enter()
		{
			Time.timeScale = 0f;
			m_pauseView.onResumeClicked += OnResumeClickedHandler;
			m_pauseView.onSettingsClicked += OnSettingsClickedHandler;
			m_pauseView.onMainMenuClicked += OnMainMenuClickedHandler;
			m_pauseView.ShowPauseMenu();
		}

		public void Exit()
		{
			Time.timeScale = 1f;
			m_pauseView.onResumeClicked -= OnResumeClickedHandler;
			m_pauseView.onSettingsClicked -= OnSettingsClickedHandler;
			m_pauseView.onMainMenuClicked -= OnMainMenuClickedHandler;
			m_pauseView.HidePauseMenu();
		}

		private void OnResumeClickedHandler()
		{
			m_stateMachine.ChangeState<GamePlayState>();
		}

		private void OnSettingsClickedHandler()
		{
			m_settingsState.SetReturnToPause(true);
			m_stateMachine.ChangeState<SettingsState>();
		}

		private void OnMainMenuClickedHandler()
		{
			m_playerCannon.ClearCannonballs();
			m_playerCannon.ResetToStart();
			m_firstPersonCamera.ResetToStart();
			m_shipMover.ResetToStart();
			m_enemySpawner.ClearAll();
			m_playerHealth.ResetHealth();
			m_scoreCounter.ResetScore();
			m_stateMachine.ChangeState<MainMenuState>();
		}
	}
}