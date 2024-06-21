// dnSpy decompiler from Assembly-CSharp.dll class: AkAmbient
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Wwise/AkAmbient")]
public class AkAmbient : AkEvent
{
	public AkAmbient ParentAkAmbience { get; set; }

	private void OnEnable()
	{
		if (this.multiPositionTypeLabel == MultiPositionTypeLabel.Simple_Mode)
		{
			AkGameObj[] components = base.gameObject.GetComponents<AkGameObj>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].enabled = true;
			}
		}
		else if (this.multiPositionTypeLabel == MultiPositionTypeLabel.Large_Mode)
		{
			AkGameObj[] components2 = base.gameObject.GetComponents<AkGameObj>();
			for (int j = 0; j < components2.Length; j++)
			{
				components2[j].enabled = false;
			}
			AkPositionArray akPositionArray = this.BuildAkPositionArray();
			AkSoundEngine.SetMultiplePositions(base.gameObject, akPositionArray, (ushort)akPositionArray.Count, this.MultiPositionType);
		}
		else if (this.multiPositionTypeLabel == MultiPositionTypeLabel.MultiPosition_Mode)
		{
			AkGameObj[] components3 = base.gameObject.GetComponents<AkGameObj>();
			for (int k = 0; k < components3.Length; k++)
			{
				components3[k].enabled = false;
			}
			AkMultiPosEvent akMultiPosEvent;
			if (AkAmbient.multiPosEventTree.TryGetValue(this.data.Id, out akMultiPosEvent))
			{
				if (!akMultiPosEvent.list.Contains(this))
				{
					akMultiPosEvent.list.Add(this);
				}
			}
			else
			{
				akMultiPosEvent = new AkMultiPosEvent();
				akMultiPosEvent.list.Add(this);
				AkAmbient.multiPosEventTree.Add(this.data.Id, akMultiPosEvent);
			}
			AkPositionArray akPositionArray2 = this.BuildMultiDirectionArray(akMultiPosEvent);
			AkSoundEngine.SetMultiplePositions(akMultiPosEvent.list[0].gameObject, akPositionArray2, (ushort)akPositionArray2.Count, this.MultiPositionType);
		}
	}

	private void OnDisable()
	{
		if (this.multiPositionTypeLabel == MultiPositionTypeLabel.MultiPosition_Mode)
		{
			AkMultiPosEvent akMultiPosEvent = AkAmbient.multiPosEventTree[this.data.Id];
			if (akMultiPosEvent.list.Count == 1)
			{
				AkAmbient.multiPosEventTree.Remove(this.data.Id);
			}
			else
			{
				akMultiPosEvent.list.Remove(this);
				AkPositionArray akPositionArray = this.BuildMultiDirectionArray(akMultiPosEvent);
				AkSoundEngine.SetMultiplePositions(akMultiPosEvent.list[0].gameObject, akPositionArray, (ushort)akPositionArray.Count, this.MultiPositionType);
			}
		}
	}

	public override void HandleEvent(GameObject in_gameObject)
	{
		if (this.multiPositionTypeLabel != MultiPositionTypeLabel.MultiPosition_Mode)
		{
			base.HandleEvent(in_gameObject);
		}
		else
		{
			AkMultiPosEvent akMultiPosEvent = AkAmbient.multiPosEventTree[this.data.Id];
			if (akMultiPosEvent.eventIsPlaying)
			{
				return;
			}
			akMultiPosEvent.eventIsPlaying = true;
			this.soundEmitterObject = akMultiPosEvent.list[0].gameObject;
			if (this.enableActionOnEvent)
			{
				this.data.ExecuteAction(this.soundEmitterObject, this.actionOnEventType, (int)this.transitionDuration * 1000, this.curveInterpolation);
			}
			else
			{
				this.playingId = this.data.Post(this.soundEmitterObject, 1u, new AkCallbackManager.EventCallback(akMultiPosEvent.FinishedPlaying), null);
			}
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.DrawIcon(base.transform.position, "WwiseAudioSpeaker.png", false);
	}

	public AkPositionArray BuildMultiDirectionArray(AkMultiPosEvent eventPosList)
	{
		AkPositionArray akPositionArray = new AkPositionArray((uint)eventPosList.list.Count);
		for (int i = 0; i < eventPosList.list.Count; i++)
		{
			akPositionArray.Add(eventPosList.list[i].transform.position, eventPosList.list[i].transform.forward, eventPosList.list[i].transform.up);
		}
		return akPositionArray;
	}

	private AkPositionArray BuildAkPositionArray()
	{
		AkPositionArray akPositionArray = new AkPositionArray((uint)this.multiPositionArray.Count);
		for (int i = 0; i < this.multiPositionArray.Count; i++)
		{
			akPositionArray.Add(base.transform.position + this.multiPositionArray[i], base.transform.forward, base.transform.up);
		}
		return akPositionArray;
	}

	public static Dictionary<uint, AkMultiPosEvent> multiPosEventTree = new Dictionary<uint, AkMultiPosEvent>();

	public List<Vector3> multiPositionArray = new List<Vector3>();

	public AkMultiPositionType MultiPositionType = AkMultiPositionType.MultiPositionType_MultiSources;

	public MultiPositionTypeLabel multiPositionTypeLabel;
}
