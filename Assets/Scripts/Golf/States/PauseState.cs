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
		private StoneSpawner m_stoneSpawner;
		private Hoe m_hoe;

		public PauseState(
			StateMachine stateMachine,
			PauseView pauseView,
			StoneSpawner stoneSpawner,
			Hoe hoe)
		{
			m_stateMachine = stateMachine;
			m_pauseView = pauseView;
			m_stoneSpawner = stoneSpawner;
			m_hoe = hoe;
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
			m_stoneSpawner.ClearStones();
			m_hoe.ResetToStart();
			m_stateMachine.ChangeState<MainMenuState>();
		}
	}
}