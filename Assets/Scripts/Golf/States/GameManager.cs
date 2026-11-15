using Golf.GamePlay;
using Golf.States;
using Golf.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Golf
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_mainMenuPanel;

		[SerializeField]
		private Button m_playButton;

		[SerializeField]
		private GameObject m_hitButtonObject;

		[SerializeField]
		private EventTrigger m_hitButtonEventTrigger;

		[SerializeField]
		private Rigidbody m_hoeRigidbody;

		[SerializeField]
		private Transform m_pointA;

		[SerializeField]
		private Transform m_pointB;

		[SerializeField]
		private float m_hoeForwardSpeed = 5f;

		[SerializeField]
		private float m_hoeReturnSpeed = 3f;

		[SerializeField]
		private GameObject m_stonePrefab;

		[SerializeField]
		private Transform m_stoneSpawnPoint;

		[SerializeField]
		private float m_minSpawnDelay = 1f;

		[SerializeField]
		private float m_maxSpawnDelay = 3f;

		[SerializeField]
		private float m_spawnDecreaseStepMs = 100f;

		private StateMachine m_stateMachine;

		private void Awake()
		{
			m_stateMachine = new StateMachine();

			var mainMenuView = new MainMenuView(m_mainMenuPanel, m_playButton);
			var hitButton = new HitButton(m_hitButtonObject, m_hitButtonEventTrigger);

			var hoe = new Hoe(
				m_hoeRigidbody,
				m_pointA,
				m_pointB,
				m_hoeForwardSpeed,
				m_hoeReturnSpeed);

			var stoneSpawner = new StoneSpawner(
				m_stonePrefab,
				m_stoneSpawnPoint,
				m_minSpawnDelay,
				m_maxSpawnDelay,
				m_spawnDecreaseStepMs);

			var mainMenuState = new MainMenuState(m_stateMachine, mainMenuView);
			var gamePlayState = new GamePlayState(hitButton, hoe, stoneSpawner);

			m_stateMachine.AddState(mainMenuState);
			m_stateMachine.AddState(gamePlayState);
		}

		private void Start()
		{
			m_hitButtonObject.SetActive(false);
			m_stateMachine.ChangeState<MainMenuState>();
		}

		private void Update()
		{
			m_stateMachine.Update();
		}
	}
}