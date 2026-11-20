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
		private PlayerCannon m_playerCannon;

		public GamePlayState(
			StateMachine stateMachine,
			HitButton hitButton,
			PauseView pauseView,
			FirstPersonCamera firstPersonCamera,
			PlayerCannon playerCannon)
		{
			m_stateMachine = stateMachine;
			m_hitButton = hitButton;
			m_pauseView = pauseView;
			m_firstPersonCamera = firstPersonCamera;
			m_playerCannon = playerCannon;
		}

		public void Enter()
		{
			m_hitButton.onPressChanged += OnPressChangedHandler;
			m_pauseView.onPauseClicked += OnPauseClickedHandler;

			m_hitButton.Show();
			m_pauseView.ShowPauseButton();
		}

		public void Exit()
		{
			m_firstPersonCamera.StopDragging();
			m_hitButton.onPressChanged -= OnPressChangedHandler;
			m_pauseView.onPauseClicked -= OnPauseClickedHandler;

			m_hitButton.Hide();
			m_pauseView.HidePauseButton();
		}

		public void Update()
		{
			m_firstPersonCamera.Update();
			m_playerCannon.UpdateAim(m_firstPersonCamera.yawOffset, m_firstPersonCamera.pitchOffset);
			m_playerCannon.Update();
		}

		private void OnPressChangedHandler(bool isPressed)
		{
			if (isPressed)
			{
				m_playerCannon.Shoot();
			}
		}

		private void OnPauseClickedHandler()
		{
			m_stateMachine.ChangeState<PauseState>();
		}
	}
}