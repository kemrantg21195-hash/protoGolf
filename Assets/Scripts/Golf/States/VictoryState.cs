using Golf.GamePlay;
using Golf.States.Interfaces;
using Golf.UI;
using UnityEngine;

namespace Golf.States
{
	public class VictoryState : IGameState
	{
		private StateMachine m_stateMachine;
		private EndGameView m_victoryView;
		private FirstPersonCamera m_firstPersonCamera;
		private PlayerCannon m_playerCannon;
		private ShipMover m_shipMover;
		private EnemySpawner m_enemySpawner;
		private PlayerShipHealth m_playerHealth;
		private ScoreCounter m_scoreCounter;

		public VictoryState(
			StateMachine stateMachine,
			EndGameView victoryView,
			FirstPersonCamera firstPersonCamera,
			PlayerCannon playerCannon,
			ShipMover shipMover,
			EnemySpawner enemySpawner,
			PlayerShipHealth playerHealth,
			ScoreCounter scoreCounter)
		{
			m_stateMachine = stateMachine;
			m_victoryView = victoryView;
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
			m_victoryView.onMainMenuClicked += OnMainMenuClickedHandler;
			m_victoryView.Show(m_scoreCounter.currentScore);
		}

		public void Exit()
		{
			Time.timeScale = 1f;
			m_victoryView.onMainMenuClicked -= OnMainMenuClickedHandler;
			m_victoryView.Hide();
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