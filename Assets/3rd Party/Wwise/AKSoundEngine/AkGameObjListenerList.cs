// dnSpy decompiler from Assembly-CSharp.dll class: AkGameObjListenerList
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AkGameObjListenerList : AkAudioListener.BaseListenerList
{
	public void SetUseDefaultListeners(bool useDefault)
	{
		if (this.useDefaultListeners != useDefault)
		{
			this.useDefaultListeners = useDefault;
			if (useDefault)
			{
				AkSoundEngine.ResetListenersToDefault(this.akGameObj.gameObject);
				for (int i = 0; i < base.ListenerList.Count; i++)
				{
					AkSoundEngine.AddListener(this.akGameObj.gameObject, base.ListenerList[i].gameObject);
				}
			}
			else
			{
				ulong[] listenerIds = base.GetListenerIds();
				AkSoundEngine.SetListeners(this.akGameObj.gameObject, listenerIds, (uint)((listenerIds != null) ? listenerIds.Length : 0));
			}
		}
	}

	public void Init(AkGameObj akGameObj)
	{
		this.akGameObj = akGameObj;
		if (!this.useDefaultListeners)
		{
			AkSoundEngine.SetListeners(akGameObj.gameObject, null, 0u);
		}
		for (int i = 0; i < this.initialListenerList.Count; i++)
		{
			this.initialListenerList[i].StartListeningToEmitter(akGameObj);
		}
	}

	public override bool Add(AkAudioListener listener)
	{
		bool flag = base.Add(listener);
		if (flag && AkSoundEngine.IsInitialized())
		{
			AkSoundEngine.AddListener(this.akGameObj.gameObject, listener.gameObject);
		}
		return flag;
	}

	public override bool Remove(AkAudioListener listener)
	{
		bool flag = base.Remove(listener);
		if (flag && AkSoundEngine.IsInitialized())
		{
			AkSoundEngine.RemoveListener(this.akGameObj.gameObject, listener.gameObject);
		}
		return flag;
	}

	[NonSerialized]
	private AkGameObj akGameObj;

	[SerializeField]
	public List<AkAudioListener> initialListenerList = new List<AkAudioListener>();

	[SerializeField]
	public bool useDefaultListeners = true;
}
