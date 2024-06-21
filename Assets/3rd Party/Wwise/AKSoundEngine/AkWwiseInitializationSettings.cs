// dnSpy decompiler from Assembly-CSharp.dll class: AkWwiseInitializationSettings
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class AkWwiseInitializationSettings : AkCommonPlatformSettings
{
	public bool IsValid
	{
		get
		{
			return this.PlatformSettingsNameList.Count == this.PlatformSettingsList.Count;
		}
	}

	public int Count
	{
		get
		{
			return this.PlatformSettingsList.Count;
		}
	}

	private void Init()
	{
		if (this.UserSettings == null)
		{
			this.UserSettings = new AkCommonUserSettings
			{
				m_MainOutputSettings = new AkCommonOutputSettings
				{
					m_PanningRule = AkCommonOutputSettings.PanningRule.Headphones,
					m_ChannelConfig = new AkCommonOutputSettings.ChannelConfiguration
					{
						m_ChannelConfigType = AkCommonOutputSettings.ChannelConfiguration.ChannelConfigType.Standard,
						m_ChannelMask = AkCommonOutputSettings.ChannelConfiguration.ChannelMask.SETUP_STEREO
					}
				},
				m_SpatialAudioSettings = new AkCommonUserSettings.SpatialAudioSettings()
			};
		}
		if (this.AdvancedSettings == null)
		{
			this.AdvancedSettings = new AkCommonAdvancedSettings
			{
				m_SpatialAudioSettings = new AkCommonAdvancedSettings.SpatialAudioSettings()
			};
		}
		if (this.CommsSettings == null)
		{
			this.CommsSettings = new AkCommonCommSettings();
		}
	}

	protected override AkCommonUserSettings GetUserSettings()
	{
		return this.UserSettings;
	}

	protected override AkCommonAdvancedSettings GetAdvancedSettings()
	{
		return this.AdvancedSettings;
	}

	protected override AkCommonCommSettings GetCommsSettings()
	{
		return this.CommsSettings;
	}

	public static AkWwiseInitializationSettings Instance
	{
		get
		{
			if (AkWwiseInitializationSettings.m_Instance == null)
			{
				AkWwiseInitializationSettings.m_Instance = ScriptableObject.CreateInstance<AkWwiseInitializationSettings>();
				AkWwiseInitializationSettings.m_Instance.Init();
				UnityEngine.Debug.LogWarning("WwiseUnity: No platform specific settings were created. Default initialization settings will be used.");
			}
			return AkWwiseInitializationSettings.m_Instance;
		}
	}

	private static AkBasePlatformSettings GetPlatformSettings(string platformName)
	{
		AkWwiseInitializationSettings instance = AkWwiseInitializationSettings.Instance;
		if (!instance.IsValid)
		{
			return instance;
		}
		for (int i = 0; i < instance.Count; i++)
		{
			AkWwiseInitializationSettings.PlatformSettings platformSettings = instance.PlatformSettingsList[i];
			if (platformSettings && string.Compare(platformName, instance.PlatformSettingsNameList[i], true) == 0)
			{
				return platformSettings;
			}
		}
		UnityEngine.Debug.LogWarning("WwiseUnity: Platform specific settings cannot be found for <" + platformName + ">. Using global settings.");
		return instance;
	}

	public static AkBasePlatformSettings ActivePlatformSettings
	{
		get
		{
			if (AkWwiseInitializationSettings.m_ActivePlatformSettings == null)
			{
				AkWwiseInitializationSettings.m_ActivePlatformSettings = AkWwiseInitializationSettings.GetPlatformSettings(AkBasePathGetter.GetPlatformName());
			}
			return AkWwiseInitializationSettings.m_ActivePlatformSettings;
		}
	}

	private void OnEnable()
	{
		if (AkWwiseInitializationSettings.m_Instance == null)
		{
			AkWwiseInitializationSettings.m_Instance = this;
		}
		else if (AkWwiseInitializationSettings.m_Instance != this)
		{
			UnityEngine.Debug.LogWarning("WwiseUnity: There are multiple AkWwiseInitializationSettings objects instantiated; only one will be used.");
		}
	}

	public static bool InitializeSoundEngine()
	{
		if (AkSoundEngine.Init(AkWwiseInitializationSettings.ActivePlatformSettings.AkInitializationSettings) != AKRESULT.AK_Success)
		{
			UnityEngine.Debug.LogError("WwiseUnity: Failed to initialize the sound engine. Abort.");
			AkSoundEngine.Term();
			return false;
		}
		if (AkSoundEngine.InitSpatialAudio(AkWwiseInitializationSettings.ActivePlatformSettings.AkSpatialAudioInitSettings) != AKRESULT.AK_Success)
		{
			UnityEngine.Debug.LogWarning("WwiseUnity: Failed to initialize spatial audio.");
		}
		AkSoundEngine.InitCommunication(AkWwiseInitializationSettings.ActivePlatformSettings.AkCommunicationSettings);
		string soundbankBasePath = AkBasePathGetter.GetSoundbankBasePath();
		if (string.IsNullOrEmpty(soundbankBasePath))
		{
			UnityEngine.Debug.LogError("WwiseUnity: Couldn't find soundbanks base path. Terminate sound engine.");
			AkSoundEngine.Term();
			return false;
		}
		if (AkSoundEngine.SetBasePath(soundbankBasePath) != AKRESULT.AK_Success)
		{
			UnityEngine.Debug.LogError("WwiseUnity: Failed to set soundbanks base path. Terminate sound engine.");
			AkSoundEngine.Term();
			return false;
		}
		string decodedBankFullPath = AkSoundEngineController.GetDecodedBankFullPath();
		if (!string.IsNullOrEmpty(decodedBankFullPath))
		{
			AkSoundEngine.SetDecodedBankPath(decodedBankFullPath);
		}
		AkSoundEngine.SetCurrentLanguage(AkWwiseInitializationSettings.ActivePlatformSettings.InitialLanguage);
		AkSoundEngine.AddBasePath(Application.persistentDataPath + Path.DirectorySeparatorChar);
		if (!string.IsNullOrEmpty(decodedBankFullPath))
		{
			AkSoundEngine.AddBasePath(decodedBankFullPath);
		}
		AkCallbackManager.Init(AkWwiseInitializationSettings.ActivePlatformSettings.CallbackManagerInitializationSettings);
		UnityEngine.Debug.Log("WwiseUnity: Sound engine initialized successfully.");
		AkWwiseInitializationSettings.LoadInitBank();
		return true;
	}

	public static bool ResetSoundEngine(bool isPlaying)
	{
		if (isPlaying)
		{
			AkSoundEngine.ClearBanks();
			AkWwiseInitializationSettings.LoadInitBank();
		}
		AkCallbackManager.Init(AkWwiseInitializationSettings.ActivePlatformSettings.CallbackManagerInitializationSettings);
		return true;
	}

	private static void LoadInitBank()
	{
		AkBankManager.Reset();
		uint num;
		AKRESULT akresult = AkSoundEngine.LoadBank("Init.bnk", -1, out num);
		if (akresult != AKRESULT.AK_Success)
		{
			UnityEngine.Debug.LogError("WwiseUnity: Failed load Init.bnk with result: " + akresult);
		}
	}

	public static void TerminateSoundEngine()
	{
		if (!AkSoundEngine.IsInitialized())
		{
			return;
		}
		AkSoundEngine.StopAll();
		AkSoundEngine.ClearBanks();
		AkSoundEngine.RenderAudio();
		int i = 0;
		while (i < 5)
		{
			if (AkCallbackManager.PostCallbacks() == 0)
			{
				AkWwiseInitializationSettings.SleepForMilliseconds(10.0);
				i++;
			}
			AkWwiseInitializationSettings.SleepForMilliseconds(1.0);
		}
		AkSoundEngine.Term();
		AkCallbackManager.PostCallbacks();
		AkCallbackManager.Term();
		AkBankManager.Reset();
	}

	private static void SleepForMilliseconds(double milliseconds)
	{
		using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
		{
			manualResetEvent.WaitOne(TimeSpan.FromMilliseconds(milliseconds));
		}
	}

	[HideInInspector]
	public List<string> PlatformSettingsNameList = new List<string>();

	[HideInInspector]
	public List<AkWwiseInitializationSettings.PlatformSettings> PlatformSettingsList = new List<AkWwiseInitializationSettings.PlatformSettings>();

	[HideInInspector]
	public List<string> InvalidReferencePlatforms = new List<string>();

	[HideInInspector]
	public AkCommonUserSettings UserSettings;

	[HideInInspector]
	public AkCommonAdvancedSettings AdvancedSettings;

	[HideInInspector]
	public AkCommonCommSettings CommsSettings;

	private static readonly string[] AllGlobalValues = new string[]
	{
		"UserSettings.m_BasePath",
		"UserSettings.m_StartupLanguage",
		"UserSettings.m_PreparePoolSize",
		"UserSettings.m_CallbackManagerBufferSize",
		"UserSettings.m_EngineLogging",
		"UserSettings.m_MaximumNumberOfMemoryPools",
		"UserSettings.m_MaximumNumberOfPositioningPaths",
		"UserSettings.m_DefaultPoolSize",
		"UserSettings.m_MemoryCutoffThreshold",
		"UserSettings.m_CommandQueueSize",
		"UserSettings.m_SamplesPerFrame",
		"UserSettings.m_MainOutputSettings.m_AudioDeviceShareset",
		"UserSettings.m_MainOutputSettings.m_DeviceID",
		"UserSettings.m_MainOutputSettings.m_PanningRule",
		"UserSettings.m_MainOutputSettings.m_ChannelConfig.m_ChannelConfigType",
		"UserSettings.m_MainOutputSettings.m_ChannelConfig.m_ChannelMask",
		"UserSettings.m_MainOutputSettings.m_ChannelConfig.m_NumberOfChannels",
		"UserSettings.m_StreamingLookAheadRatio",
		"UserSettings.m_StreamManagerPoolSize",
		"UserSettings.m_SampleRate",
		"UserSettings.m_LowerEnginePoolSize",
		"UserSettings.m_LowerEngineMemoryCutoffThreshold",
		"UserSettings.m_NumberOfRefillsInVoice",
		"UserSettings.m_SpatialAudioSettings.m_PoolSize",
		"UserSettings.m_SpatialAudioSettings.m_MaxSoundPropagationDepth",
		"UserSettings.m_SpatialAudioSettings.m_DiffractionFlags",
		"CommsSettings.m_PoolSize",
		"CommsSettings.m_DiscoveryBroadcastPort",
		"CommsSettings.m_CommandPort",
		"CommsSettings.m_NotificationPort",
		"CommsSettings.m_InitializeSystemComms",
		"CommsSettings.m_NetworkName",
		"AdvancedSettings.m_IOMemorySize",
		"AdvancedSettings.m_TargetAutoStreamBufferLengthMs",
		"AdvancedSettings.m_UseStreamCache",
		"AdvancedSettings.m_MaximumPinnedBytesInCache",
		"AdvancedSettings.m_PrepareEventMemoryPoolID",
		"AdvancedSettings.m_EnableGameSyncPreparation",
		"AdvancedSettings.m_ContinuousPlaybackLookAhead",
		"AdvancedSettings.m_MonitorPoolSize",
		"AdvancedSettings.m_MonitorQueuePoolSize",
		"AdvancedSettings.m_MaximumHardwareTimeoutMs",
		"AdvancedSettings.m_SpatialAudioSettings.m_DiffractionShadowAttenuationFactor",
		"AdvancedSettings.m_SpatialAudioSettings.m_DiffractionShadowDegrees"
	};

	private static AkWwiseInitializationSettings m_Instance = null;

	private static AkBasePlatformSettings m_ActivePlatformSettings = null;

	public abstract class PlatformSettings : AkCommonPlatformSettings
	{
		protected PlatformSettings()
		{
			this.SetGlobalPropertyValues(AkWwiseInitializationSettings.AllGlobalValues);
		}

		public void IgnorePropertyValue(string propertyPath)
		{
			if (this.IsPropertyIgnored(propertyPath))
			{
				return;
			}
			this.IgnorePropertyNameList.Add(propertyPath);
			this.SetUseGlobalPropertyValue(propertyPath, false);
		}

		public bool IsPropertyIgnored(string propertyPath)
		{
			return this.IgnorePropertyNameList.Contains(propertyPath);
		}

		public void SetUseGlobalPropertyValue(string propertyPath, bool use)
		{
			if (this.IsUsingGlobalPropertyValue(propertyPath) == use)
			{
				return;
			}
			if (use)
			{
				this.GlobalPropertyNameList.Add(propertyPath);
			}
			else
			{
				this.GlobalPropertyNameList.Remove(propertyPath);
			}
			this._GlobalPropertyHashSet = null;
		}

		public void SetGlobalPropertyValues(IEnumerable enumerable)
		{
			foreach (object obj in enumerable)
			{
				string text = obj as string;
				if (!this.IsUsingGlobalPropertyValue(text))
				{
					this.GlobalPropertyNameList.Add(text);
				}
			}
		}

		private bool IsUsingGlobalPropertyValue(string propertyPath)
		{
			return this.GlobalPropertyNameList.Contains(propertyPath);
		}

		public HashSet<string> GlobalPropertyHashSet
		{
			get
			{
				if (this._GlobalPropertyHashSet == null)
				{
					this._GlobalPropertyHashSet = new HashSet<string>(this.GlobalPropertyNameList);
				}
				return this._GlobalPropertyHashSet;
			}
			set
			{
				this._GlobalPropertyHashSet = value;
			}
		}

		[SerializeField]
		[HideInInspector]
		private List<string> IgnorePropertyNameList = new List<string>();

		[SerializeField]
		[HideInInspector]
		private List<string> GlobalPropertyNameList = new List<string>();

		private HashSet<string> _GlobalPropertyHashSet;
	}

	public class CommonPlatformSettings : AkWwiseInitializationSettings.PlatformSettings
	{
		protected override AkCommonUserSettings GetUserSettings()
		{
			return this.UserSettings;
		}

		protected override AkCommonAdvancedSettings GetAdvancedSettings()
		{
			return this.AdvancedSettings;
		}

		protected override AkCommonCommSettings GetCommsSettings()
		{
			return this.CommsSettings;
		}

		[HideInInspector]
		public AkCommonUserSettings UserSettings;

		[HideInInspector]
		public AkCommonAdvancedSettings AdvancedSettings;

		[HideInInspector]
		public AkCommonCommSettings CommsSettings;
	}
}
