using System;
using System.Collections.Generic;
using UnityEngine;

namespace AudioStudio
{
    public sealed class AudioManager
    {
        private AudioManager()
        {
        }

        public static AudioManager Instance
        {
            get
            {
                return AudioManager.instance;
            }
        }

        public void Init(GameObject gameObj)
        {
            this.GlobalSoundObject = gameObj;
        }

        private void Callback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
        {
            switch (in_type)
            {
                case AkCallbackType.AK_EndOfEvent:
                    if (in_cookie != null)
                    {
                        AudioCtrl.EventCallback eventCallback = (AudioCtrl.EventCallback)in_cookie;
                        eventCallback();
                    }
                    break;
                case AkCallbackType.AK_Marker:
                    {
                        AkMarkerCallbackInfo akMarkerCallbackInfo = in_info as AkMarkerCallbackInfo;
                        AkSoundEngine.Log(akMarkerCallbackInfo.strLabel);
                        break;
                    }
            }
        }

        public uint PlaySound(string eventName, GameObject gameObj)
        {
            uint result = 0U;
            if (!string.IsNullOrEmpty(eventName))
            {
                result = AkSoundEngine.PostEvent(eventName, gameObj);
            }
            return result;
        }

        public uint PlaySound(uint eventID, GameObject gameObj)
        {
            uint result = 0U;
            if (eventID != 0U)
            {
                result = AkSoundEngine.PostEvent(eventID, gameObj);
            }
            return result;
        }

        public uint PlaySound(string eventName)
        {
            uint result = 0U;
            if (!string.IsNullOrEmpty(eventName))
            {
                result = AkSoundEngine.PostEvent(eventName, this.GlobalSoundObject);
            }
            return result;
        }

        public uint PlaySound(string eventName, AudioCtrl.EventCallback cb)
        {
            uint result = 0U;
            if (!string.IsNullOrEmpty(eventName))
            {
                result = AkSoundEngine.PostEvent(eventName, this.GlobalSoundObject, 1U, new AkCallbackManager.EventCallback(this.Callback), cb);
            }
            return result;
        }

        public AKRESULT StopSound(string eventName, GameObject gameObj, int transitionDuration = 300, AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear)
        {
            if (!string.IsNullOrEmpty(eventName))
            {
                transitionDuration = Mathf.Clamp(transitionDuration, 0, 10000);
                if (gameObj != null)
                {
                    return AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Stop, gameObj, transitionDuration, curveInterpolation);
                }
            }
            return AKRESULT.AK_Fail;
        }

