using System;
using AK.Wwise;
using UnityEngine;

namespace AudioStudio
{
    [Serializable]
    public class AudioEvent : BaseType
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

        public uint Post()
        {
            if (!this.IsValid())
            {
                return 0U;
            }
            uint num = AkSoundEngine.PostEvent(this.Id, AudioManager.Instance.GlobalSoundObject);
            this.VerifyPlayingID(num);
            return num;
        }

        public uint Post(GameObject gameObj)
        {
            if (!this.IsValid())
            {
                return 0U;
            }
            GameObject in_gameObjectID = (!(gameObj == null)) ? gameObj : AudioManager.Instance.GlobalSoundObject;
            uint num = AkSoundEngine.PostEvent(this.Id, in_gameObjectID);
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
                AKRESULT result = AkSoundEngine.ExecuteActionOnEvent(this.Id, actionOnEventType, gameObj, transitionDuration, curveInterpolation);
                base.Verify(result);
            }
        }
    }
}
