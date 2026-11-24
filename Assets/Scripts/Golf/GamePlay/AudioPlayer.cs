using UnityEngine;

namespace Golf.GamePlay
{
	public class AudioPlayer
	{
		private AudioSource m_seaSource;
		private AudioSource m_sfxSource;
		private AudioClip m_seaClip;
		private AudioClip m_shootClip;
		private AudioClip m_hitClip;
		private float m_hitVolumeScale;
		private float m_volume;

		public AudioPlayer(
			AudioSource seaSource,
			AudioSource sfxSource,
			AudioClip seaClip,
			AudioClip shootClip,
			AudioClip hitClip,
			float hitVolumeScale,
			float initialVolume)
		{
			m_seaSource = seaSource;
			m_sfxSource = sfxSource;
			m_seaClip = seaClip;
			m_shootClip = shootClip;
			m_hitClip = hitClip;
			m_hitVolumeScale = Mathf.Clamp01(hitVolumeScale);
			SetVolume(initialVolume);
		}

		public void SetVolume(float volume)
		{
			m_volume = Mathf.Clamp01(volume);

			if (m_seaSource != null)
			{
				m_seaSource.volume = m_volume * 0.5f;
			}

			if (m_sfxSource != null)
			{
				m_sfxSource.volume = m_volume;
			}
		}

		public void PlaySeaAmbient()
		{
			if (m_seaSource == null || m_seaClip == null)
			{
				return;
			}

			if (m_seaSource.clip != m_seaClip)
			{
				m_seaSource.clip = m_seaClip;
			}

			m_seaSource.loop = true;

			if (!m_seaSource.isPlaying)
			{
				m_seaSource.Play();
			}
		}

		public void PlayPlayerShot()
		{
			PlaySfx(m_shootClip, 1f);
		}

		public void PlayEnemyShot()
		{
			PlaySfx(m_shootClip, 0.55f);
		}

		public void PlayHit()
		{
			PlaySfx(m_hitClip, m_hitVolumeScale);
		}

		private void PlaySfx(AudioClip clip, float volumeScale)
		{
			if (m_sfxSource != null && clip != null)
			{
				m_sfxSource.PlayOneShot(clip, volumeScale);
			}
		}
	}
}