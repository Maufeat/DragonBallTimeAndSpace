// dnSpy decompiler from Assembly-CSharp.dll class: WwiseSwitchGroupReference
using System;

public class WwiseSwitchGroupReference : WwiseObjectReference
{
	public override WwiseObjectType WwiseObjectType
	{
		get
		{
			return WwiseSwitchGroupReference.MyWwiseObjectType;
		}
	}

	private static readonly WwiseObjectType MyWwiseObjectType = WwiseObjectType.SwitchGroup;
}
