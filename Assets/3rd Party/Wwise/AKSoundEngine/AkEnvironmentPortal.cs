// dnSpy decompiler from Assembly-CSharp.dll class: AkEnvironmentPortal
using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("Wwise/AkEnvironmentPortal")]
public class AkEnvironmentPortal : MonoBehaviour
{
	public float GetAuxSendValueForPosition(Vector3 in_position, int index)
	{
		float num = Vector3.Dot(Vector3.Scale(base.GetComponent<BoxCollider>().size, base.transform.lossyScale), this.axis);
		Vector3 vector = Vector3.Normalize(base.transform.rotation * this.axis);
		float num2 = Vector3.Dot(in_position - (base.transform.position - num * 0.5f * vector), vector);
		if (index == 0)
		{
			return (num - num2) * (num - num2) / (num * num);
		}
		return num2 * num2 / (num * num);
	}

	public const int MAX_ENVIRONMENTS_PER_PORTAL = 2;

	public Vector3 axis = new Vector3(1f, 0f, 0f);

	public AkEnvironment[] environments = new AkEnvironment[2];
}
