using System;

namespace AudioStudio
{
    [Serializable]
    public class StateParam
    {
        public void TriggerOnEnable()
        {
            if (this.trigger == TriggerState.Enable)
            {
                this.state.SetValue();
            }
        }

        public void TriggerOnDisable()
        {
            if (this.trigger == TriggerState.Disable)
            {
                this.state.SetValue();
            }
        }

        public TriggerState trigger;

        public State state;
    }
}
