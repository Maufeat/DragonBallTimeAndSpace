// dnSpy decompiler from Assembly-CSharp.dll class: AK.Wwise.Switch
using System;
using UnityEngine;

namespace AK.Wwise
{
	[Serializable]
	public class Switch : BaseGroupType
	{
		public override WwiseObjectType WwiseObjectType
		{
			get
			{
				return WwiseObjectType.Switch;
			}
		}

		public override WwiseObjectType WwiseObjectGroupType
		{
			get
			{
				return WwiseObjectType.SwitchGroup;
			}
		}

		public void SetValue(GameObject gameObject)
		{
			if (this.IsValid())
			{
				AKRESULT result = AkSoundEngine.SetSwitch(this.GroupId, this.Id, gameObject);
				base.Verify(result);
			}
		}
	}
}
