// dnSpy decompiler from Assembly-CSharp.dll class: WwiseStateReference
using System;
using UnityEngine;

public class WwiseStateReference : WwiseGroupValueObjectReference
{
	public override WwiseObjectType WwiseObjectType
	{
		get
		{
			return WwiseStateReference.MyWwiseObjectType;
		}
	}

	public override WwiseObjectReference GroupObjectReference
	{
		get
		{
			return this.WwiseStateGroupReference;
		}
		set
		{
			this.WwiseStateGroupReference = (value as WwiseStateGroupReference);
		}
	}

	public override WwiseObjectType GroupWwiseObjectType
	{
		get
		{
			return WwiseStateReference.MyGroupWwiseObjectType;
		}
	}

	private static readonly WwiseObjectType MyWwiseObjectType = WwiseObjectType.State;

	private static readonly WwiseObjectType MyGroupWwiseObjectType = WwiseObjectType.StateGroup;

	[AkShowOnly]
	[SerializeField]
	private WwiseStateGroupReference WwiseStateGroupReference;
}
