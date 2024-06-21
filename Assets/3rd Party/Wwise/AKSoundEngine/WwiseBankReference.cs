// dnSpy decompiler from Assembly-CSharp.dll class: WwiseBankReference
using System;

public class WwiseBankReference : WwiseObjectReference
{
	public override WwiseObjectType WwiseObjectType
	{
		get
		{
			return WwiseBankReference.MyWwiseObjectType;
		}
	}

	private static readonly WwiseObjectType MyWwiseObjectType = WwiseObjectType.Soundbank;
}
