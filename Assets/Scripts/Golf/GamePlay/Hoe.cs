using Golf.States.Interfaces;
using UnityEngine;

namespace Golf.GamePlay
{
	public class Hoe : IUpdatable
	{
		private const float MinProgress = 0f;
		private const float MaxProgress = 1f;

		private readonly Rigidbody m_rigidbody;
		private readonly Transform m_pointA;
		private readonly Transform m_pointB;
		private readonly float m_forwardSpeed;
		private readonly float m_returnSpeed;

		private float m_progress;
		private bool m_isPressed;

		public Hoe(
			Rigidbody rigidbody,
			Transform pointA,
			Transform pointB,
			float forwardSpeed,
			float returnSpeed)
		{
			m_rigidbody = rigidbody;
			m_pointA = pointA;
			m_pointB = pointB;
			m_forwardSpeed = forwardSpeed;
			m_returnSpeed = returnSpeed;
		}

		public void SetPressed(bool isPressed)
		{
			m_isPressed = isPressed;
		}

		public void ResetToStart()
		{
			m_isPressed = false;
			m_progress = MinProgress;
			ApplyTransform(m_progress);
		}

		public void Update()
		{
			float speed = m_isPressed ? m_forwardSpeed : m_returnSpeed;
			float target = m_isPressed ? MaxProgress : MinProgress;

			m_progress = Mathf.MoveTowards(m_progress, target, speed * Time.deltaTime);
			ApplyTransform(m_progress);
		}

		private void ApplyTransform(float progress)
		{
			Vector3 targetPosition = Vector3.Lerp(m_pointA.position, m_pointB.position, progress);
			Quaternion targetRotation = Quaternion.Slerp(m_pointA.rotation, m_pointB.rotation, progress);

			m_rigidbody.transform.SetPositionAndRotation(targetPosition, targetRotation);
			m_rigidbody.MovePosition(targetPosition);
			m_rigidbody.MoveRotation(targetRotation);
		}
	}
}