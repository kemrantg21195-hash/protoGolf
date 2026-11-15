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
		private Hoe m_hoe;
		private StoneSpawner m_stoneSpawner;

		public GamePlayState(
			StateMachine stateMachine,
			HitButton hitButton,
			PauseView pauseView,
			Hoe hoe,
			StoneSpawner stoneSpawner)
		{
			m_stateMachine = stateMachine;
			m_hitButton = hitButton;
			m_pauseView = pauseView;
			m_hoe = hoe;
			m_stoneSpawner = stoneSpawner;
		}

		public void Enter()
		{
			m_hitButton.onPressChanged += OnPressChangedHandler;
			m_pauseView.onPauseClicked += OnPauseClickedHandler;

			m_hitButton.Show();
			m_pauseView.ShowPauseButton();
			m_stoneSpawner.StartSpawning();
		}

		public void Exit()
		{
			m_stoneSpawner.StopSpawning();
			m_hitButton.onPressChanged -= OnPressChangedHandler;
			m_pauseView.onPauseClicked -= OnPauseClickedHandler;

			m_hitButton.Hide();
			m_pauseView.HidePauseButton();
		}

		public void Update()
		{
			m_hoe.Update();
			m_stoneSpawner.Update();
		}

		private void OnPressChangedHandler(bool isPressed)
		{
			m_hoe.SetPressed(isPressed);
		}

		private void OnPauseClickedHandler()
		{
			m_stateMachine.ChangeState<PauseState>();
		}
	}
}