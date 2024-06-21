// dnSpy decompiler from Assembly-CSharp.dll class: AK.Wwise.Event
using System;
using UnityEngine;

namespace AK.Wwise
{
	[Serializable]
	public class Event : BaseType
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

		public uint Post(GameObject gameObject)
		{
			if (!this.IsValid())
			{
				return 0u;
			}
			uint num = AkSoundEngine.PostEvent(this.Id, gameObject);
			this.VerifyPlayingID(num);
			return num;
		}

		public uint Post(GameObject gameObject, CallbackFlags flags, AkCallbackManager.EventCallback callback, object cookie = null)
		{
			if (!this.IsValid())
			{
				return 0u;
			}
			uint num = AkSoundEngine.PostEvent(this.Id, gameObject, flags.value, callback, cookie);
			this.VerifyPlayingID(num);
			return num;
		}

		public uint Post(GameObject gameObject, uint flags, AkCallbackManager.EventCallback callback, object cookie = null)
		{
			if (!this.IsValid())
			{
				return 0u;
			}
			uint num = AkSoundEngine.PostEvent(this.Id, gameObject, flags, callback, cookie);
			this.VerifyPlayingID(num);
			return num;
		}

		public void Stop(GameObject gameObject, int transitionDuration = 0, AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear)
		{
			this.ExecuteAction(gameObject, AkActionOnEventType.AkActionOnEventType_Stop, transitionDuration, curveInterpolation);
		}

		public void ExecuteAction(GameObject gameObject, AkActionOnEventType actionOnEventType, int transitionDuration, AkCurveInterpolation curveInterpolation)
		{
			if (this.IsValid())
			{
				AKRESULT result = AkSoundEngine.ExecuteActionOnEvent(this.Id, actionOnEventType, gameObject, transitionDuration, curveInterpolation);
				base.Verify(result);
			}
		}

		public void PostMIDI(GameObject gameObject, AkMIDIPostArray array)
		{
			if (this.IsValid())
			{
				array.PostOnEvent(this.Id, gameObject);
			}
		}

		public void PostMIDI(GameObject gameObject, AkMIDIPostArray array, int count)
		{
			if (this.IsValid())
			{
				array.PostOnEvent(this.Id, gameObject, count);
			}
		}

		public void StopMIDI(GameObject gameObject)
		{
			if (this.IsValid())
			{
				AkSoundEngine.StopMIDIOnEvent(this.Id, gameObject);
			}
		}

		public void StopMIDI()
		{
			if (this.IsValid())
			{
				AkSoundEngine.StopMIDIOnEvent(this.Id);
			}
		}
	}
}
