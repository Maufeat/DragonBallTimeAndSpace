// dnSpy decompiler from Assembly-CSharp.dll class: AkTriggerExit
using System;
using UnityEngine;

public class AkTriggerExit : AkTriggerBase
{
	private void OnTriggerExit(Collider in_other)
	{
		if (this.triggerDelegate != null && (this.triggerObject == null || this.triggerObject == in_other.gameObject))
		{
			this.triggerDelegate(in_other.gameObject);
		}
	}

	public GameObject triggerObject;
}
