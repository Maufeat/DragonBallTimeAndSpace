// dnSpy decompiler from Assembly-CSharp.dll class: AkRoomPortalObstruction
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AkRoomPortal))]
[AddComponentMenu("Wwise/AkRoomPortalObstruction")]
public class AkRoomPortalObstruction : AkObstructionOcclusion
{
	private void Awake()
	{
		base.InitIntervalsAndFadeRates();
		this.m_portal = base.GetComponent<AkRoomPortal>();
	}

	protected override void UpdateObstructionOcclusionValuesForListeners()
	{
		base.UpdateObstructionOcclusionValues(AkSpatialAudioListener.TheSpatialAudioListener);
	}

	protected override void SetObstructionOcclusion(KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> ObsOccPair)
	{
		AkSoundEngine.SetPortalObstructionAndOcclusion(this.m_portal.GetID(), ObsOccPair.Value.currentValue, 0f);
	}

	private AkRoomPortal m_portal;
}
