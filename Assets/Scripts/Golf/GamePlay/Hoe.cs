using Golf.States.Interfaces;
using UnityEngine;

namespace Golf.GamePlay
{
	public class Hoe : IUpdatable
	{
		private Rigidbody m_rigidbody;
		private Collider m_collider;
		private Transform m_pointA;
		private Transform m_pointB;
		private float m_forwardSpeed;
		private float m_returnSpeed;
		private float m_hitForce;
		private float m_progress;
		private bool m_isPressed;

		public Hoe(
			Rigidbody rigidbody,
			Transform pointA,
			Transform pointB,
			float forwardSpeed,
			float returnSpeed,
			float hitForce = 10f)
		{
			m_rigidbody = rigidbody;
			m_collider = rigidbody.GetComponentInChildren<Collider>();
			m_pointA = pointA;
			m_pointB = pointB;
			m_forwardSpeed = forwardSpeed;
			m_returnSpeed = returnSpeed;
			m_hitForce = hitForce;
		}

		public void SetPressed(bool isPressed)
		{
			m_isPressed = isPressed;
		}

		public void ResetToStart()
		{
			m_isPressed = false;
			m_progress = 0f;
			m_rigidbody.transform.SetPositionAndRotation(m_pointA.position, m_pointA.rotation);
			m_rigidbody.position = m_pointA.position;
			m_rigidbody.rotation = m_pointA.rotation;
		}

		public void Update()
		{
			float speed = m_isPressed ? m_forwardSpeed : m_returnSpeed;
			float target = m_isPressed ? 1f : 0f;

			m_progress = Mathf.MoveTowards(m_progress, target, speed * Time.deltaTime);

			Vector3 position = Vector3.Lerp(m_pointA.position, m_pointB.position, m_progress);
			Quaternion rotation = Quaternion.Slerp(m_pointA.rotation, m_pointB.rotation, m_progress);

			m_rigidbody.MovePosition(position);
			m_rigidbody.MoveRotation(rotation);

			if (m_isPressed && m_progress < 1f)
			{
				CheckHit();
			}
		}

		private void CheckHit()
		{
			if (m_collider == null)
			{
				return;
			}

			Bounds bounds = m_collider.bounds;
			Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.extents, m_rigidbody.rotation);
			Vector3 direction = ((m_pointB.position - m_pointA.position).normalized + Vector3.up * 0.5f).normalized;

			foreach (Collider hit in colliders)
			{
				if (hit == m_collider)
				{
					continue;
				}

				Rigidbody rb = hit.attachedRigidbody;

				if (rb != null && !rb.isKinematic)
				{
					rb.AddForce(direction * m_hitForce, ForceMode.Impulse);
				}
			}
		}
	}
}