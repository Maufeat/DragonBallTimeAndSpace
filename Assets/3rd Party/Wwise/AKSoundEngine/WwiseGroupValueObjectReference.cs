// dnSpy decompiler from Assembly-CSharp.dll class: WwiseGroupValueObjectReference
using System;

public abstract class WwiseGroupValueObjectReference : WwiseObjectReference
{
	public abstract WwiseObjectReference GroupObjectReference { get; set; }

	public abstract WwiseObjectType GroupWwiseObjectType { get; }

	public override string DisplayName
	{
		get
		{
			WwiseObjectReference groupObjectReference = this.GroupObjectReference;
			if (!groupObjectReference)
			{
				return base.ObjectName;
			}
			return groupObjectReference.ObjectName + " / " + base.ObjectName;
		}
	}
}
