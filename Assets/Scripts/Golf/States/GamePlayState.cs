using Golf.GamePlay;
using Golf.States.Interfaces;
using Golf.UI;

namespace Golf.States
{
	public class GamePlayState : IGameState, IUpdatable
	{
		private StateMachine m_stateMachine;
		private HitButton m_hitButton;
		private PauseView m_pauseView;
		private FirstPersonCamera m_firstPersonCamera;

		public GamePlayState(
			StateMachine stateMachine,
			HitButton hitButton,
			PauseView pauseView,
			FirstPersonCamera firstPersonCamera)
		{
			m_stateMachine = stateMachine;
			m_hitButton = hitButton;
			m_pauseView = pauseView;
			m_firstPersonCamera = firstPersonCamera;
		}

		public void Enter()
		{
			m_pauseView.onPauseClicked += OnPauseClickedHandler;

			m_hitButton.Show();
			m_pauseView.ShowPauseButton();
		}

		public void Exit()
		{
			m_firstPersonCamera.StopDragging();
			m_pauseView.onPauseClicked -= OnPauseClickedHandler;

			m_hitButton.Hide();
			m_pauseView.HidePauseButton();
		}

		public void Update()
		{
			m_firstPersonCamera.Update();
		}

		private void OnPauseClickedHandler()
		{
			m_stateMachine.ChangeState<PauseState>();
		}
	}
}