// dnSpy decompiler from Assembly-CSharp.dll class: AkState
using System;
using AK.Wwise;
using UnityEngine;

[AddComponentMenu("Wwise/AkState")]
public class AkState : AkUnityEventHandler, ISerializationCallbackReceiver
{
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
	}

	public override void HandleEvent(GameObject in_gameObject)
	{
		this.data.SetValue();
	}

	public State data = new State();

	[SerializeField]
	[HideInInspector]
	private byte[] valueGuid;

	[SerializeField]
	[HideInInspector]
	private byte[] groupGuid;
}
