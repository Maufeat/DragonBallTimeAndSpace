// dnSpy decompiler from Assembly-CSharp.dll class: AkGameObj
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Wwise/AkGameObj")]
[DisallowMultipleComponent]
public class AkGameObj : MonoBehaviour
{
	public bool IsUsingDefaultListeners
	{
		get
		{
			return this.m_listeners.useDefaultListeners;
		}
	}

	public List<AkAudioListener> ListenerList
	{
		get
		{
			return this.m_listeners.ListenerList;
		}
	}

	internal void AddListener(AkAudioListener listener)
	{
		this.m_listeners.Add(listener);
	}

	internal void RemoveListener(AkAudioListener listener)
	{
		this.m_listeners.Remove(listener);
	}

	public AKRESULT Register()
	{
		if (this.isRegistered)
		{
			return AKRESULT.AK_Success;
		}
		this.isRegistered = true;
		return AkSoundEngine.RegisterGameObj(base.gameObject, base.gameObject.name);
	}

	private void Awake()
	{
		if (!this.isStaticObject)
		{
			this.m_posData = new AkGameObjPositionData();
		}
		this.m_Collider = base.GetComponent<Collider>();
		if (this.Register() == AKRESULT.AK_Success)
		{
			AkSoundEngine.SetObjectPosition(base.gameObject, this.GetPosition(), this.GetForward(), this.GetUpward());
			if (this.isEnvironmentAware)
			{
				this.m_envData = new AkGameObjEnvironmentData();
				if (this.m_Collider)
				{
					this.m_envData.AddAkEnvironment(this.m_Collider, this.m_Collider);
				}
				this.m_envData.UpdateAuxSend(base.gameObject, base.transform.position);
			}
			this.m_listeners.Init(this);
		}
	}

	private void CheckStaticStatus()
	{
	}

	private void OnEnable()
	{
		base.enabled = !this.isStaticObject;
	}

	private void OnDestroy()
	{
		AkUnityEventHandler[] components = base.gameObject.GetComponents<AkUnityEventHandler>();
		foreach (AkUnityEventHandler akUnityEventHandler in components)
		{
			if (akUnityEventHandler.triggerList.Contains(-358577003))
			{
				akUnityEventHandler.DoDestroy();
			}
		}
		if (AkSoundEngine.IsInitialized())
		{
			AkSoundEngine.UnregisterGameObj(base.gameObject);
		}
	}

	private void Update()
	{
		if (this.m_envData != null)
		{
			this.m_envData.UpdateAuxSend(base.gameObject, base.transform.position);
		}
		if (this.isStaticObject)
		{
			return;
		}
		Vector3 position = this.GetPosition();
		Vector3 forward = this.GetForward();
		Vector3 upward = this.GetUpward();
		if (this.m_posData.position == position && this.m_posData.forward == forward && this.m_posData.up == upward)
		{
			return;
		}
		this.m_posData.position = position;
		this.m_posData.forward = forward;
		this.m_posData.up = upward;
		AkSoundEngine.SetObjectPosition(base.gameObject, position, forward, upward);
	}

	public virtual Vector3 GetPosition()
	{
		if (this.m_positionOffsetData == null)
		{
			return base.transform.position;
		}
		Vector3 b = base.transform.rotation * this.m_positionOffsetData.positionOffset;
		return base.transform.position + b;
	}

	public virtual Vector3 GetForward()
	{
		return base.transform.forward;
	}

	public virtual Vector3 GetUpward()
	{
		return base.transform.up;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.isEnvironmentAware && this.m_envData != null)
		{
			this.m_envData.AddAkEnvironment(other, this.m_Collider);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (this.isEnvironmentAware && this.m_envData != null)
		{
			this.m_envData.RemoveAkEnvironment(other, this.m_Collider);
		}
	}

	private const int AK_NUM_LISTENERS = 8;

	[SerializeField]
	private AkGameObjListenerList m_listeners = new AkGameObjListenerList();

	public bool isEnvironmentAware = true;

	[SerializeField]
	private bool isStaticObject;

	private Collider m_Collider;

	private AkGameObjEnvironmentData m_envData;

	private AkGameObjPositionData m_posData;

	public AkGameObjPositionOffsetData m_positionOffsetData;

	private bool isRegistered;

	[SerializeField]
	[HideInInspector]
	private AkGameObjPosOffsetData m_posOffsetData;

	[SerializeField]
	[HideInInspector]
	private int listenerMask = 1;
}
