using System;
using System.Collections.Generic;
using Golf.States.Interfaces;
using UnityEngine;

namespace Golf.GamePlay
{
	public class PlayerCannon : IUpdatable
	{
		public event Action onShot;

		public bool isCharging => m_isCharging;
		public bool isReloading => m_reloadTimer > 0f;
		public float chargeProgress => m_maxChargeTime > 0f
			? Mathf.Clamp01(m_chargeTimer / m_maxChargeTime)
			: 1f;
		public float reloadProgress => m_reloadDuration > 0f
			? 1f - Mathf.Clamp01(m_reloadTimer / m_reloadDuration)
			: 1f;
		public IReadOnlyList<Cannonball> cannonballs => m_cannonballs;

		private Transform m_cannonBase;
		private Transform m_cannonBarrel;
		private Transform m_muzzlePoint;
		private GameObject m_cannonballPrefab;
		private float m_minShootForce;
		private float m_maxShootForce;
		private float m_maxChargeTime;
		private float m_reloadDuration;
		private Collider m_seaCollider;
		private float m_startBaseYaw;
		private float m_startBasePitch;
		private float m_startBarrelYaw;
		private float m_startBarrelPitch;
		private float m_chargeTimer;
		private float m_reloadTimer;
		private bool m_isCharging;
		private List<Cannonball> m_cannonballs = new();

		public PlayerCannon(
			Transform cannonBase,
			Transform cannonBarrel,
			Transform muzzlePoint,
			GameObject cannonballPrefab,
			float minShootForce,
			float maxShootForce,
			float maxChargeTime,
			float reloadDuration,
			Collider seaCollider = null)
		{
			m_cannonBase = cannonBase;
			m_cannonBarrel = cannonBarrel;
			m_muzzlePoint = muzzlePoint;
			m_cannonballPrefab = cannonballPrefab;
			m_minShootForce = minShootForce;
			m_maxShootForce = maxShootForce;
			m_maxChargeTime = maxChargeTime;
			m_reloadDuration = reloadDuration;
			m_seaCollider = seaCollider;

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
			CancelCharge();
			m_reloadTimer = 0f;
			UpdateAim(0f, 0f);
		}

		public void StartCharging()
		{
			if (isReloading)
			{
				return;
			}

			m_isCharging = true;
			m_chargeTimer = 0f;
		}

		public void ReleaseAndShoot()
		{
			if (!m_isCharging || isReloading)
			{
				return;
			}

			float shootForce = Mathf.Lerp(m_minShootForce, m_maxShootForce, chargeProgress);
			CancelCharge();
			Shoot(shootForce);
			m_reloadTimer = m_reloadDuration;
		}

		public void CancelCharge()
		{
			m_isCharging = false;
			m_chargeTimer = 0f;
		}

		public void Update()
		{
			if (m_reloadTimer > 0f)
			{
				m_reloadTimer -= Time.deltaTime;
			}

			if (m_isCharging)
			{
				m_chargeTimer += Time.deltaTime;
			}

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

		private void Shoot(float shootForce)
		{
			GameObject ballObject = UnityEngine.Object.Instantiate(
				m_cannonballPrefab,
				m_muzzlePoint.position,
				m_muzzlePoint.rotation);

			Rigidbody rb = ballObject.GetComponentInChildren<Rigidbody>();

			var cannonball = new Cannonball(ballObject, rb, m_seaCollider);
			cannonball.Launch(m_muzzlePoint.forward * shootForce);
			m_cannonballs.Add(cannonball);
			onShot?.Invoke();
		}
	}
}