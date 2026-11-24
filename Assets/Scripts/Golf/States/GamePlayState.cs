using Golf.GamePlay;
using Golf.States.Interfaces;
using Golf.UI;

namespace Golf.States
{
	public class GamePlayState : IGameState, IUpdatable, IFixedUpdatable
	{
		private StateMachine m_stateMachine;
		private HitButton m_hitButton;
		private PauseView m_pauseView;
		private ScoreView m_scoreView;
		private FirstPersonCamera m_firstPersonCamera;
		private PlayerCannon m_playerCannon;
		private ShipMover m_shipMover;
		private EnemySpawner m_enemySpawner;
		private PlayerShipHealth m_playerHealth;
		private ScoreCounter m_scoreCounter;
		private AudioPlayer m_audioPlayer;

		public GamePlayState(
			StateMachine stateMachine,
			HitButton hitButton,
			PauseView pauseView,
			ScoreView scoreView,
			FirstPersonCamera firstPersonCamera,
			PlayerCannon playerCannon,
			ShipMover shipMover,
			EnemySpawner enemySpawner,
			PlayerShipHealth playerHealth,
			ScoreCounter scoreCounter,
			AudioPlayer audioPlayer)
		{
			m_stateMachine = stateMachine;
			m_hitButton = hitButton;
			m_pauseView = pauseView;
			m_scoreView = scoreView;
			m_firstPersonCamera = firstPersonCamera;
			m_playerCannon = playerCannon;
			m_shipMover = shipMover;
			m_enemySpawner = enemySpawner;
			m_playerHealth = playerHealth;
			m_scoreCounter = scoreCounter;
			m_audioPlayer = audioPlayer;
		}

		public void Enter()
		{
			m_hitButton.onPressChanged += OnPressChangedHandler;
			m_pauseView.onPauseClicked += OnPauseClickedHandler;
			m_shipMover.onPortReached += OnPortReachedHandler;
			m_playerCannon.onShot += OnPlayerShotHandler;
			m_enemySpawner.onEnemyShot += OnEnemyShotHandler;
			m_enemySpawner.onEnemyHit += OnEnemyHitHandler;
			m_enemySpawner.onEnemySunk += OnEnemySunkHandler;
			m_enemySpawner.onPlayerHit += OnPlayerHitHandler;
			m_playerHealth.onDied += OnPlayerDiedHandler;
			m_scoreCounter.onScoreChanged += OnScoreChangedHandler;

			m_hitButton.Show();
			m_pauseView.ShowPauseButton();
			m_scoreView.Show(m_scoreCounter.currentScore);
		}

		public void Exit()
		{
			m_firstPersonCamera.StopDragging();
			m_playerCannon.CancelCharge();
			m_hitButton.onPressChanged -= OnPressChangedHandler;
			m_pauseView.onPauseClicked -= OnPauseClickedHandler;
			m_shipMover.onPortReached -= OnPortReachedHandler;
			m_playerCannon.onShot -= OnPlayerShotHandler;
			m_enemySpawner.onEnemyShot -= OnEnemyShotHandler;
			m_enemySpawner.onEnemyHit -= OnEnemyHitHandler;
			m_enemySpawner.onEnemySunk -= OnEnemySunkHandler;
			m_enemySpawner.onPlayerHit -= OnPlayerHitHandler;
			m_playerHealth.onDied -= OnPlayerDiedHandler;
			m_scoreCounter.onScoreChanged -= OnScoreChangedHandler;

			m_hitButton.Hide();
			m_pauseView.HidePauseButton();
			m_scoreView.Hide();
		}

		public void Update()
		{
			m_firstPersonCamera.Update();
			m_playerCannon.UpdateAim(m_firstPersonCamera.yawOffset, m_firstPersonCamera.pitchOffset);
			m_playerCannon.Update();
			m_enemySpawner.Update();
		}

		public void FixedUpdate()
		{
			m_shipMover.FixedUpdate();
			m_enemySpawner.FixedUpdate(m_playerCannon.cannonballs);
		}

		private void OnPressChangedHandler(bool isPressed)
		{
			if (isPressed)
			{
				m_playerCannon.StartCharging();
			}
			else
			{
				m_playerCannon.ReleaseAndShoot();
			}
		}

		private void OnPauseClickedHandler()
		{
			m_stateMachine.ChangeState<PauseState>();
		}

		private void OnPlayerShotHandler()
		{
			m_audioPlayer.PlayPlayerShot();
		}

		private void OnEnemyShotHandler()
		{
			m_audioPlayer.PlayEnemyShot();
		}

		private void OnEnemyHitHandler(float distance)
		{
			m_audioPlayer.PlayHit();
			m_scoreCounter.AddHitScore(distance);
		}

		private void OnEnemySunkHandler()
		{
			m_scoreCounter.AddDestroyScore();
		}

		private void OnScoreChangedHandler(int score)
		{
			m_scoreView.UpdateScore(score);
		}

		private void OnPlayerHitHandler()
		{
			m_audioPlayer.PlayHit();
			m_playerHealth.TakeDamage(1);
		}

		private void OnPlayerDiedHandler()
		{
			m_stateMachine.ChangeState<GameOverState>();
		}

		private void OnPortReachedHandler()
		{
			m_stateMachine.ChangeState<VictoryState>();
		}
	}
}