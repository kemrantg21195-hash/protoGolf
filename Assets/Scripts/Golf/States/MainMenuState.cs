using Golf.GamePlay;
using Golf.States.Interfaces;
using Golf.UI;

namespace Golf.States
{
	public class MainMenuState : IGameState
	{
		private StateMachine m_stateMachine;
		private MainMenuView m_mainMenuView;
		private SettingsState m_settingsState;
		private ScoreCounter m_scoreCounter;

		public MainMenuState(
			StateMachine stateMachine,
			MainMenuView mainMenuView,
			SettingsState settingsState,
			ScoreCounter scoreCounter)
		{
			m_stateMachine = stateMachine;
			m_mainMenuView = mainMenuView;
			m_settingsState = settingsState;
			m_scoreCounter = scoreCounter;
		}

		public void Enter()
		{
			m_mainMenuView.onPlayClicked += OnPlayClickedHandler;
			m_mainMenuView.onSettingsClicked += OnSettingsClickedHandler;
			m_mainMenuView.Show(m_scoreCounter.bestScore);
		}

		public void Exit()
		{
			m_mainMenuView.onPlayClicked -= OnPlayClickedHandler;
			m_mainMenuView.onSettingsClicked -= OnSettingsClickedHandler;
			m_mainMenuView.Hide();
		}

		private void OnPlayClickedHandler()
		{
			m_stateMachine.ChangeState<GamePlayState>();
		}

		private void OnSettingsClickedHandler()
		{
			m_settingsState.SetReturnToPause(false);
			m_stateMachine.ChangeState<SettingsState>();
		}
	}
}