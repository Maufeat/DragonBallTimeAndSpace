using System;
using UnityEngine;

namespace AudioStudio
{
    public class GlobalSoundObj : MonoBehaviour
    {
        private void Awake()
        {
            AkSoundEngine.RegisterGameObj(base.gameObject, "Global SoundObject");
            AkSoundEngine.Log("Register: Global SoundObject");
            ListenerManager.SetListener(base.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }

        private void Destroy()
        {
            if (AkSoundEngine.IsInitialized())
            {
                ListenerManager.RemoveListener(base.gameObject);
                AkSoundEngine.UnregisterGameObj(base.gameObject);
                AkSoundEngine.Log("Unregister: Global SoundObject");
            }
        }

        private void Update()
        {
            ListenerManager.Refresh();
        }
    }
}
