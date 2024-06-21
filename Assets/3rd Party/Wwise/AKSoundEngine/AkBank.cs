// dnSpy decompiler from Assembly-CSharp.dll class: AkBank
using System;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;

[AddComponentMenu("Wwise/AkBank")]
[ExecuteInEditMode]
public class AkBank : AkUnityEventHandler, ISerializationCallbackReceiver
{
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
	}

	protected override void Awake()
	{
		base.Awake();
		base.RegisterTriggers(this.unloadTriggerList, new AkTriggerBase.Trigger(this.UnloadBank));
		if (this.unloadTriggerList.Contains(1151176110))
		{
			this.UnloadBank(null);
		}
	}

	protected override void Start()
	{
		base.Start();
		if (this.unloadTriggerList.Contains(1281810935))
		{
			this.UnloadBank(null);
		}
	}

	public override void HandleEvent(GameObject in_gameObject)
	{
		if (!this.loadAsynchronous)
		{
			this.data.Load(this.decodeBank, this.saveDecodedBank);
		}
		else
		{
			this.data.LoadAsync(null);
		}
	}

	public void UnloadBank(GameObject in_gameObject)
	{
		this.data.Unload();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.UnregisterTriggers(this.unloadTriggerList, new AkTriggerBase.Trigger(this.UnloadBank));
		if (this.unloadTriggerList.Contains(-358577003))
		{
			this.UnloadBank(null);
		}
	}

	public Bank data = new Bank();

	public bool decodeBank;

	public bool loadAsynchronous;

	public bool saveDecodedBank;

	public List<int> unloadTriggerList = new List<int>
	{
		-358577003
	};

	[SerializeField]
	[HideInInspector]
	private byte[] valueGuid;
}
