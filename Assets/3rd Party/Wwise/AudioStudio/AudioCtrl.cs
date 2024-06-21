using System;
using UnityEngine;

namespace AudioStudio
{
    public class AudioCtrl
    {
        public static void SoundCtrl(bool bSwitch)
        {
            AudioManager.Instance.PlaySound((!bSwitch) ? AudioManager.SoundOff : AudioManager.SoundOn);
        }

        public static void VoiceCtrl(bool bSwitch)
        {
            AudioManager.Instance.PlaySound((!bSwitch) ? AudioManager.VoiceOff : AudioManager.VoiceOn);
        }

        public static void MusicCtrl(bool bSwitch)
        {
            AudioManager.Instance.PlaySound((!bSwitch) ? AudioManager.MusicOff : AudioManager.MusicOn);
        }

        public static void SetSoundVolume(float volume)
        {
            AudioManager.Instance.SetSoundVolume(volume);
        }

        public static void SetVoiceVolume(float volume)
        {
            AudioManager.Instance.SetVoiceVolume(volume);
        }

        public static void SetMusicVolume(float volume)
        {
            AudioManager.Instance.SetMusicVolume(volume);
        }

        public static uint PlaySound(string eventName, GameObject gameObj)
        {
            return AudioManager.Instance.PlaySound(eventName, gameObj);
        }

        public static uint PlaySound(string eventName)
        {
            return AudioManager.Instance.PlaySound(eventName);
        }

        public static uint PlaySound(string eventName, AudioCtrl.EventCallback cb)
        {
            return AudioManager.Instance.PlaySound(eventName, cb);
        }

        public static AKRESULT StopSound(string eventName, GameObject gameObj)
        {
            return AudioManager.Instance.StopSound(eventName, gameObj, 300, AkCurveInterpolation.AkCurveInterpolation_Linear);
        }

        public static AKRESULT StopSound(string eventName)
        {
            return AudioManager.Instance.StopSound(eventName, 300, AkCurveInterpolation.AkCurveInterpolation_Linear);
        }

        public static void SetState(string stateGroup, string state)
        {
            AkSoundEngine.SetState(stateGroup, state);
        }

        public static void SetSwitch(string switchGroup, string switchValue, GameObject gameObj)
        {
            AkSoundEngine.SetSwitch(switchGroup, switchValue, gameObj);
        }

        public static void SetRTPCValue(string RTPC, float parameter, GameObject gameObj)
        {
            AkSoundEngine.SetRTPCValue(RTPC, parameter, gameObj);
        }

        public static void SetRTPCValue(string RTPC, float parameter)
        {
            AkSoundEngine.SetRTPCValue(RTPC, parameter);
        }

        public static void SetDefaultAudioListener(GameObject gameObj)
        {
            AudioManager.Instance.AddDefaultAudioListener(gameObj);
        }

        public static void Init()
        {
            if (!AudioCtrl.inited)
            {
                AudioCtrl.inited = true;
                GameObject gameObject = new GameObject("AudioManager");
                AkTerminator akTerminator = gameObject.AddComponent<AkTerminator>();
                AkInitializer akInitializer = gameObject.AddComponent<AkInitializer>();
                akInitializer.InitializationSettings.CallbackManagerInitializationSettings.IsLoggingEnabled = true;
                GlobalSoundObj globalSoundObj = gameObject.AddComponent<GlobalSoundObj>();
                AudioManager.Instance.Init(gameObject);
                AudioManager.Instance.LoadBank("UI");
                AudioManager.Instance.LoadBank("Music");
                AudioManager.Instance.LoadBank("SystemSetting");
                AudioManager.Instance.LoadBank("Footsteps");
                AudioManager.Instance.LoadBank("Footsteps_e_1p");
                AudioManager.Instance.LoadBank("ui_stop");
                AudioManager.Instance.LoadBank("ui_button_e");
                AudioManager.Instance.LoadBank("ui_button");
                AudioManager.Instance.LoadBank("ui_fx_e");
                AudioManager.Instance.LoadBank("ui_fx");
                AudioManager.Instance.LoadBank("ui_view_e");
                AudioManager.Instance.LoadBank("ui_view");
                AudioManager.Instance.LoadBank("Avatar_EM");
                AudioManager.Instance.LoadBank("Avatar_EM_e");
                AudioManager.Instance.LoadBank("Avatar_MM");
                AudioManager.Instance.LoadBank("Avatar_MM_e");
                AudioManager.Instance.LoadBank("Avatar_NM");
                AudioManager.Instance.LoadBank("Avatar_NM_e");
                AudioManager.Instance.LoadBank("Avatar_SM");
                AudioManager.Instance.LoadBank("Avatar_SM_e");
                AudioManager.Instance.LoadBank("Avatar_JiaoZi");
                AudioManager.Instance.LoadBank("Avatar_JiaoZi_e");
                AudioManager.Instance.LoadBank("Avatar_SunWuKong");
                AudioManager.Instance.LoadBank("Avatar_SunWuKong_e");
                AudioManager.Instance.LoadBank("Avatar_TianJinFan");
                AudioManager.Instance.LoadBank("Avatar_TianJinFan_e");
                AudioManager.Instance.LoadBank("Hero_PIC");
                AudioManager.Instance.LoadBank("Hero_PIC_e");
                AudioManager.Instance.LoadBank("Hero_BLM");
                AudioManager.Instance.LoadBank("Hero_BLM_e");
                AudioManager.Instance.LoadBank("Hero_MKLL");
                AudioManager.Instance.LoadBank("Hero_MKLL_e");
                AudioManager.Instance.LoadBank("Hero_MR");
                AudioManager.Instance.LoadBank("Hero_MR_e");
                AudioManager.Instance.LoadBank("Hero_YMC");
                AudioManager.Instance.LoadBank("Hero_YMC_e");
                AudioManager.Instance.LoadBank("Hero_GOD");
                AudioManager.Instance.LoadBank("Hero_GOD_e");
                AudioManager.Instance.LoadBank("Hero_TRF");
                AudioManager.Instance.LoadBank("Hero_TRF_e");
                AudioManager.Instance.LoadBank("Avatar_DQR");
                AudioManager.Instance.LoadBank("Avatar_DQR_e");
                AudioManager.Instance.LoadBank("Avatar_SYR");
                AudioManager.Instance.LoadBank("Avatar_SYR_e");
                AudioManager.Instance.LoadBank("Hero_SWF");
                AudioManager.Instance.LoadBank("Hero_SWT");
                AudioManager.Instance.PlaySound("Music_Play");
            }
            else
            {
                Debug.LogError("Wwise shouldn't be initialized multiple times! Please Check the [ AudioCtrl.Init()]");
            }
        }

        public static bool inited;

        public delegate void EventCallback();
    }
}
