using UnityEngine;

namespace Golf.GamePlay
{
	public class GameSettings
	{
		private const string CameraSensitivityKey = "CameraSensitivity";
		private const string BotDifficultyKey = "BotDifficulty";
		private const string SoundVolumeKey = "SoundVolume";

		public float cameraSensitivity => m_cameraSensitivity;
		public BotDifficulty botDifficulty => m_botDifficulty;
		public float soundVolume => m_soundVolume;

		private float m_cameraSensitivity;
		private BotDifficulty m_botDifficulty;
		private float m_soundVolume;

		public GameSettings(float defaultSensitivity, BotDifficulty defaultDifficulty)
		{
			m_cameraSensitivity = PlayerPrefs.GetFloat(CameraSensitivityKey, defaultSensitivity);
			int savedDifficulty = PlayerPrefs.GetInt(BotDifficultyKey, (int)defaultDifficulty);
			m_botDifficulty = (BotDifficulty)Mathf.Clamp(savedDifficulty, 0, 1);
			m_soundVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 1f);
		}

		public void SetCameraSensitivity(float sensitivity)
		{
			m_cameraSensitivity = Mathf.Clamp(sensitivity, 0.05f, 1.5f);
			PlayerPrefs.SetFloat(CameraSensitivityKey, m_cameraSensitivity);
			PlayerPrefs.Save();
		}

		public void SetBotDifficulty(BotDifficulty difficulty)
		{
			m_botDifficulty = difficulty;
			PlayerPrefs.SetInt(BotDifficultyKey, (int)m_botDifficulty);
			PlayerPrefs.Save();
		}

		public void SetSoundVolume(float volume)
		{
			m_soundVolume = Mathf.Clamp01(volume);
			PlayerPrefs.SetFloat(SoundVolumeKey, m_soundVolume);
			PlayerPrefs.Save();
		}
	}
}