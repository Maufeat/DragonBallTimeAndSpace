// dnSpy decompiler from Assembly-CSharp.dll class: AkAndroidSettings
using System;
using UnityEngine;

public class AkAndroidSettings : AkWwiseInitializationSettings.PlatformSettings
{
	public AkAndroidSettings()
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
	public AkAndroidSettings.PlatformAdvancedSettings AdvancedSettings;

	[HideInInspector]
	public AkCommonCommSettings CommsSettings;

	[Serializable]
	public class PlatformAdvancedSettings : AkCommonAdvancedSettings
	{
		public void CopyTo(AkPlatformInitSettings settings)
		{
		}

		[Tooltip("Used when hardware-preferred frame size and user-preferred frame size are not compatible. If true (default), the sound engine will initialize to a multiple of the HW setting, close to the user setting. If false, the user setting is used as is, regardless of the HW preference (might incur a performance hit).")]
		public bool m_RoundFrameSizeToHardwareSize = true;
	}
}
