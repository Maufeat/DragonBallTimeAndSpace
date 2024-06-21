// dnSpy decompiler from Assembly-CSharp.dll class: AkTriggerCollisionEnter
using System;
using UnityEngine;

public class AkTriggerCollisionEnter : AkTriggerBase
{
	private void OnCollisionEnter(Collision in_other)
	{
		if (this.triggerDelegate != null && (this.triggerObject == null || this.triggerObject == in_other.gameObject))
		{
			this.triggerDelegate(in_other.gameObject);
		}
	}

	private void OnTriggerEnter(Collider in_other)
	{
		if (this.triggerDelegate != null && (this.triggerObject == null || this.triggerObject == in_other.gameObject))
		{
			this.triggerDelegate(in_other.gameObject);
		}
	}

	public GameObject triggerObject;
}
