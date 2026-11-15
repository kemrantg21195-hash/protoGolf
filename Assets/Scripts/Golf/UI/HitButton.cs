using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Golf.UI
{
	public class HitButton
	{
		public event Action<bool> onPressChanged;

		private Button m_button;
		private EventTrigger m_eventTrigger;

		public HitButton(Button button)
		{
			m_button = button;
			m_eventTrigger = m_button.GetComponent<EventTrigger>();

			if (m_eventTrigger == null)
			{
				m_eventTrigger = m_button.gameObject.AddComponent<EventTrigger>();
			}

			InitTriggers();
		}

		public void Show()
		{
			m_button.gameObject.SetActive(true);
		}

		public void Hide()
		{
			m_button.gameObject.SetActive(false);
			onPressChanged?.Invoke(false);
		}

		private void InitTriggers()
		{
			m_eventTrigger.triggers.Clear();

			var downEntry = new EventTrigger.Entry();
			downEntry.eventID = EventTriggerType.PointerDown;
			downEntry.callback.AddListener(OnPointerDownHandler);
			m_eventTrigger.triggers.Add(downEntry);

			var upEntry = new EventTrigger.Entry();
			upEntry.eventID = EventTriggerType.PointerUp;
			upEntry.callback.AddListener(OnPointerUpHandler);
			m_eventTrigger.triggers.Add(upEntry);
		}

		private void OnPointerDownHandler(BaseEventData data)
		{
			onPressChanged?.Invoke(true);
		}

		private void OnPointerUpHandler(BaseEventData data)
		{
			onPressChanged?.Invoke(false);
		}
	}
}