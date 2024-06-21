// dnSpy decompiler from Assembly-CSharp.dll class: AkCommonAdvancedSettings
using System;
using UnityEngine;

[Serializable]
public class AkCommonAdvancedSettings : AkSettingsValidationHandler
{
	public virtual void CopyTo(AkDeviceSettings settings)
	{
		settings.uIOMemorySize = this.m_IOMemorySize;
		settings.fTargetAutoStmBufferLength = this.m_TargetAutoStreamBufferLengthMs;
		settings.bUseStreamCache = this.m_UseStreamCache;
		settings.uMaxCachePinnedBytes = this.m_MaximumPinnedBytesInCache;
	}

	public virtual void CopyTo(AkInitSettings settings)
	{
		settings.uPrepareEventMemoryPoolID = this.m_PrepareEventMemoryPoolID;
		settings.bEnableGameSyncPreparation = this.m_EnableGameSyncPreparation;
		settings.uContinuousPlaybackLookAhead = this.m_ContinuousPlaybackLookAhead;
		settings.uMonitorPoolSize = this.m_MonitorPoolSize;
		settings.uMonitorQueuePoolSize = this.m_MonitorQueuePoolSize;
		settings.uMaxHardwareTimeoutMs = this.m_MaximumHardwareTimeoutMs;
	}

	public virtual void CopyTo(AkSpatialAudioInitSettings settings)
	{
		settings.fDiffractionShadowAttenFactor = this.m_SpatialAudioSettings.m_DiffractionShadowAttenuationFactor;
		settings.fDiffractionShadowDegrees = this.m_SpatialAudioSettings.m_DiffractionShadowDegrees;
	}

	[Tooltip("Size of memory pool for I/O (for automatic streams). It is passed directly to AK::MemoryMgr::CreatePool(), after having been rounded down to a multiple of uGranularity.")]
	public uint m_IOMemorySize = 2097152u;

	[Tooltip("Targeted automatic stream buffer length (ms). When a stream reaches that buffering, it stops being scheduled for I/O except if the scheduler is idle.")]
	public float m_TargetAutoStreamBufferLengthMs = 380f;

	[Tooltip("If true the device attempts to reuse IO buffers that have already been streamed from disk. This is particularly useful when streaming small looping sounds. The drawback is a small CPU hit when allocating memory, and a slightly larger memory footprint in the StreamManager pool.")]
	public bool m_UseStreamCache;

	[Tooltip("Maximum number of bytes that can be \"pinned\" using AK::SoundEngine::PinEventInStreamCache() or AK::IAkStreamMgr::PinFileInCache()")]
	public uint m_MaximumPinnedBytesInCache = uint.MaxValue;

	[Tooltip("Memory pool where data allocated by AK::SoundEngine::PrepareEvent() and AK::SoundEngine::PrepareGameSyncs() will be done.")]
	public int m_PrepareEventMemoryPoolID = -1;

	[Tooltip("Set to true to enable AK::SoundEngine::PrepareGameSync usage.")]
	public bool m_EnableGameSyncPreparation;

	[Tooltip("Number of quanta ahead when continuous containers should instantiate a new voice before which next sounds should start playing. This look-ahead time allows I/O to occur, and is especially useful to reduce the latency of continuous containers with trigger rate or sample-accurate transitions.")]
	public uint m_ContinuousPlaybackLookAhead = 1u;

	[Tooltip("Size of the monitoring pool. This parameter is not used in Release build.")]
	public uint m_MonitorPoolSize = 262144u;

	[Tooltip("Size of the monitoring queue pool. This parameter is not used in Release build.")]
	public uint m_MonitorQueuePoolSize = 65536u;

	[Tooltip("Amount of time to wait for hardware devices to trigger an audio interrupt. If there is no interrupt after that time, the sound engine will revert to silent mode and continue operating until the hardware finally comes back.")]
	public uint m_MaximumHardwareTimeoutMs = 1000u;

	[Tooltip("Spatial audio advanced settings.")]
	public AkCommonAdvancedSettings.SpatialAudioSettings m_SpatialAudioSettings;

	[Serializable]
	public class SpatialAudioSettings
	{
		[Tooltip("Multiplier that is applied to the distance attenuation of diffracted sounds (sounds that are in the 'shadow region') to simulate the phenomenon where by diffracted sound waves decay faster than incident sound waves.")]
		public float m_DiffractionShadowAttenuationFactor;

		[Tooltip("Interpolation angle, in degrees, over which the \"Diffraction Shadow Attenuation Factor\" is applied.")]
		public float m_DiffractionShadowDegrees;
	}
}
