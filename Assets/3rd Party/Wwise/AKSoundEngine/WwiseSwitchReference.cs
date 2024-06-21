// dnSpy decompiler from Assembly-CSharp.dll class: WwiseSwitchReference
using System;
using UnityEngine;

public class WwiseSwitchReference : WwiseGroupValueObjectReference
{
	public override WwiseObjectType WwiseObjectType
	{
		get
		{
			return WwiseSwitchReference.MyWwiseObjectType;
		}
	}

	public override WwiseObjectReference GroupObjectReference
	{
		get
		{
			return this.WwiseSwitchGroupReference;
		}
		set
		{
			this.WwiseSwitchGroupReference = (value as WwiseSwitchGroupReference);
		}
	}

	public override WwiseObjectType GroupWwiseObjectType
	{
		get
		{
			return WwiseSwitchReference.MyGroupWwiseObjectType;
		}
	}

	private static readonly WwiseObjectType MyWwiseObjectType = WwiseObjectType.Switch;

	private static readonly WwiseObjectType MyGroupWwiseObjectType = WwiseObjectType.SwitchGroup;

	[AkShowOnly]
	[SerializeField]
	private WwiseSwitchGroupReference WwiseSwitchGroupReference;
}
