using Golf.States.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Golf.GamePlay
{
	public class FirstPersonCamera : IUpdatable
	{
		public float yawOffset => m_yawOffset;
		public float pitchOffset => m_pitchOffset;

		private Transform m_cameraTransform;
		private float m_sensitivity;
		private float m_maxHorizontalAngle;
		private float m_minVerticalAngle;
		private float m_maxVerticalAngle;
		private float m_startYaw;
		private float m_startPitch;
		private float m_yawOffset;
		private float m_pitchOffset;
		private Vector3 m_lastPointerPosition;
		private bool m_isDragging;

		public FirstPersonCamera(
			Transform cameraTransform,
			float sensitivity,
			float maxHorizontalAngle,
			float minVerticalAngle,
			float maxVerticalAngle)
		{
			m_cameraTransform = cameraTransform;
			m_sensitivity = sensitivity;
			m_maxHorizontalAngle = maxHorizontalAngle;
			m_minVerticalAngle = minVerticalAngle;
			m_maxVerticalAngle = maxVerticalAngle;

			Vector3 startAngles = m_cameraTransform.localEulerAngles;
			m_startYaw = startAngles.y;
			m_startPitch = startAngles.x > 180f ? startAngles.x - 360f : startAngles.x;
		}

		public void StopDragging()
		{
			m_isDragging = false;
		}

		public void ResetToStart()
		{
			m_isDragging = false;
			m_yawOffset = 0f;
			m_pitchOffset = 0f;
			m_cameraTransform.localEulerAngles = new Vector3(m_startPitch, m_startYaw, 0f);
		}

		public void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (!IsPointerOverUI())
				{
					m_isDragging = true;
					m_lastPointerPosition = Input.mousePosition;
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				m_isDragging = false;
			}

			if (m_isDragging && Input.GetMouseButton(0))
			{
				Vector3 delta = Input.mousePosition - m_lastPointerPosition;
				m_lastPointerPosition = Input.mousePosition;

				m_yawOffset += delta.x * m_sensitivity;
				m_pitchOffset -= delta.y * m_sensitivity;

				m_yawOffset = Mathf.Clamp(m_yawOffset, -m_maxHorizontalAngle, m_maxHorizontalAngle);
				m_pitchOffset = Mathf.Clamp(m_pitchOffset, m_minVerticalAngle, m_maxVerticalAngle);

				m_cameraTransform.localEulerAngles = new Vector3(
					m_startPitch + m_pitchOffset,
					m_startYaw + m_yawOffset,
					0f);
			}
		}

		private bool IsPointerOverUI()
		{
			if (EventSystem.current == null)
			{
				return false;
			}

			if (Input.touchCount > 0)
			{
				return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
			}

			return EventSystem.current.IsPointerOverGameObject();
		}
	}
}