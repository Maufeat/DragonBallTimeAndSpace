// dnSpy decompiler from Assembly-CSharp.dll class: AkEvent
using System;
using AK.Wwise;
using UnityEngine;

[AddComponentMenu("Wwise/AkEvent")]
[RequireComponent(typeof(AkGameObj))]
public class AkEvent : AkUnityEventHandler, ISerializationCallbackReceiver
{
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
	}

	[Obsolete("This functionality is deprecated as of Wwise v2018.1.2 and will be removed in a future release.")]
	public int eventID
	{
		get
		{
			return (int)((this.data != null) ? this.data.Id : 0u);
		}
	}

	private void Callback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
	{
		for (int i = 0; i < this.m_callbackData.callbackFunc.Count; i++)
		{
			if ((in_type & (AkCallbackType)this.m_callbackData.callbackFlags[i]) != (AkCallbackType)0 && this.m_callbackData.callbackGameObj[i] != null)
			{
				AkEventCallbackMsg value = new AkEventCallbackMsg
				{
					type = in_type,
					sender = base.gameObject,
					info = in_info
				};
				this.m_callbackData.callbackGameObj[i].SendMessage(this.m_callbackData.callbackFunc[i], value);
			}
		}
	}

	public override void HandleEvent(GameObject in_gameObject)
	{
		GameObject gameObject = (!this.useOtherObject || !(in_gameObject != null)) ? base.gameObject : in_gameObject;
		this.soundEmitterObject = gameObject;
		if (this.enableActionOnEvent)
		{
			this.data.ExecuteAction(gameObject, this.actionOnEventType, (int)this.transitionDuration * 1000, this.curveInterpolation);
			return;
		}
		if (this.m_callbackData != null)
		{
			this.playingId = this.data.Post(gameObject, (uint)this.m_callbackData.uFlags, new AkCallbackManager.EventCallback(this.Callback), null);
		}
		else
		{
			this.playingId = this.data.Post(gameObject);
		}
	}

	public void Stop(int _transitionDuration)
	{
		this.Stop(_transitionDuration, AkCurveInterpolation.AkCurveInterpolation_Linear);
	}

	public void Stop(int _transitionDuration, AkCurveInterpolation _curveInterpolation)
	{
		this.data.Stop(this.soundEmitterObject, _transitionDuration, _curveInterpolation);
	}

	public AkActionOnEventType actionOnEventType;

	public AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear;

	public bool enableActionOnEvent;

	public AK.Wwise.Event data = new AK.Wwise.Event();

	public AkEventCallbackData m_callbackData;

	public uint playingId;

	public GameObject soundEmitterObject;

	public float transitionDuration;

	[HideInInspector]
	[SerializeField]
	private byte[] valueGuid;
}
