using Golf.GamePlay;
using Golf.States.Interfaces;
using Golf.UI;

namespace Golf.States
{
	public class GamePlayState : IGameState, IUpdatable
	{
		private readonly HitButton m_hitButton;
		private readonly Hoe m_hoe;
		private readonly StoneSpawner m_stoneSpawner;

		public GamePlayState(
			HitButton hitButton,
			Hoe hoe,
			StoneSpawner stoneSpawner)
		{
			m_hitButton = hitButton;
			m_hoe = hoe;
			m_stoneSpawner = stoneSpawner;
		}

		public void Enter()
		{
			m_hoe.ResetToStart();
			m_hitButton.onPressChanged += OnHitButtonPressChangedHandler;
			m_hitButton.Show();
			m_stoneSpawner.StartSpawning();
		}

		public void Exit()
		{
			m_stoneSpawner.StopSpawning();
			m_hitButton.onPressChanged -= OnHitButtonPressChangedHandler;
			m_hitButton.Hide();
			m_hoe.ResetToStart();
		}

		public void Update()
		{
			m_hoe.Update();
			m_stoneSpawner.Update();
		}

		private void OnHitButtonPressChangedHandler(bool isPressed)
		{
			m_hoe.SetPressed(isPressed);
		}
	}
}