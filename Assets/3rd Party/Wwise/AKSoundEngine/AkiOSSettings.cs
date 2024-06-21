// dnSpy decompiler from Assembly-CSharp.dll class: AkiOSSettings
using System;
using UnityEngine;

public class AkiOSSettings : AkWwiseInitializationSettings.PlatformSettings
{
	public AkiOSSettings()
	{
		base.SetUseGlobalPropertyValue("UserSettings.m_MainOutputSettings.m_PanningRule", false);
		base.SetUseGlobalPropertyValue("UserSettings.m_MainOutputSettings.m_ChannelConfig.m_ChannelConfigType", false);
		base.SetUseGlobalPropertyValue("UserSettings.m_MainOutputSettings.m_ChannelConfig.m_ChannelMask", false);
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

	[HideInInspector]
	public AkCommonUserSettings UserSettings = new AkCommonUserSettings
	{
		m_MainOutputSettings = new AkCommonOutputSettings
		{
			m_PanningRule = AkCommonOutputSettings.PanningRule.Headphones,
			m_ChannelConfig = new AkCommonOutputSettings.ChannelConfiguration
			{
				m_ChannelConfigType = AkCommonOutputSettings.ChannelConfiguration.ChannelConfigType.Standard,
				m_ChannelMask = AkCommonOutputSettings.ChannelConfiguration.ChannelMask.SETUP_STEREO
			}
		}
	};

	[HideInInspector]
	public AkiOSSettings.PlatformAdvancedSettings AdvancedSettings;

	[HideInInspector]
	public AkCommonCommSettings CommsSettings;

	[Serializable]
	public class PlatformAdvancedSettings : AkCommonAdvancedSettings
	{
		public void CopyTo(AkPlatformInitSettings settings)
		{
		}

		[Tooltip("The IDs of the iOS audio session categories, useful for defining app-level audio behaviours such as inter-app audio mixing policies and audio routing behaviours.These IDs are functionally equivalent to the corresponding constants defined by the iOS audio session service back-end (AVAudioSession). Refer to Xcode documentation for details on the audio session categories.")]
		public AkiOSSettings.PlatformAdvancedSettings.Category m_AudioSessionCategory = AkiOSSettings.PlatformAdvancedSettings.Category.SoloAmbient;

		[AkEnumFlag(typeof(AkiOSSettings.PlatformAdvancedSettings.CategoryOptions))]
		[Tooltip("The IDs of the iOS audio session category options, used for customizing the audio session category features. These IDs are functionally equivalent to the corresponding constants defined by the iOS audio session service back-end (AVAudioSession). Refer to Xcode documentation for details on the audio session category options.")]
		public AkiOSSettings.PlatformAdvancedSettings.CategoryOptions m_AudioSessionCategoryOptions = AkiOSSettings.PlatformAdvancedSettings.CategoryOptions.DuckOthers;

		[Tooltip("The IDs of the iOS audio session modes, used for customizing the audio session for typical app types. These IDs are functionally equivalent to the corresponding constants defined by the iOS audio session service back-end (AVAudioSession). Refer to Xcode documentation for details on the audio session category options.")]
		public AkiOSSettings.PlatformAdvancedSettings.Mode m_AudioSessionMode;

		public enum Category
		{
			Ambient,
			SoloAmbient,
			PlayAndRecord
		}

		public enum CategoryOptions
		{
			MixWithOthers = 1,
			DuckOthers,
			AllowBluetooth = 4,
			DefaultToSpeaker = 8
		}

		public enum Mode
		{
			Default,
			VoiceChat,
			GameChat,
			VideoRecording,
			Measurement,
			MoviePlayback,
			VideoChat
		}
	}
}
