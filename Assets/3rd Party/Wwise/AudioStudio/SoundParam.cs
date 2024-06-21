using System;

namespace AudioStudio
{
    [Serializable]
    public class SoundParam
    {
        public AudioEventPC audioEvent;

        public bool stop;

        public float transitionDuration = 0.2f;
    }
}
