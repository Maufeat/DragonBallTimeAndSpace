// dnSpy decompiler from Assembly-CSharp.dll class: AkTriggerMouseEnter
using System;

public class AkTriggerMouseEnter : AkTriggerBase
{
	private void OnMouseEnter()
	{
		if (this.triggerDelegate != null)
		{
			this.triggerDelegate(null);
		}
	}
}
