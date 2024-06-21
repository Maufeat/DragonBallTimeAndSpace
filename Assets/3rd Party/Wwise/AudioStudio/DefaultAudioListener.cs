using System;
using UnityEngine;

namespace AudioStudio
{
    [AddComponentMenu("AudioStudio/DefaultAudioListener")]
    [DisallowMultipleComponent]
    public class DefaultAudioListener : AudioGameObject
    {
        private new void Awake()
        {
            base.Awake();
            ListenerManager.SetListener(base.Player);
        }

        private new void OnDestroy()
        {
            ListenerManager.RemoveListener(base.Player);
            base.OnDestroy();
        }

        public override Vector3 GetForward()
        {
            if (DefaultAudioListener.FollowCameraForward && Camera.main != null)
            {
                return Camera.main.transform.forward;
            }
            return base.transform.forward;
        }

        public override Vector3 GetUp()
        {
            if (DefaultAudioListener.FollowCameraForward && Camera.main != null)
            {
                return Camera.main.transform.up;
            }
            return base.transform.up;
        }

        public static bool FollowCameraForward;
    }
}
