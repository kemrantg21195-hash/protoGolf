using Golf.GamePlay;
using Golf.States;
using Golf.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Golf
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private GameObject m_mainMenuPanel;
		[SerializeField] private Button m_playButton;
		[SerializeField] private Button m_mainMenuSettingsButton;
		[SerializeField] private TMP_Text m_bestScoreText;
		[SerializeField] private Button m_hitButton;
		[SerializeField] private Button m_pauseButton;
		[SerializeField] private TMP_Text m_hudScoreText;
		[SerializeField] private GameObject m_pausePanel;
		[SerializeField] private Button m_resumeButton;
		[SerializeField] private Button m_pauseSettingsButton;
		[SerializeField] private Button m_pauseMainMenuButton;
		[SerializeField] private GameObject m_settingsPanel;
		[SerializeField] private Slider m_sensitivitySlider;
		[SerializeField] private Slider m_soundVolumeSlider;
		[SerializeField] private Button m_normalDifficultyButton;
		[SerializeField] private Button m_hardDifficultyButton;
		[SerializeField] private Button m_settingsBackButton;
		[SerializeField] private GameObject m_victoryPanel;
		[SerializeField] private Button m_victoryMainMenuButton;
		[SerializeField] private TMP_Text m_victoryScoreText;
		[SerializeField] private GameObject m_gameOverPanel;
		[SerializeField] private Button m_gameOverMainMenuButton;
		[SerializeField] private TMP_Text m_gameOverScoreText;
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
		[SerializeField] private Collider m_seaCollider;
		[SerializeField] private Rigidbody m_playerShipRigidbody;
		[SerializeField] private int m_playerMaxHealth = 5;
		[SerializeField] private Transform[] m_routeWaypoints;
		[SerializeField] private float m_shipMoveSpeed = 4f;
		[SerializeField] private float m_shipTurnSpeed = 2f;
		[SerializeField] private GameObject m_enemyShipPrefab;
		[SerializeField] private Transform[] m_enemySpawnPoints;
		[SerializeField] private float m_enemySpawnDistance = 65f;
		[SerializeField] private BotDifficulty m_defaultDifficulty = BotDifficulty.Normal;
		[SerializeField] private int m_hardExtraHealth = 1;
		[SerializeField] private float m_hardShootRangeMultiplier = 1.3f;
		[SerializeField] private float m_hardShootIntervalMultiplier = 0.65f;
		[SerializeField] private int m_enemyHealth = 2;
		[SerializeField] private float m_enemyMoveSpeed = 2.5f;
		[SerializeField] private float m_enemyTurnSpeed = 1.5f;
		[SerializeField] private float m_enemyCombatDistance = 35f;
		[SerializeField] private float m_enemyShootRange = 55f;
		[SerializeField] private float m_enemyShootInterval = 4f;
		[SerializeField] private int m_baseHitPoints = 10;
		[SerializeField] private float m_pointsPerMeter = 2f;
		[SerializeField] private int m_destroyBonusPoints = 100;
		[SerializeField] private AudioSource m_seaAudioSource;
		[SerializeField] private AudioSource m_sfxAudioSource;
		[SerializeField] private AudioClip m_seaAmbientClip;
		[SerializeField] private AudioClip m_cannonShootClip;
		[SerializeField] private AudioClip m_hitClip;

		private StateMachine m_stateMachine;
		private AudioPlayer m_audioPlayer;

		private void Awake()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 60;

			m_stateMachine = new StateMachine();

			var gameSettings = new GameSettings(m_cameraSensitivity, m_defaultDifficulty);

			m_audioPlayer = new AudioPlayer(
				m_seaAudioSource,
				m_sfxAudioSource,
				m_seaAmbientClip,
				m_cannonShootClip,
				m_hitClip,
				gameSettings.soundVolume);

			var mainMenuView = new MainMenuView(
				m_mainMenuPanel,
				m_playButton,
				m_mainMenuSettingsButton,
				m_bestScoreText);
			var hitButton = new HitButton(m_hitButton);
			var pauseView = new PauseView(
				m_pauseButton,
				m_pausePanel,
				m_resumeButton,
				m_pauseSettingsButton,
				m_pauseMainMenuButton);
			var scoreView = new ScoreView(m_hudScoreText);
			var settingsView = new SettingsView(
				m_settingsPanel,
				m_sensitivitySlider,
				m_normalDifficultyButton,
				m_hardDifficultyButton,
				m_settingsBackButton,
				m_soundVolumeSlider);
			var victoryView = new EndGameView(m_victoryPanel, m_victoryMainMenuButton, m_victoryScoreText);
			var gameOverView = new EndGameView(m_gameOverPanel, m_gameOverMainMenuButton, m_gameOverScoreText);

			var firstPersonCamera = new FirstPersonCamera(
				m_cameraTransform,
				gameSettings.cameraSensitivity,
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
				m_reloadDuration,
				m_seaCollider);

			var shipMover = new ShipMover(
				m_playerShipRigidbody,
				m_routeWaypoints,
				m_shipMoveSpeed,
				m_shipTurnSpeed);

			var playerHealth = new PlayerShipHealth(m_playerMaxHealth);
			var scoreCounter = new ScoreCounter(m_baseHitPoints, m_pointsPerMeter, m_destroyBonusPoints);

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
				m_enemyShootInterval,
				gameSettings.botDifficulty,
				m_hardExtraHealth,
				m_hardShootRangeMultiplier,
				m_hardShootIntervalMultiplier,
				m_seaCollider);

			var settingsState = new SettingsState(
				m_stateMachine,
				settingsView,
				gameSettings,
				firstPersonCamera,
				enemySpawner,
				m_audioPlayer);

			var mainMenuState = new MainMenuState(
				m_stateMachine,
				mainMenuView,
				settingsState,
				scoreCounter);

			var gamePlayState = new GamePlayState(
				m_stateMachine,
				hitButton,
				pauseView,
				scoreView,
				firstPersonCamera,
				playerCannon,
				shipMover,
				enemySpawner,
				playerHealth,
				scoreCounter,
				m_audioPlayer);

			var pauseState = new PauseState(
				m_stateMachine,
				pauseView,
				settingsState,
				firstPersonCamera,
				playerCannon,
				shipMover,
				enemySpawner,
				playerHealth,
				scoreCounter);

			var victoryState = new VictoryState(
				m_stateMachine,
				victoryView,
				firstPersonCamera,
				playerCannon,
				shipMover,
				enemySpawner,
				playerHealth,
				scoreCounter);

			var gameOverState = new GameOverState(
				m_stateMachine,
				gameOverView,
				firstPersonCamera,
				playerCannon,
				shipMover,
				enemySpawner,
				playerHealth,
				scoreCounter);

			m_stateMachine.AddState(mainMenuState);
			m_stateMachine.AddState(gamePlayState);
			m_stateMachine.AddState(pauseState);
			m_stateMachine.AddState(settingsState);
			m_stateMachine.AddState(victoryState);
			m_stateMachine.AddState(gameOverState);
		}

		private void Start()
		{
			m_hitButton.gameObject.SetActive(false);
			m_pauseButton.gameObject.SetActive(false);

			if (m_hudScoreText != null)
			{
				m_hudScoreText.gameObject.SetActive(false);
			}

			m_pausePanel.SetActive(false);
			m_settingsPanel.SetActive(false);
			m_victoryPanel.SetActive(false);
			m_gameOverPanel.SetActive(false);

			m_audioPlayer.PlaySeaAmbient();
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