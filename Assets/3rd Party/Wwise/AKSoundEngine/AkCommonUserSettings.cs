// dnSpy decompiler from Assembly-CSharp.dll class: AkCommonUserSettings
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class AkCommonUserSettings : AkSettingsValidationHandler
{
	public void CopyTo(AkMemSettings settings)
	{
		settings.uMaxNumPools = this.m_MaximumNumberOfMemoryPools;
	}

	public virtual void CopyTo(AkInitSettings settings)
	{
		settings.uMaxNumPaths = this.m_MaximumNumberOfPositioningPaths;
		settings.uDefaultPoolSize = this.m_DefaultPoolSize;
		settings.fDefaultPoolRatioThreshold = this.m_MemoryCutoffThreshold;
		settings.uCommandQueueSize = this.m_CommandQueueSize;
		settings.uNumSamplesPerFrame = this.m_SamplesPerFrame;
		this.m_MainOutputSettings.CopyTo(settings.settingsMainOutput);
		settings.szPluginDLLPath = Path.Combine(Application.dataPath, "Plugins" + Path.DirectorySeparatorChar);
	}

	public void CopyTo(AkMusicSettings settings)
	{
		settings.fStreamingLookAheadRatio = this.m_StreamingLookAheadRatio;
	}

	public void CopyTo(AkStreamMgrSettings settings)
	{
		settings.uMemorySize = this.m_StreamManagerPoolSize;
	}

	public virtual void CopyTo(AkDeviceSettings settings)
	{
	}

	public virtual void CopyTo(AkPlatformInitSettings settings)
	{
		settings.uSampleRate = this.m_SampleRate;
		settings.uLEngineDefaultPoolSize = this.m_LowerEnginePoolSize;
		settings.fLEngineDefaultPoolRatioThreshold = this.m_LowerEngineMemoryCutoffThreshold;
		settings.uNumRefillsInVoice = this.m_NumberOfRefillsInVoice;
	}

	public virtual void CopyTo(AkSpatialAudioInitSettings settings)
	{
		settings.uPoolSize = this.m_SpatialAudioSettings.m_PoolSize;
		settings.uMaxSoundPropagationDepth = this.m_SpatialAudioSettings.m_MaxSoundPropagationDepth;
		settings.uDiffractionFlags = (uint)this.m_SpatialAudioSettings.m_DiffractionFlags;
	}

	public override void Validate()
	{
		if (this.m_PreparePoolSize > 0u && this.m_PreparePoolSize < 8096u)
		{
			this.m_PreparePoolSize = 8096u;
		}
	}

	[Tooltip("Path for the soundbanks. This must contain one sub folder per platform, with the same as in the Wwise project.")]
	public string m_BasePath = AkBasePathGetter.DefaultBasePath;

	[Tooltip("Language sub-folder used at startup.")]
	public string m_StartupLanguage = "English(US)";

	[Tooltip("Prepare Pool size. This contains the banks loaded using PrepareBank (Banks decoded on load use this). Default size is 0 MB (will not allocate a Prepare Pool), and minimal size if 8096 bytes. This should be adjusted for your needs.")]
	public uint m_PreparePoolSize;

	[Tooltip("CallbackManager buffer size. The size of the buffer used per-frame to transfer callback data. Default size is 4 KB, but you should increase this, if required.")]
	public int m_CallbackManagerBufferSize = AkCallbackManager.InitializationSettings.DefaultBufferSize;

	[Tooltip("Enable Wwise engine logging. This is used to turn on/off the logging of the Wwise engine.")]
	public bool m_EngineLogging = AkCallbackManager.InitializationSettings.DefaultIsLoggingEnabled;

	[Tooltip("Maximum number of memory pools. A memory pool is required for each loaded bank.")]
	public uint m_MaximumNumberOfMemoryPools = 32u;

	[Tooltip("Maximum number of automation paths for positioning sounds.")]
	public uint m_MaximumNumberOfPositioningPaths = 255u;

	[Tooltip("Size of the default memory pool.")]
	public uint m_DefaultPoolSize = 16777216u;

	[Range(0f, 1f)]
	[Tooltip("The percentage of occupied memory where the sound engine should enter in Low memory Mode.")]
	public float m_MemoryCutoffThreshold = 1f;

	[Tooltip("Size of the command queue.")]
	public uint m_CommandQueueSize = 262144u;

	[Tooltip("Number of samples per audio frame (256, 512, 1024, or 2048).")]
	public uint m_SamplesPerFrame = 1024u;

	[Tooltip("Main output device settings.")]
	public AkCommonOutputSettings m_MainOutputSettings;

	[Tooltip("Multiplication factor for all streaming look-ahead heuristic values.")]
	[Range(0f, 1f)]
	public float m_StreamingLookAheadRatio = 1f;

	[Tooltip("Size of memory pool for small objects of Stream Manager. Small objects are the Stream Manager instance, devices, stream objects, user stream names, pending transfers, buffer records, pending open commands, and so on. Ideally, this pool should never run out of memory, because it may cause undesired I/O transfer cancellation, and even major CPU spikes. I/O memory should be bound by the size of each device's I/O pool instead.")]
	public uint m_StreamManagerPoolSize = 65536u;

	[Tooltip("Sampling Rate. Default is 48000 Hz. Use 24000hz for low quality. Any positive reasonable sample rate is supported; however, be careful setting a custom value. Using an odd or really low sample rate may cause the sound engine to malfunction.")]
	public uint m_SampleRate = 48000u;

	[Tooltip("Lower Engine default memory pool size.")]
	public uint m_LowerEnginePoolSize = 16777216u;

	[Range(0f, 1f)]
	[Tooltip("The percentage of occupied memory where the sound engine should enter in Low memory mode.")]
	public float m_LowerEngineMemoryCutoffThreshold = 1f;

	[Tooltip("Number of refill buffers in voice buffer. Set to 2 for double-buffered, defaults to 4.")]
	public ushort m_NumberOfRefillsInVoice = 4;

	[Tooltip("Spatial audio common settings.")]
	public AkCommonUserSettings.SpatialAudioSettings m_SpatialAudioSettings;

	[Serializable]
	public class SpatialAudioSettings
	{
		[Tooltip("Desired spatial audio memory pool size.")]
		public uint m_PoolSize = 4194304u;

		[Range(0f, 8f)]
		[Tooltip("Maximum number of portals that sound can propagate through.")]
		public uint m_MaxSoundPropagationDepth = 8u;

		[AkEnumFlag(typeof(AkCommonUserSettings.SpatialAudioSettings.DiffractionFlags))]
		[Tooltip("Determines whether diffraction values for sound passing through portals will be calculated, and how to apply those calculations to Wwise parameters.")]
		public AkCommonUserSettings.SpatialAudioSettings.DiffractionFlags m_DiffractionFlags = (AkCommonUserSettings.SpatialAudioSettings.DiffractionFlags)(-1);

		public enum DiffractionFlags
		{
			UseBuiltInParam = 1,
			UseObstruction,
			CalcEmitterVirtualPosition = 8
		}
	}
}
