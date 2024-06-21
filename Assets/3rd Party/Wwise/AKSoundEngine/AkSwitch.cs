// dnSpy decompiler from Assembly-CSharp.dll class: AkSwitch
using System;
using AK.Wwise;
using UnityEngine;

[AddComponentMenu("Wwise/AkSwitch")]
public class AkSwitch : AkUnityEventHandler, ISerializationCallbackReceiver
{
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
	}

	public override void HandleEvent(GameObject in_gameObject)
	{
		this.data.SetValue((!this.useOtherObject || !(in_gameObject != null)) ? base.gameObject : in_gameObject);
	}

	public Switch data = new Switch();

	[HideInInspector]
	[SerializeField]
	private byte[] valueGuid;

	[SerializeField]
	[HideInInspector]
	private byte[] groupGuid;
}
