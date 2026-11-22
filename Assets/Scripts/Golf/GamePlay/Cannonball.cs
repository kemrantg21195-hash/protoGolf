using UnityEngine;

namespace Golf.GamePlay
{
	public class Cannonball
	{
		public bool isExpired => m_lifeTime <= 0f;
		public bool isValid => m_gameObject != null && m_lifeTime > 0f;
		public Vector3 position => m_gameObject != null ? m_gameObject.transform.position : Vector3.zero;

		private GameObject m_gameObject;
		private Rigidbody m_rigidbody;
		private float m_lifeTime = 5f;

		public Cannonball(GameObject gameObject, Rigidbody rigidbody)
		{
			m_gameObject = gameObject;
			m_rigidbody = rigidbody;
		}

		public void Launch(Vector3 impulse)
		{
			if (m_rigidbody != null)
			{
				m_rigidbody.AddForce(impulse, ForceMode.Impulse);
			}
		}

		public void UpdateLifeTime(float deltaTime)
		{
			m_lifeTime -= deltaTime;
		}

		public void Expire()
		{
			m_lifeTime = 0f;
		}

		public void Destroy()
		{
			if (m_gameObject != null)
			{
				Object.Destroy(m_gameObject);
			}
		}
	}
}