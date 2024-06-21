using System;
using AK.Wwise;
using UnityEngine;

namespace AudioStudio
{
    [Serializable]
    public class AudioEventPC : BaseType
    {
        public override WwiseObjectType WwiseObjectType
        {
            get
            {
                return WwiseObjectType.Event;
            }
        }

        private void VerifyPlayingID(uint playingId)
        {
        }

        public uint Post(bool playerCtrl = false)
        {
            if (!this.IsValid())
            {
                return 0U;
            }
            uint num = AkSoundEngine.PostEvent((!playerCtrl) ? this.Id : this.ID_PC, AudioManager.Instance.GlobalSoundObject);
            this.VerifyPlayingID(num);
            return num;
        }

        public uint Post(GameObject gameObj, bool playerCtrl = false)
        {
            if (!this.IsValid())
            {
                return 0U;
            }
            uint num = AkSoundEngine.PostEvent((!playerCtrl) ? this.Id : this.ID_PC, gameObj);
            this.VerifyPlayingID(num);
            return num;
        }

        public void Stop(int transitionDuration = 0, AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear, bool playerCtrl = false)
        {
            this.ExecuteAction(AudioManager.Instance.GlobalSoundObject, AkActionOnEventType.AkActionOnEventType_Stop, transitionDuration, curveInterpolation, playerCtrl);
        }

        public void Stop(GameObject gameObj, int transitionDuration = 0, AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear, bool playerCtrl = false)
        {
            this.ExecuteAction(gameObj, AkActionOnEventType.AkActionOnEventType_Stop, transitionDuration, curveInterpolation, playerCtrl);
        }

        public void ExecuteAction(GameObject gameObj, AkActionOnEventType actionOnEventType, int transitionDuration, AkCurveInterpolation curveInterpolation, bool playerCtrl = false)
        {
            if (this.IsValid())
            {
                AKRESULT result = AkSoundEngine.ExecuteActionOnEvent((!playerCtrl) ? this.Id : this.ID_PC, actionOnEventType, gameObj, transitionDuration, curveInterpolation);
                base.Verify(result);
            }
        }

        public uint ID_PC;

        public byte[] valueGuidPC;
    }
}
