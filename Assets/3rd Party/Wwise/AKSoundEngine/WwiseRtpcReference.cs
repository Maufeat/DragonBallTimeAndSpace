// dnSpy decompiler from Assembly-CSharp.dll class: WwiseRtpcReference
using System;

public class WwiseRtpcReference : WwiseObjectReference
{
	public override WwiseObjectType WwiseObjectType
	{
		get
		{
			return WwiseRtpcReference.MyWwiseObjectType;
		}
	}

	private static readonly WwiseObjectType MyWwiseObjectType = WwiseObjectType.GameParameter;
}
