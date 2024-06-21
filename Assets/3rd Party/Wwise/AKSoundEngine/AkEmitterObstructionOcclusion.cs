// dnSpy decompiler from Assembly-CSharp.dll class: AkEmitterObstructionOcclusion
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AkGameObj))]
[AddComponentMenu("Wwise/AkEmitterObstructionOcclusion")]
public class AkEmitterObstructionOcclusion : AkObstructionOcclusion
{
	private void Awake()
	{
		base.InitIntervalsAndFadeRates();
		this.m_gameObj = base.GetComponent<AkGameObj>();
	}

	protected override void UpdateObstructionOcclusionValuesForListeners()
	{
		if (AkRoom.IsSpatialAudioEnabled)
		{
			base.UpdateObstructionOcclusionValues(AkSpatialAudioListener.TheSpatialAudioListener);
		}
		else
		{
			if (this.m_gameObj.IsUsingDefaultListeners)
			{
				base.UpdateObstructionOcclusionValues(AkAudioListener.DefaultListeners.ListenerList);
			}
			base.UpdateObstructionOcclusionValues(this.m_gameObj.ListenerList);
		}
	}

	protected override void SetObstructionOcclusion(KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> ObsOccPair)
	{
		if (AkRoom.IsSpatialAudioEnabled)
		{
			AkSoundEngine.SetEmitterObstructionAndOcclusion(base.gameObject, ObsOccPair.Value.currentValue, 0f);
		}
		else
		{
			AkSoundEngine.SetObjectObstructionAndOcclusion(base.gameObject, ObsOccPair.Key.gameObject, 0f, ObsOccPair.Value.currentValue);
		}
	}

	private AkGameObj m_gameObj;
}
