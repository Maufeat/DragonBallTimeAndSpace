using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AudioStudio
{
    [AddComponentMenu("AudioStudio/ButtonSound")]
    public class ButtonSound : MonoBehaviour
    {
        private void Start()
        {
            Button component = base.GetComponent<Button>();
            if (component != null && component.onClick != null)
            {
                component.onClick.AddListener(new UnityAction(this.PlaySound));
                EventTrigger component2 = component.gameObject.GetComponent<EventTrigger>();
                if (component2 != null)
                {
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerEnter;
                    entry.callback = new EventTrigger.TriggerEvent();
                    entry.callback.AddListener(new UnityAction<BaseEventData>(this.OnPointEnter));
                    component2.triggers.Add(entry);
                }
            }
        }

        public void PlaySound()
        {
            if (AkSoundEngine.IsInitialized())
            {
                AudioManager.Instance.PostEvents(null, this.audioEvents);
            }
        }

        public void OnPointEnter(BaseEventData pointData)
        {
            if (AkSoundEngine.IsInitialized() && this.mouseinEvent != null)
            {
                this.mouseinEvent.Post();
            }
        }

        public AudioEvent[] audioEvents;

        public AudioEvent mouseinEvent;
    }
}
