using System;

namespace AudioStudio
{
    public abstract class AudioEventHandler : AudioGameObject
    {
        private void Start()
        {
            this.HandleEnableEvent();
            this.started = true;
            this.disabled = false;
        }

        private void OnEnable()
        {
            if (this.started)
            {
                this.HandleEnableEvent();
                this.disabled = false;
            }
        }

        private void OnDisable()
        {
            if (this.started && !this.disabled)
            {
                this.HandleDisableEvent();
                this.disabled = true;
            }
        }

        private new void OnDestroy()
        {
            if (this.started && !this.disabled)
            {
                this.HandleDisableEvent();
                this.disabled = true;
            }
            base.OnDestroy();
        }

        public abstract void HandleEnableEvent();

        public abstract void HandleDisableEvent();

        private bool started;

        private bool disabled;
    }
}
