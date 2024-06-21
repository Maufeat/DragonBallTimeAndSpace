// dnSpy decompiler from Assembly-CSharp.dll class: WwiseAuxBusReference
using System;

public class WwiseAuxBusReference : WwiseObjectReference
{
	public override WwiseObjectType WwiseObjectType
	{
		get
		{
			return WwiseAuxBusReference.MyWwiseObjectType;
		}
	}

	private static readonly WwiseObjectType MyWwiseObjectType = WwiseObjectType.AuxBus;
}
