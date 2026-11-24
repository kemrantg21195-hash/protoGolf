using System;
using Golf.GamePlay;
using TMPro;
using UnityEngine.UI;

namespace Golf.UI
{
	public class AmmoSwitchView
	{
		public event Action onSwitchClicked;

		private Button m_switchButton;
		private TMP_Text m_labelText;

		public AmmoSwitchView(Button switchButton, TMP_Text labelText = null)
		{
			m_switchButton = switchButton;
			m_labelText = labelText != null
				? labelText
				: (switchButton != null ? switchButton.GetComponentInChildren<TMP_Text>() : null);
		}

		public void Show(AmmoType ammoType)
		{
			if (m_switchButton != null)
			{
				m_switchButton.gameObject.SetActive(true);
				m_switchButton.onClick.AddListener(OnSwitchButtonClick);
			}

			UpdateAmmoType(ammoType);
		}

		public void Hide()
		{
			if (m_switchButton != null)
			{
				m_switchButton.onClick.RemoveListener(OnSwitchButtonClick);
				m_switchButton.gameObject.SetActive(false);
			}
		}

		public void UpdateAmmoType(AmmoType ammoType)
		{
			if (m_labelText != null)
			{
				m_labelText.text = ammoType == AmmoType.Heavy
					? "Снаряд: Тяжёлый"
					: "Снаряд: Обычный";
			}
		}

		private void OnSwitchButtonClick()
		{
			onSwitchClicked?.Invoke();
		}
	}
}