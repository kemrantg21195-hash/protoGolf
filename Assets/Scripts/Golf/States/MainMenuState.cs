using Golf.States.Interfaces;
using Golf.UI;

namespace Golf.States
{
	public class MainMenuState : IGameState
	{
		private StateMachine m_stateMachine;
		private MainMenuView m_mainMenuView;

		public MainMenuState(StateMachine stateMachine, MainMenuView mainMenuView)
		{
			m_stateMachine = stateMachine;
			m_mainMenuView = mainMenuView;
		}

		public void Enter()
		{
			m_mainMenuView.onPlayClicked += OnPlayClickedHandler;
			m_mainMenuView.Show();
		}

		public void Exit()
		{
			m_mainMenuView.onPlayClicked -= OnPlayClickedHandler;
			m_mainMenuView.Hide();
		}

		private void OnPlayClickedHandler()
		{
			m_stateMachine.ChangeState<GamePlayState>();
		}
	}
}