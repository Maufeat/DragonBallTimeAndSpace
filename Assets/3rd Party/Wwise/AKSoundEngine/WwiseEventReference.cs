// dnSpy decompiler from Assembly-CSharp.dll class: WwiseEventReference
using System;

public class WwiseEventReference : WwiseObjectReference
{
	public override WwiseObjectType WwiseObjectType
	{
		get
		{
			return WwiseEventReference.MyWwiseObjectType;
		}
	}

	private static readonly WwiseObjectType MyWwiseObjectType = WwiseObjectType.Event;
}
