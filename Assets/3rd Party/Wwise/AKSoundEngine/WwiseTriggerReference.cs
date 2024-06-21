// dnSpy decompiler from Assembly-CSharp.dll class: WwiseTriggerReference
using System;

public class WwiseTriggerReference : WwiseObjectReference
{
	public override WwiseObjectType WwiseObjectType
	{
		get
		{
			return WwiseTriggerReference.MyWwiseObjectType;
		}
	}

	private static readonly WwiseObjectType MyWwiseObjectType = WwiseObjectType.Trigger;
}
