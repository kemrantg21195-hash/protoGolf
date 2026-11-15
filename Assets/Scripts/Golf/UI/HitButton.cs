using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Golf.UI
{
	public class HitButton
	{
		public event Action<bool> onPressChanged;

		private readonly GameObject m_buttonObject;
		private readonly EventTrigger m_eventTrigger;

		public HitButton(GameObject buttonObject, EventTrigger eventTrigger)
		{
			m_buttonObject = buttonObject;
			m_eventTrigger = eventTrigger;
			SetupTriggers();
		}

		public void Show()
		{
			m_buttonObject.SetActive(true);
		}

		public void Hide()
		{
			m_buttonObject.SetActive(false);
			onPressChanged?.Invoke(false);
		}

		private void SetupTriggers()
		{
			m_eventTrigger.triggers.Clear();

			var pointerDownEntry = new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerDown
			};
			pointerDownEntry.callback.AddListener(OnPointerDownHandler);
			m_eventTrigger.triggers.Add(pointerDownEntry);

			var pointerUpEntry = new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerUp
			};
			pointerUpEntry.callback.AddListener(OnPointerUpHandler);
			m_eventTrigger.triggers.Add(pointerUpEntry);
		}

		private void OnPointerDownHandler(BaseEventData eventData)
		{
			onPressChanged?.Invoke(true);
		}

		private void OnPointerUpHandler(BaseEventData eventData)
		{
			onPressChanged?.Invoke(false);
		}
	}
}