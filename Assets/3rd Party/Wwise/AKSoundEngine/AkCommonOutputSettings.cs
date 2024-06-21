// dnSpy decompiler from Assembly-CSharp.dll class: AkCommonOutputSettings
using System;
using UnityEngine;

[Serializable]
public class AkCommonOutputSettings
{
	public void CopyTo(AkOutputSettings settings)
	{
		settings.audioDeviceShareset = ((!string.IsNullOrEmpty(this.m_AudioDeviceShareset)) ? AkUtilities.ShortIDGenerator.Compute(this.m_AudioDeviceShareset) : 0u);
		settings.idDevice = this.m_DeviceID;
		settings.ePanningRule = (AkPanningRule)this.m_PanningRule;
		this.m_ChannelConfig.CopyTo(settings.channelConfig);
	}

	[Tooltip("The name of a custom audio device to be used. Custom audio devices are defined in the Audio Device Shareset section of the Wwise project. Leave this empty to output normally through the default audio device.")]
	public string m_AudioDeviceShareset = string.Empty;

	[Tooltip("Device specific identifier, when multiple devices of the same type are possible.  If only one device is possible, leave to 0.")]
	public uint m_DeviceID;

	[Tooltip("Rule for 3D panning of signals routed to a stereo bus. In \"Speakers\" mode, the angle of the front loudspeakers is used. In \"Headphones\" mode, the speaker angles are superseded with constant power panning between two virtual microphones spaced 180 degrees apart.")]
	public AkCommonOutputSettings.PanningRule m_PanningRule;

	[Tooltip("Channel configuration for this output. Hardware might not support the selected configuration.")]
	public AkCommonOutputSettings.ChannelConfiguration m_ChannelConfig = new AkCommonOutputSettings.ChannelConfiguration();

	public enum PanningRule
	{
		Speakers,
		Headphones
	}

	[Serializable]
	public class ChannelConfiguration
	{
		public void CopyTo(AkChannelConfig config)
		{
			switch (this.m_ChannelConfigType)
			{
			case AkCommonOutputSettings.ChannelConfiguration.ChannelConfigType.Anonymous:
				config.SetAnonymous(this.m_NumberOfChannels);
				break;
			case AkCommonOutputSettings.ChannelConfiguration.ChannelConfigType.Standard:
				config.SetStandard((uint)this.m_ChannelMask);
				break;
			case AkCommonOutputSettings.ChannelConfiguration.ChannelConfigType.Ambisonic:
				config.SetAmbisonic(this.m_NumberOfChannels);
				break;
			}
		}

		[Tooltip("A code that completes the identification of channels by uChannelMask. Anonymous: Channel mask == 0 and channels; Standard: Channels must be identified with standard defines in AkSpeakerConfigs; Ambisonic: Channel mask == 0 and channels follow standard ambisonic order.")]
		public AkCommonOutputSettings.ChannelConfiguration.ChannelConfigType m_ChannelConfigType;

		[Tooltip("A bit field, whose channel identifiers depend on AkChannelConfigType (up to 20).")]
		[AkEnumFlag(typeof(AkCommonOutputSettings.ChannelConfiguration.ChannelMask))]
		public AkCommonOutputSettings.ChannelConfiguration.ChannelMask m_ChannelMask;

		[Tooltip("The number of channels, identified (deduced from channel mask) or anonymous (set directly).")]
		public uint m_NumberOfChannels;

		public enum ChannelConfigType
		{
			Anonymous,
			Standard,
			Ambisonic
		}

		public enum ChannelMask
		{
			NONE,
			FRONT_LEFT,
			FRONT_RIGHT,
			FRONT_CENTER = 4,
			LOW_FREQUENCY = 8,
			BACK_LEFT = 16,
			BACK_RIGHT = 32,
			BACK_CENTER = 256,
			SIDE_LEFT = 512,
			SIDE_RIGHT = 1024,
			TOP = 2048,
			HEIGHT_FRONT_LEFT = 4096,
			HEIGHT_FRONT_CENTER = 8192,
			HEIGHT_FRONT_RIGHT = 16384,
			HEIGHT_BACK_LEFT = 32768,
			HEIGHT_BACK_CENTER = 65536,
			HEIGHT_BACK_RIGHT = 131072,
			SETUP_MONO = 4,
			SETUP_0POINT1 = 8,
			SETUP_1POINT1 = 12,
			SETUP_STEREO = 3,
			SETUP_2POINT1 = 11,
			SETUP_3STEREO = 7,
			SETUP_3POINT1 = 15,
			SETUP_4 = 1539,
			SETUP_4POINT1 = 1547,
			SETUP_5 = 1543,
			SETUP_5POINT1 = 1551,
			SETUP_6 = 1587,
			SETUP_6POINT1 = 1595,
			SETUP_7 = 1591,
			SETUP_7POINT1 = 1599,
			SETUP_SURROUND = 259,
			SETUP_DPL2 = 1539,
			SETUP_HEIGHT_4 = 184320,
			SETUP_HEIGHT_5 = 192512,
			SETUP_HEIGHT_ALL = 258048,
			SETUP_AURO_222 = 22019,
			SETUP_AURO_8 = 185859,
			SETUP_AURO_9 = 185863,
			SETUP_AURO_9POINT1 = 185871,
			SETUP_AURO_10 = 187911,
			SETUP_AURO_10POINT1 = 187919,
			SETUP_AURO_11 = 196103,
			SETUP_AURO_11POINT1 = 196111,
			SETUP_AURO_11_740 = 185911,
			SETUP_AURO_11POINT1_740 = 185919,
			SETUP_AURO_13_751 = 196151,
			SETUP_AURO_13POINT1_751 = 196159,
			SETUP_DOLBY_5_0_2 = 22023,
			SETUP_DOLBY_5_1_2 = 22031,
			SETUP_DOLBY_6_0_2 = 22067,
			SETUP_DOLBY_6_1_2 = 22075,
			SETUP_DOLBY_6_0_4 = 185907,
			SETUP_DOLBY_6_1_4 = 185915,
			SETUP_DOLBY_7_0_2 = 22071,
			SETUP_DOLBY_7_1_2 = 22079,
			SETUP_DOLBY_7_0_4 = 185911,
			SETUP_DOLBY_7_1_4 = 185919,
			SETUP_ALL_SPEAKERS = 261951
		}
	}
}
