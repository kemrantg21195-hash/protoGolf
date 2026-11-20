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
		[SerializeField] private Transform m_cameraTransform;
		[SerializeField] private float m_cameraSensitivity = 0.25f;
		[SerializeField] private float m_maxHorizontalAngle = 80f;
		[SerializeField] private float m_minVerticalAngle = -25f;
		[SerializeField] private float m_maxVerticalAngle = 45f;
		[SerializeField] private Transform m_cannonBase;
		[SerializeField] private Transform m_cannonBarrel;
		[SerializeField] private Transform m_cannonMuzzle;
		[SerializeField] private GameObject m_cannonballPrefab;
		[SerializeField] private float m_cannonShootForce = 20f;

		private StateMachine m_stateMachine;

		private void Awake()
		{
			m_stateMachine = new StateMachine();

			var mainMenuView = new MainMenuView(m_mainMenuPanel, m_playButton);
			var hitButton = new HitButton(m_hitButton);
			var pauseView = new PauseView(m_pauseButton, m_pausePanel, m_resumeButton, m_pauseMainMenuButton);

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
				m_cannonShootForce);

			var mainMenuState = new MainMenuState(m_stateMachine, mainMenuView);

			var gamePlayState = new GamePlayState(
				m_stateMachine,
				hitButton,
				pauseView,
				firstPersonCamera,
				playerCannon);

			var pauseState = new PauseState(
				m_stateMachine,
				pauseView,
				firstPersonCamera,
				playerCannon);

			m_stateMachine.AddState(mainMenuState);
			m_stateMachine.AddState(gamePlayState);
			m_stateMachine.AddState(pauseState);
		}

		private void Start()
		{
			m_hitButton.gameObject.SetActive(false);
			m_pauseButton.gameObject.SetActive(false);
			m_pausePanel.SetActive(false);

			m_stateMachine.ChangeState<MainMenuState>();
		}

		private void Update()
		{
			m_stateMachine.Update();
		}
	}
}