        public AKRESULT StopSound(uint eventID, GameObject gameObj, int transitionDuration = 300, AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear)
        {
            transitionDuration = Mathf.Clamp(transitionDuration, 0, 10000);
            if (gameObj != null)
            {
                return AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, gameObj, transitionDuration, curveInterpolation);
            }
            return AKRESULT.AK_Fail;
        }

        public AKRESULT StopSound(string eventName, int transitionDuration = 300, AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear)
        {
            if (!string.IsNullOrEmpty(eventName))
            {
                transitionDuration = Mathf.Clamp(transitionDuration, 0, 10000);
                return AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Stop, this.GlobalSoundObject, transitionDuration, curveInterpolation);
            }
            return AKRESULT.AK_Fail;
        }

        public uint StopSound()
        {
            return AkSoundEngine.PostEvent("Stop_All_Sound", this.GlobalSoundObject);
        }

        public uint PlayVoice(string eventName)
        {
            uint result = 0U;
            if (!string.IsNullOrEmpty(eventName))
            {
                result = AkSoundEngine.PostEvent(eventName, this.GlobalSoundObject);
            }
            return result;
        }

        public AKRESULT StopVoice(string eventName, int transitionDuration = 200)
        {
            if (!string.IsNullOrEmpty(eventName))
            {
                transitionDuration = Mathf.Clamp(transitionDuration, 0, 10000);
                return AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Stop, this.GlobalSoundObject, transitionDuration, AkCurveInterpolation.AkCurveInterpolation_Linear);
            }
            return AKRESULT.AK_Fail;
        }

        public uint StopVoice()
        {
            return AkSoundEngine.PostEvent("Stop_Voice_Dialogue", this.GlobalSoundObject);
        }

        public AKRESULT SetState(string stateGroup, string state)
        {
            return AkSoundEngine.SetState(stateGroup, state);
        }

        public AKRESULT SetState(int stateGroupID, int stateValueID)
        {
            return AkSoundEngine.SetState((uint)stateGroupID, (uint)stateValueID);
        }

        public void LoadBank(string bankName)
        {
            if (!string.IsNullOrEmpty(bankName))
            {
                AkBankManager.LoadBank(bankName);
            }
        }

        public void UnloadBank(string bankName)
        {
            if (!string.IsNullOrEmpty(bankName))
            {
                AkBankManager.UnloadBank(bankName);
            }
        }

        public void AddDefaultAudioListener(GameObject gameObj)
        {
            if (gameObj != null)
            {
                gameObj.AddComponent<DefaultAudioListener>();
            }
        }

        public void SetSoundVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0f, 100f);
            AkSoundEngine.SetRTPCValue(AudioManager.VolumeParam.SoundVolume, volume);
        }

        public void SetVoiceVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0f, 100f);
            AkSoundEngine.SetRTPCValue(AudioManager.VolumeParam.VoiceVolume, volume);
        }

        public void SetMusicVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0f, 100f);
            AkSoundEngine.SetRTPCValue(AudioManager.VolumeParam.MusicVolume, volume);
        }

        public void StartVoicePlay()
        {
            AkSoundEngine.PostEvent(AudioManager.VoicePlayStart, this.GlobalSoundObject);
        }

        public void EndVoicePlayEnd()
        {
            AkSoundEngine.PostEvent(AudioManager.VoicePlayEnd, this.GlobalSoundObject);
        }

        public void StartVoiceRecord()
        {
            AkSoundEngine.PostEvent(AudioManager.VoiceRecordStart, this.GlobalSoundObject);
        }

        public void EndVoiceRecord()
        {
            AkSoundEngine.PostEvent(AudioManager.VoiceRecordEnd, this.GlobalSoundObject);
        }

        public void CustomMusicStart()
        {
            AkSoundEngine.MuteBackgroundMusic(true);
        }

        public void CustomMusicEnd()
        {
            AkSoundEngine.MuteBackgroundMusic(false);
        }

        public void AudioPause()
        {
            AkSoundEngine.PostEvent("Stop_Sound", this.GlobalSoundObject);
            AkSoundEngine.Suspend();
        }

        public void AudioResume()
        {
            AkSoundEngine.WakeupFromSuspend();
        }

        public void PostEvents(GameObject obj, AudioEvent[] events)
        {
            if (AkSoundEngine.IsInitialized() && events != null)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    events[i].Post(obj);
                }
            }
        }

        public string GetEventNameWithSuffix(string eventName)
        {
            string text;
            if (!this.EventSuffixCache.TryGetValue(eventName, out text))
            {
                text = eventName + AudioManager.PlayerCtrlSuffix;
                this.EventSuffixCache[eventName] = text;
            }
            return text;
        }

        private const float minVolume = 0f;

        private const float maxVolume = 100f;

        private static readonly AudioManager instance = new AudioManager();

        public GameObject GlobalSoundObject;

        private Dictionary<string, string> EventSuffixCache = new Dictionary<string, string>();

        public static string PlayerCtrlSuffix = "_PC";

        public static string SoundOn = "Sound_On";

        public static string SoundOff = "Sound_Off";

        public static string VoiceOn = "Voice_On";

        public static string VoiceOff = "Voice_Off";

        public static string MusicOn = "Music_On";

        public static string MusicOff = "Music_Off";

        private static string VoicePlayStart = "Voice_Play_Start";

        private static string VoicePlayEnd = "Voice_Play_End";

        private static string VoiceRecordStart = "Voice_Record_Start";

        private static string VoiceRecordEnd = "Voice_Record_End";

        public struct VolumeParam
        {
            public static string SoundVolume = "Sound_Volume";

            public static string VoiceVolume = "Voice_Volume";

            public static string MusicVolume = "Music_Volume";
        }
    }
}
