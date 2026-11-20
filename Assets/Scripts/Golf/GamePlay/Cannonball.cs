using UnityEngine;

namespace Golf.GamePlay
{
	public class Cannonball
	{
		public bool isExpired => m_lifeTime <= 0f;

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

		public void Destroy()
		{
			if (m_gameObject != null)
			{
				Object.Destroy(m_gameObject);
			}
		}
	}
}