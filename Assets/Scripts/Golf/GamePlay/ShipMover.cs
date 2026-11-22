using System;
using Golf.States.Interfaces;
using UnityEngine;

namespace Golf.GamePlay
{
	public class ShipMover : IFixedUpdatable
	{
		public event Action onPortReached;

		public bool isPortReached => m_isPortReached;

		private Rigidbody m_shipRigidbody;
		private Transform m_shipTransform;
		private Transform[] m_waypoints;
		private float m_moveSpeed;
		private float m_turnSpeed;
		private Vector3 m_startPosition;
		private Quaternion m_startRotation;
		private Quaternion m_courseRotation;
		private int m_currentWaypointIndex;
		private float m_rockingTimer;
		private bool m_isPortReached;

		public ShipMover(
			Rigidbody shipRigidbody,
			Transform[] waypoints,
			float moveSpeed,
			float turnSpeed)
		{
			m_shipRigidbody = shipRigidbody;
			m_shipTransform = shipRigidbody.transform;
			m_waypoints = waypoints;
			m_moveSpeed = moveSpeed;
			m_turnSpeed = turnSpeed;

			m_startPosition = m_shipTransform.position;
			m_startRotation = m_shipTransform.rotation;
			m_courseRotation = m_shipTransform.rotation;
		}

		public void ResetToStart()
		{
			m_currentWaypointIndex = 0;
			m_rockingTimer = 0f;
			m_isPortReached = false;
			m_courseRotation = m_startRotation;

			m_shipTransform.SetPositionAndRotation(m_startPosition, m_startRotation);
			m_shipRigidbody.position = m_startPosition;
			m_shipRigidbody.rotation = m_startRotation;
		}

		public void FixedUpdate()
		{
			if (m_waypoints == null || m_waypoints.Length == 0)
			{
				return;
			}

			Vector3 nextPosition = m_shipRigidbody.position;

			if (!m_isPortReached)
			{
				nextPosition = CalculateRoutePosition(nextPosition);
			}

			m_rockingTimer += Time.fixedDeltaTime * 0.8f;
			nextPosition.y = m_startPosition.y + Mathf.Sin(m_rockingTimer) * 0.12f;

			float pitch = Mathf.Cos(m_rockingTimer * 0.9f) * 1.5f;
			float roll = Mathf.Sin(m_rockingTimer * 1.1f) * 2.5f;
			Quaternion nextRotation = m_courseRotation * Quaternion.Euler(pitch, 0f, roll);

			m_shipRigidbody.MovePosition(nextPosition);
			m_shipRigidbody.MoveRotation(nextRotation);
		}

		private Vector3 CalculateRoutePosition(Vector3 currentPosition)
		{
			Transform targetPoint = m_waypoints[m_currentWaypointIndex];
			Vector3 flatCurrent = new Vector3(currentPosition.x, m_startPosition.y, currentPosition.z);
			Vector3 targetPosition = new Vector3(
				targetPoint.position.x,
				m_startPosition.y,
				targetPoint.position.z);

			Vector3 nextPosition = Vector3.MoveTowards(
				flatCurrent,
				targetPosition,
				m_moveSpeed * Time.fixedDeltaTime);

			Vector3 direction = targetPosition - flatCurrent;

			if (direction.sqrMagnitude > 0.01f)
			{
				Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
				m_courseRotation = Quaternion.Slerp(
					m_courseRotation,
					targetRotation,
					m_turnSpeed * Time.fixedDeltaTime);
			}

			if (Vector3.Distance(flatCurrent, targetPosition) < 0.5f)
			{
				m_currentWaypointIndex++;

				if (m_currentWaypointIndex >= m_waypoints.Length)
				{
					m_isPortReached = true;
					onPortReached?.Invoke();
				}
			}

			return nextPosition;
		}
	}
}