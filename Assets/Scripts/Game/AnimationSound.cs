using System;
using Framework.Managers;
using UnityEngine;

namespace AudioStudio
{
    [DisallowMultipleComponent]
    [AddComponentMenu("AudioStudio/AnimationSound")]
    public class AnimationSound : AudioGameObject
    {
        private new void Awake()
        {
            this.LoadBanks();
            base.Awake();
        }

        private new void OnDestroy()
        {
            base.OnDestroy();
            this.UnloadBanks();
        }

        public void SetEffectData(CharactorBase player)
        {
            this.m_PlayerData = player;
            this.m_IsMainPlayer = (player is MainPlayer);
            this.m_IsNPC = !(player is OtherPlayer);
        }

        public static bool IsPlayerSoundSwitchOn(CharactorBase player, bool pc, bool npc)
        {
            if (player != null)
            {
                SystemSettingData_Sound soundData = ControllerManager.Instance.GetController<SystemSettingController>().soundData;
                if (pc)
                {
                    if (!soundData.IsMain)
                    {
                        return false;
                    }
                }
                else if (npc)
                {
                    if (!soundData.IsNPC)
                    {
                        return false;
                    }
                }
                else if (ControllerManager.Instance.GetController<TeamController>().IsMyTeamNember(player.EID))
                {
                    if (!soundData.IsTeam)
                    {
                        return false;
                    }
                }
                else if (!soundData.IsOther)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsSoundSwitchOn()
        {
            return AnimationSound.IsPlayerSoundSwitchOn(this.m_PlayerData, this.m_IsMainPlayer, this.m_IsNPC);
        }

        public void PlaySound(string eventName)
        {
            if (AkSoundEngine.IsInitialized())
            {
                this.isSwitchOn = this.IsSoundSwitchOn();
                if (this.isSwitchOn)
                {
                    if (this.PlayerControl)
                    {
                        string eventNameWithSuffix = AudioManager.Instance.GetEventNameWithSuffix(eventName);
                        AudioManager.Instance.PlaySound(eventNameWithSuffix, base.Player);
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(eventName, base.Player);
                    }
                }
            }
        }

        public void StopSound(string eventName)
        {
            if (AkSoundEngine.IsInitialized())
            {
                this.isSwitchOn = this.IsSoundSwitchOn();
                if (this.isSwitchOn)
                {
                    if (this.PlayerControl)
                    {
                        string eventNameWithSuffix = AudioManager.Instance.GetEventNameWithSuffix(eventName);
                        AudioManager.Instance.StopSound(eventNameWithSuffix, base.Player, 300, AkCurveInterpolation.AkCurveInterpolation_Linear);
                    }
                    else
                    {
                        AudioManager.Instance.StopSound(eventName, base.Player, 300, AkCurveInterpolation.AkCurveInterpolation_Linear);
                    }
                }
            }
        }

        public void Play(AudioEventPC events)
        {
            if (AkSoundEngine.IsInitialized())
            {
                events.Post(base.Player, this.PlayerControl);
            }
        }

        public void Stop(AudioEventPC events)
        {
            if (AkSoundEngine.IsInitialized())
            {
                events.Stop(base.Player, 0, AkCurveInterpolation.AkCurveInterpolation_Linear, this.PlayerControl);
            }
        }

        public void PostStep(string eventName, string defaultGroupName = "Footsteps")
        {
            AkSoundEngine.SetSwitch(defaultGroupName, eventName, base.Player);
            AudioManager.Instance.PlaySound(defaultGroupName, base.Player);
        }

        private void LoadBanks()
        {
            if (this.Banks == null)
            {
                return;
            }
            for (int i = 0; i < this.Banks.Length; i++)
            {
                this.Banks[i].Load();
            }
        }

        private void UnloadBanks()
        {
            if (this.Banks == null)
            {
                return;
            }
            for (int i = 0; i < this.Banks.Length; i++)
            {
                this.Banks[i].Unload();
            }
        }

        public Bank[] Banks;

        public bool PlayerControl;

        private CharactorBase m_PlayerData;

        private bool m_IsMainPlayer;

        private bool m_IsNPC;

        private bool isSwitchOn;
    }
}
