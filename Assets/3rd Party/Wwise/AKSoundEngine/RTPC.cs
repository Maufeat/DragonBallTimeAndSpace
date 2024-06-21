// dnSpy decompiler from Assembly-CSharp.dll class: AK.Wwise.RTPC
using System;
using UnityEngine;

namespace AK.Wwise
{
	[Serializable]
	public class RTPC : BaseType
	{
		public override WwiseObjectType WwiseObjectType
		{
			get
			{
				return WwiseObjectType.GameParameter;
			}
		}

		public void SetValue(GameObject gameObject, float value)
		{
			if (this.IsValid())
			{
				AKRESULT result = AkSoundEngine.SetRTPCValue(this.Id, value, gameObject);
				base.Verify(result);
			}
		}

		public float GetValue(GameObject gameObject)
		{
			float result = 0f;
			if (this.IsValid())
			{
				AkQueryRTPCValue akQueryRTPCValue = (!gameObject) ? AkQueryRTPCValue.RTPCValue_Global : AkQueryRTPCValue.RTPCValue_GameObject;
				int num = (int)akQueryRTPCValue;
				AKRESULT rtpcvalue = AkSoundEngine.GetRTPCValue(this.Id, gameObject, 0u, out result, ref num);
				base.Verify(rtpcvalue);
			}
			return result;
		}

		public void SetGlobalValue(float value)
		{
			if (this.IsValid())
			{
				AKRESULT result = AkSoundEngine.SetRTPCValue(this.Id, value);
				base.Verify(result);
			}
		}

		public float GetGlobalValue()
		{
			return this.GetValue(null);
		}
	}
}
