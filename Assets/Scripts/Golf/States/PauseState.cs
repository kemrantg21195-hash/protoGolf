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
		private FirstPersonCamera m_firstPersonCamera;
		private PlayerCannon m_playerCannon;

		public PauseState(
			StateMachine stateMachine,
			PauseView pauseView,
			FirstPersonCamera firstPersonCamera,
			PlayerCannon playerCannon)
		{
			m_stateMachine = stateMachine;
			m_pauseView = pauseView;
			m_firstPersonCamera = firstPersonCamera;
			m_playerCannon = playerCannon;
		}

		public void Enter()
		{
			Time.timeScale = 0f;
			m_pauseView.onResumeClicked += OnResumeClickedHandler;
			m_pauseView.onMainMenuClicked += OnMainMenuClickedHandler;
			m_pauseView.ShowPauseMenu();
		}

		public void Exit()
		{
			Time.timeScale = 1f;
			m_pauseView.onResumeClicked -= OnResumeClickedHandler;
			m_pauseView.onMainMenuClicked -= OnMainMenuClickedHandler;
			m_pauseView.HidePauseMenu();
		}

		private void OnResumeClickedHandler()
		{
			m_stateMachine.ChangeState<GamePlayState>();
		}

		private void OnMainMenuClickedHandler()
		{
			m_playerCannon.ClearCannonballs();
			m_playerCannon.ResetToStart();
			m_firstPersonCamera.ResetToStart();
			m_stateMachine.ChangeState<MainMenuState>();
		}
	}
}