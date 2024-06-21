// dnSpy decompiler from Assembly-CSharp.dll class: AkTriggerEnable
using System;

public class AkTriggerEnable : AkTriggerBase
{
	private void OnEnable()
	{
		if (this.triggerDelegate != null)
		{
			this.triggerDelegate(null);
		}
	}
}
