using System;
using UnityEngine;

namespace AudioStudio
{
    [AddComponentMenu("AudioStudio/MenuSound")]
    public class MenuSound : AudioEventHandler
    {
        private new void Awake()
        {
            this.IsUpdatePosition = false;
            this.isEnvironmentAware = false;
            this.StopOnDestroy = false;
            base.Awake(); 
        }

        public override void HandleEnableEvent()
        {
            this.Post(this.Opens);
        }

        public override void HandleDisableEvent()
        {
            this.Post(this.Closes);
        }

        private void Post(AudioEvent[] events)
        {
            AudioManager.Instance.PostEvents(base.Player, events);
        }

        public AudioEvent[] Opens;

        public AudioEvent[] Closes;
    }
}
