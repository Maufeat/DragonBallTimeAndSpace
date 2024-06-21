using System;
using UnityEngine;

namespace AudioStudio
{
    [AddComponentMenu("AudioStudio/EffectSound")]
    public class EffectSound : AudioEventHandler
    {
        public void SetEffectData(CharactorBase player)
        {
            this.m_PlayerData = player;
            this.m_IsMainPlayer = (player is MainPlayer);
            this.m_IsNPC = !(player is OtherPlayer);
        }

        private bool IsSoundSwitchOn()
        {
            return AnimationSound.IsPlayerSoundSwitchOn(this.m_PlayerData, this.m_IsMainPlayer, this.m_IsNPC);
        }

        public override void HandleEnableEvent()
        {
            if (AkSoundEngine.IsInitialized() && this.IsSoundSwitchOn())
            {
                this.PostEnableEvents();
            }
        }

        public override void HandleDisableEvent()
        {
            if (AkSoundEngine.IsInitialized() && this.IsSoundSwitchOn())
            {
                this.StopEnableEvents();
                this.PostEvents(this.disables);
            }
        }

        private void PostEvents(AudioEventPC[] events)
        {
            if (AkSoundEngine.IsInitialized() && events != null)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    events[i].Post(base.Player, this.PlayerControl);
                }
            }
        }

        private void PostEnableEvents()
        {
            if (AkSoundEngine.IsInitialized())
            {
                for (int i = 0; i < this.enables.Length; i++)
                {
                    this.enables[i].audioEvent.Post(base.Player, this.PlayerControl);
                }
            }
        }

        private void StopEnableEvents()
        {
            if (AkSoundEngine.IsInitialized())
            {
                for (int i = 0; i < this.enables.Length; i++)
                {
                    if (this.enables[i].stop)
                    {
                        this.enables[i].audioEvent.Stop(base.Player, (int)this.enables[i].transitionDuration * 1000, AkCurveInterpolation.AkCurveInterpolation_Linear, false);
                    }
                }
            }
        }

        public SoundParam[] enables;

        public AudioEventPC[] disables;

        public bool PlayerControl;

        private CharactorBase m_PlayerData;

        private bool m_IsMainPlayer;

        private bool m_IsNPC;
    }
}
