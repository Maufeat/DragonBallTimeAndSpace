// dnSpy decompiler from Assembly-CSharp.dll class: AkTriggerCollisionExit
using System;
using UnityEngine;

public class AkTriggerCollisionExit : AkTriggerBase
{
	private void OnCollisionExit(Collision in_other)
	{
		if (this.triggerDelegate != null && (this.triggerObject == null || this.triggerObject == in_other.gameObject))
		{
			this.triggerDelegate(in_other.gameObject);
		}
	}

	public GameObject triggerObject;
}
