// dnSpy decompiler from Assembly-CSharp.dll class: AkTriggerMouseDown
using System;

public class AkTriggerMouseDown : AkTriggerBase
{
	private void OnMouseDown()
	{
		if (this.triggerDelegate != null)
		{
			this.triggerDelegate(null);
		}
	}
}
