using Golf.GamePlay;
using Golf.States;
using Golf.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Golf
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private GameObject m_mainMenuPanel;
		[SerializeField] private Button m_playButton;
		[SerializeField] private Button m_hitButton;
		[SerializeField] private Button m_pauseButton;
		[SerializeField] private GameObject m_pausePanel;
		[SerializeField] private Button m_resumeButton;
		[SerializeField] private Button m_pauseMainMenuButton;
		[SerializeField] private GameObject m_victoryPanel;
		[SerializeField] private Button m_victoryMainMenuButton;
		[SerializeField] private GameObject m_gameOverPanel;
		[SerializeField] private Button m_gameOverMainMenuButton;
		[SerializeField] private Transform m_cameraTransform;
		[SerializeField] private float m_cameraSensitivity = 0.25f;
		[SerializeField] private float m_maxHorizontalAngle = 80f;
		[SerializeField] private float m_minVerticalAngle = -25f;
		[SerializeField] private float m_maxVerticalAngle = 45f;
		[SerializeField] private Transform m_cannonBase;
		[SerializeField] private Transform m_cannonBarrel;
		[SerializeField] private Transform m_cannonMuzzle;
		[SerializeField] private GameObject m_cannonballPrefab;
		[SerializeField] private float m_minShootForce = 10f;
		[SerializeField] private float m_maxShootForce = 32f;
		[SerializeField] private float m_maxChargeTime = 1.5f;
		[SerializeField] private float m_reloadDuration = 2f;
		[SerializeField] private Rigidbody m_playerShipRigidbody;
		[SerializeField] private int m_playerMaxHealth = 5;
		[SerializeField] private Transform[] m_routeWaypoints;
		[SerializeField] private float m_shipMoveSpeed = 4f;
		[SerializeField] private float m_shipTurnSpeed = 2f;
		[SerializeField] private GameObject m_enemyShipPrefab;
		[SerializeField] private Transform[] m_enemySpawnPoints;
		[SerializeField] private float m_enemySpawnDistance = 65f;
		[SerializeField] private int m_enemyHealth = 2;
		[SerializeField] private float m_enemyMoveSpeed = 2.5f;
		[SerializeField] private float m_enemyTurnSpeed = 1.5f;
		[SerializeField] private float m_enemyCombatDistance = 35f;
		[SerializeField] private float m_enemyShootRange = 55f;
		[SerializeField] private float m_enemyShootInterval = 4f;

		private StateMachine m_stateMachine;

		private void Awake()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 60;

			m_stateMachine = new StateMachine();

			var mainMenuView = new MainMenuView(m_mainMenuPanel, m_playButton);
			var hitButton = new HitButton(m_hitButton);
			var pauseView = new PauseView(m_pauseButton, m_pausePanel, m_resumeButton, m_pauseMainMenuButton);
			var victoryView = new EndGameView(m_victoryPanel, m_victoryMainMenuButton);
			var gameOverView = new EndGameView(m_gameOverPanel, m_gameOverMainMenuButton);

			var firstPersonCamera = new FirstPersonCamera(
				m_cameraTransform,
				m_cameraSensitivity,
				m_maxHorizontalAngle,
				m_minVerticalAngle,
				m_maxVerticalAngle);

			var playerCannon = new PlayerCannon(
				m_cannonBase,
				m_cannonBarrel,
				m_cannonMuzzle,
				m_cannonballPrefab,
				m_minShootForce,
				m_maxShootForce,
				m_maxChargeTime,
				m_reloadDuration);

			var shipMover = new ShipMover(
				m_playerShipRigidbody,
				m_routeWaypoints,
				m_shipMoveSpeed,
				m_shipTurnSpeed);

			var playerHealth = new PlayerShipHealth(m_playerMaxHealth);

			var enemySpawner = new EnemySpawner(
				m_enemyShipPrefab,
				m_enemySpawnPoints,
				m_playerShipRigidbody.transform,
				m_cannonballPrefab,
				m_enemySpawnDistance,
				m_enemyHealth,
				m_enemyMoveSpeed,
				m_enemyTurnSpeed,
				m_enemyCombatDistance,
				m_enemyShootRange,
				m_enemyShootInterval);

			var mainMenuState = new MainMenuState(m_stateMachine, mainMenuView);

			var gamePlayState = new GamePlayState(
				m_stateMachine,
				hitButton,
				pauseView,
				firstPersonCamera,
				playerCannon,
				shipMover,
				enemySpawner,
				playerHealth);

			var pauseState = new PauseState(
				m_stateMachine,
				pauseView,
				firstPersonCamera,
				playerCannon,
				shipMover,
				enemySpawner,
				playerHealth);

			var victoryState = new VictoryState(
				m_stateMachine,
				victoryView,
				firstPersonCamera,
				playerCannon,
				shipMover,
				enemySpawner,
				playerHealth);

			var gameOverState = new GameOverState(
				m_stateMachine,
				gameOverView,
				firstPersonCamera,
				playerCannon,
				shipMover,
				enemySpawner,
				playerHealth);

			m_stateMachine.AddState(mainMenuState);
			m_stateMachine.AddState(gamePlayState);
			m_stateMachine.AddState(pauseState);
			m_stateMachine.AddState(victoryState);
			m_stateMachine.AddState(gameOverState);
		}

		private void Start()
		{
			m_hitButton.gameObject.SetActive(false);
			m_pauseButton.gameObject.SetActive(false);
			m_pausePanel.SetActive(false);
			m_victoryPanel.SetActive(false);
			m_gameOverPanel.SetActive(false);

			m_stateMachine.ChangeState<MainMenuState>();
		}

		private void Update()
		{
			m_stateMachine.Update();
		}

		private void FixedUpdate()
		{
			m_stateMachine.FixedUpdate();
		}
	}
}