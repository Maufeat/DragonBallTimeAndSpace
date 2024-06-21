using System;
using UnityEngine;

namespace AudioStudio
{
    [AddComponentMenu("AudioStudio/SetState")]
    public class SetState : AudioEventHandler
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
            if (AkSoundEngine.IsInitialized())
            {
                for (int i = 0; i < this.States.Length; i++)
                {
                    this.States[i].TriggerOnEnable();
                }
            }
        }

        public override void HandleDisableEvent()
        {
            if (AkSoundEngine.IsInitialized())
            {
                for (int i = 0; i < this.States.Length; i++)
                {
                    this.States[i].TriggerOnDisable();
                }
            }
        }

        public StateParam[] States;
    }
}
