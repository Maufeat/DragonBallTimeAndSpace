// dnSpy decompiler from Assembly-CSharp.dll class: AkSpatialAudioListener
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Wwise/AkSpatialAudioListener")]
[DisallowMultipleComponent]
[RequireComponent(typeof(AkAudioListener))]
public class AkSpatialAudioListener : AkSpatialAudioBase
{
	public static AkAudioListener TheSpatialAudioListener
	{
		get
		{
			return (!(AkSpatialAudioListener.s_SpatialAudioListener != null)) ? null : AkSpatialAudioListener.s_SpatialAudioListener.AkAudioListener;
		}
	}

	public static AkSpatialAudioListener.SpatialAudioListenerList SpatialAudioListeners
	{
		get
		{
			return AkSpatialAudioListener.spatialAudioListeners;
		}
	}

	private void Awake()
	{
		this.AkAudioListener = base.GetComponent<AkAudioListener>();
	}

	private void OnEnable()
	{
		AkSpatialAudioListener.spatialAudioListeners.Add(this);
	}

	private void OnDisable()
	{
		AkSpatialAudioListener.spatialAudioListeners.Remove(this);
	}

	private static AkSpatialAudioListener s_SpatialAudioListener;

	private static readonly AkSpatialAudioListener.SpatialAudioListenerList spatialAudioListeners = new AkSpatialAudioListener.SpatialAudioListenerList();

	private AkAudioListener AkAudioListener;

	public class SpatialAudioListenerList
	{
		public List<AkSpatialAudioListener> ListenerList
		{
			get
			{
				return this.listenerList;
			}
		}

		public bool Add(AkSpatialAudioListener listener)
		{
			if (listener == null)
			{
				return false;
			}
			if (this.listenerList.Contains(listener))
			{
				return false;
			}
			this.listenerList.Add(listener);
			this.Refresh();
			return true;
		}

		public bool Remove(AkSpatialAudioListener listener)
		{
			if (listener == null)
			{
				return false;
			}
			if (!this.listenerList.Contains(listener))
			{
				return false;
			}
			this.listenerList.Remove(listener);
			this.Refresh();
			return true;
		}

		private void Refresh()
		{
			if (this.ListenerList.Count == 1)
			{
				if (AkSpatialAudioListener.s_SpatialAudioListener != null)
				{
					AkSoundEngine.UnregisterSpatialAudioListener(AkSpatialAudioListener.s_SpatialAudioListener.gameObject);
				}
				AkSpatialAudioListener.s_SpatialAudioListener = this.ListenerList[0];
				if (AkSoundEngine.RegisterSpatialAudioListener(AkSpatialAudioListener.s_SpatialAudioListener.gameObject) == AKRESULT.AK_Success)
				{
					AkSpatialAudioListener.s_SpatialAudioListener.SetGameObjectInRoom();
				}
			}
			else if (this.ListenerList.Count == 0 && AkSpatialAudioListener.s_SpatialAudioListener != null)
			{
				AkSoundEngine.UnregisterSpatialAudioListener(AkSpatialAudioListener.s_SpatialAudioListener.gameObject);
				AkSpatialAudioListener.s_SpatialAudioListener = null;
			}
		}

		private readonly List<AkSpatialAudioListener> listenerList = new List<AkSpatialAudioListener>();
	}
}
