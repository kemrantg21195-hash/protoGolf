using System.Collections.Generic;
using Golf.States.Interfaces;
using UnityEngine;

namespace Golf.GamePlay
{
	public class PlayerCannon : IUpdatable
	{
		private Transform m_cannonBase;
		private Transform m_cannonBarrel;
		private Transform m_muzzlePoint;
		private GameObject m_cannonballPrefab;
		private float m_shootForce;
		private float m_startBaseYaw;
		private float m_startBasePitch;
		private float m_startBarrelYaw;
		private float m_startBarrelPitch;
		private List<Cannonball> m_cannonballs = new();

		public PlayerCannon(
			Transform cannonBase,
			Transform cannonBarrel,
			Transform muzzlePoint,
			GameObject cannonballPrefab,
			float shootForce)
		{
			m_cannonBase = cannonBase;
			m_cannonBarrel = cannonBarrel;
			m_muzzlePoint = muzzlePoint;
			m_cannonballPrefab = cannonballPrefab;
			m_shootForce = shootForce;

			Vector3 baseAngles = m_cannonBase.localEulerAngles;
			m_startBaseYaw = baseAngles.y;
			m_startBasePitch = baseAngles.x > 180f ? baseAngles.x - 360f : baseAngles.x;

			Vector3 barrelAngles = m_cannonBarrel.localEulerAngles;
			m_startBarrelYaw = barrelAngles.y;
			m_startBarrelPitch = barrelAngles.x > 180f ? barrelAngles.x - 360f : barrelAngles.x;
		}

		public void UpdateAim(float yawOffset, float pitchOffset)
		{
			if (m_cannonBase == m_cannonBarrel)
			{
				m_cannonBase.localEulerAngles = new Vector3(
					m_startBarrelPitch + pitchOffset,
					m_startBaseYaw + yawOffset,
					0f);
				return;
			}

			m_cannonBase.localEulerAngles = new Vector3(
				m_startBasePitch,
				m_startBaseYaw + yawOffset,
				0f);

			m_cannonBarrel.localEulerAngles = new Vector3(
				m_startBarrelPitch + pitchOffset,
				m_startBarrelYaw,
				0f);
		}

		public void ResetToStart()
		{
			UpdateAim(0f, 0f);
		}

		public void Shoot()
		{
			GameObject ballObject = Object.Instantiate(
				m_cannonballPrefab,
				m_muzzlePoint.position,
				m_muzzlePoint.rotation);

			Rigidbody rb = ballObject.GetComponentInChildren<Rigidbody>();

			var cannonball = new Cannonball(ballObject, rb);
			cannonball.Launch(m_muzzlePoint.forward * m_shootForce);
			m_cannonballs.Add(cannonball);
		}

		public void Update()
		{
			for (int i = m_cannonballs.Count - 1; i >= 0; i--)
			{
				Cannonball ball = m_cannonballs[i];
				ball.UpdateLifeTime(Time.deltaTime);

				if (ball.isExpired)
				{
					ball.Destroy();
					m_cannonballs.RemoveAt(i);
				}
			}
		}

		public void ClearCannonballs()
		{
			foreach (Cannonball ball in m_cannonballs)
			{
				ball.Destroy();
			}

			m_cannonballs.Clear();
		}
	}
}