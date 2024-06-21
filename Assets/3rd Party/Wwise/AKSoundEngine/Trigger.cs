// dnSpy decompiler from Assembly-CSharp.dll class: AK.Wwise.Trigger
using System;
using UnityEngine;

namespace AK.Wwise
{
	[Serializable]
	public class Trigger : BaseType
	{
		public override WwiseObjectType WwiseObjectType
		{
			get
			{
				return WwiseObjectType.Trigger;
			}
		}

		public void Post(GameObject gameObject)
		{
			if (this.IsValid())
			{
				AKRESULT result = AkSoundEngine.PostTrigger(this.Id, gameObject);
				base.Verify(result);
			}
		}
	}
}
