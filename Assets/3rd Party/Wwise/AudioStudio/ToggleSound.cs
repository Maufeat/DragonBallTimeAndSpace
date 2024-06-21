using System;
using UnityEngine;
using UnityEngine.UI;

namespace AudioStudio
{
    [AddComponentMenu("AudioStudio/ToggleSound")]
    public class ToggleSound : MonoBehaviour
    {
        private void Start()
        {
            Toggle toggle = base.GetComponent<Toggle>();
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener(delegate (bool isOnChange)
                {
                    if (!toggle.isOn)
                    {
                        this.PlaySound(this.toggleaudioEvents);
                    }
                    else
                    {
                        this.PlaySound(this.untoggleaudioEvents);
                    }
                });
            }
        }

        public void PlaySound(AudioEvent[] events)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AudioManager.Instance.PostEvents(null, events);
            }
        }

        public AudioEvent[] toggleaudioEvents;

        public AudioEvent[] untoggleaudioEvents;
    }
}
