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
		[SerializeField] private Rigidbody m_hoeRigidbody;
		[SerializeField] private Transform m_pointA;
		[SerializeField] private Transform m_pointB;
		[SerializeField] private float m_hoeForwardSpeed = 5f;
		[SerializeField] private float m_hoeReturnSpeed = 3f;
		[SerializeField] private float m_hoeHitForce = 10f;
		[SerializeField] private GameObject m_stonePrefab;
		[SerializeField] private Transform m_stoneSpawnPoint;
		[SerializeField] private float m_minSpawnDelay = 1f;
		[SerializeField] private float m_maxSpawnDelay = 3f;
		[SerializeField] private float m_spawnDecreaseStepMs = 100f;

		private StateMachine m_stateMachine;

		private void Awake()
		{
			m_stateMachine = new StateMachine();

			var mainMenuView = new MainMenuView(m_mainMenuPanel, m_playButton);
			var hitButton = new HitButton(m_hitButton);
			var pauseView = new PauseView(m_pauseButton, m_pausePanel, m_resumeButton, m_pauseMainMenuButton);

			var hoe = new Hoe(
				m_hoeRigidbody,
				m_pointA,
				m_pointB,
				m_hoeForwardSpeed,
				m_hoeReturnSpeed,
				m_hoeHitForce);

			var stoneSpawner = new StoneSpawner(
				m_stonePrefab,
				m_stoneSpawnPoint,
				m_minSpawnDelay,
				m_maxSpawnDelay,
				m_spawnDecreaseStepMs);

			var mainMenuState = new MainMenuState(m_stateMachine, mainMenuView);
			var gamePlayState = new GamePlayState(m_stateMachine, hitButton, pauseView, hoe, stoneSpawner);
			var pauseState = new PauseState(m_stateMachine, pauseView, stoneSpawner, hoe);

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