using System;
using UnityEngine;

namespace AudioStudio
{
    public class ListenerManager
    {
        public static bool SetListener(GameObject gameObj)
        {
            if (gameObj == null)
            {
                return false;
            }
            ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObj);
            if (akGameObjectID != ListenerManager.Listeners[0])
            {
                ListenerManager.ListenerChanged = true;
                ListenerManager.Listeners[0] = akGameObjectID;
            }
            return true;
        }

        public static void RemoveListener(GameObject gameObj)
        {
            if (gameObj != null)
            {
                ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObj);
                if (akGameObjectID != ListenerManager.Listeners[0])
                {
                    return;
                }
            }
            ListenerManager.ListenerChanged = true;
            ListenerManager.Listeners[0] = AkSoundEngine.GetAkGameObjectID(AudioManager.Instance.GlobalSoundObject);
        }

        public static void Refresh()
        {
            if (ListenerManager.ListenerChanged)
            {
                ListenerManager.ListenerChanged = false;
                AkSoundEngine.SetDefaultListeners(ListenerManager.Listeners, 1U);
            }
        }

        private static bool ListenerChanged = false;

        private static ulong[] Listeners = new ulong[]
        {
            ulong.MaxValue
        };
    }
}
