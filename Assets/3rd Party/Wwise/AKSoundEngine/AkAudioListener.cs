// dnSpy decompiler from Assembly-CSharp.dll class: AkAudioListener
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Wwise/AkAudioListener")]
[RequireComponent(typeof(AkGameObj))]
[DisallowMultipleComponent]
public class AkAudioListener : MonoBehaviour
{
	public static AkAudioListener.DefaultListenerList DefaultListeners
	{
		get
		{
			return AkAudioListener.defaultListeners;
		}
	}

	public void StartListeningToEmitter(AkGameObj emitter)
	{
		this.EmittersToStartListeningTo.Add(emitter);
		this.EmittersToStopListeningTo.Remove(emitter);
	}

	public void StopListeningToEmitter(AkGameObj emitter)
	{
		this.EmittersToStartListeningTo.Remove(emitter);
		this.EmittersToStopListeningTo.Add(emitter);
	}

	public void SetIsDefaultListener(bool isDefault)
	{
		if (this.isDefaultListener != isDefault)
		{
			this.isDefaultListener = isDefault;
			if (isDefault)
			{
				AkAudioListener.DefaultListeners.Add(this);
			}
			else
			{
				AkAudioListener.DefaultListeners.Remove(this);
			}
		}
	}

	private void Awake()
	{
		AkGameObj component = base.GetComponent<AkGameObj>();
		if (component)
		{
			component.Register();
		}
		this.akGameObjectID = AkSoundEngine.GetAkGameObjectID(base.gameObject);
	}

	private void OnEnable()
	{
		if (this.isDefaultListener)
		{
			AkAudioListener.DefaultListeners.Add(this);
		}
	}

	private void OnDisable()
	{
		if (this.isDefaultListener)
		{
			AkAudioListener.DefaultListeners.Remove(this);
		}
	}

	private void Update()
	{
		for (int i = 0; i < this.EmittersToStartListeningTo.Count; i++)
		{
			this.EmittersToStartListeningTo[i].AddListener(this);
		}
		this.EmittersToStartListeningTo.Clear();
		for (int j = 0; j < this.EmittersToStopListeningTo.Count; j++)
		{
			this.EmittersToStopListeningTo[j].RemoveListener(this);
		}
		this.EmittersToStopListeningTo.Clear();
	}

	public ulong GetAkGameObjectID()
	{
		return this.akGameObjectID;
	}

	public void Migrate14()
	{
		bool flag = this.listenerId == 0;
		UnityEngine.Debug.Log("WwiseUnity: AkAudioListener.Migrate14 for " + base.gameObject.name);
		this.isDefaultListener = flag;
	}

	private static readonly AkAudioListener.DefaultListenerList defaultListeners = new AkAudioListener.DefaultListenerList();

	private ulong akGameObjectID = ulong.MaxValue;

	private List<AkGameObj> EmittersToStartListeningTo = new List<AkGameObj>();

	private List<AkGameObj> EmittersToStopListeningTo = new List<AkGameObj>();

	public bool isDefaultListener = true;

	[SerializeField]
	public int listenerId;

	public class BaseListenerList
	{
		public List<AkAudioListener> ListenerList
		{
			get
			{
				return this.listenerList;
			}
		}

		public virtual bool Add(AkAudioListener listener)
		{
			if (listener == null)
			{
				return false;
			}
			ulong akGameObjectID = listener.GetAkGameObjectID();
			if (this.listenerIdList.Contains(akGameObjectID))
			{
				return false;
			}
			this.listenerIdList.Add(akGameObjectID);
			this.listenerList.Add(listener);
			return true;
		}

		public virtual bool Remove(AkAudioListener listener)
		{
			if (listener == null)
			{
				return false;
			}
			ulong akGameObjectID = listener.GetAkGameObjectID();
			if (!this.listenerIdList.Contains(akGameObjectID))
			{
				return false;
			}
			this.listenerIdList.Remove(akGameObjectID);
			this.listenerList.Remove(listener);
			return true;
		}

		public ulong[] GetListenerIds()
		{
			return this.listenerIdList.ToArray();
		}

		private readonly List<ulong> listenerIdList = new List<ulong>();

		private readonly List<AkAudioListener> listenerList = new List<AkAudioListener>();
	}

	public class DefaultListenerList : AkAudioListener.BaseListenerList
	{
		public override bool Add(AkAudioListener listener)
		{
			bool flag = base.Add(listener);
			if (flag && AkSoundEngine.IsInitialized())
			{
				AkSoundEngine.AddDefaultListener(listener.gameObject);
			}
			return flag;
		}

		public override bool Remove(AkAudioListener listener)
		{
			bool flag = base.Remove(listener);
			if (flag && AkSoundEngine.IsInitialized())
			{
				AkSoundEngine.RemoveDefaultListener(listener.gameObject);
			}
			return flag;
		}
	}
}
