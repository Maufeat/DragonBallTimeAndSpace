// dnSpy decompiler from Assembly-CSharp.dll class: AkTriggerEnter
using System;
using UnityEngine;

public class AkTriggerEnter : AkTriggerBase
{
	private void OnTriggerEnter(Collider in_other)
	{
		if (this.triggerDelegate != null && (this.triggerObject == null || this.triggerObject == in_other.gameObject))
		{
			this.triggerDelegate(in_other.gameObject);
		}
	}

	public GameObject triggerObject;
}
