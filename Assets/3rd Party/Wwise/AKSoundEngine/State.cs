// dnSpy decompiler from Assembly-CSharp.dll class: AK.Wwise.State
using System;

namespace AK.Wwise
{
	[Serializable]
	public class State : BaseGroupType
	{
		public override WwiseObjectType WwiseObjectType
		{
			get
			{
				return WwiseObjectType.State;
			}
		}

		public override WwiseObjectType WwiseObjectGroupType
		{
			get
			{
				return WwiseObjectType.StateGroup;
			}
		}

		public void SetValue()
		{
			if (this.IsValid())
			{
				AKRESULT result = AkSoundEngine.SetState(this.GroupId, this.Id);
				base.Verify(result);
			}
		}
	}
}
