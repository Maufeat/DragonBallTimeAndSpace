// dnSpy decompiler from Assembly-CSharp.dll class: AkWindowsSettings
using System;
using UnityEngine;

public class AkWindowsSettings : AkWwiseInitializationSettings.PlatformSettings
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
	public AkWindowsSettings.PlatformAdvancedSettings AdvancedSettings;

	[HideInInspector]
	public AkCommonCommSettings CommsSettings;

	[Serializable]
	public class PlatformAdvancedSettings : AkCommonAdvancedSettings
	{
		public void CopyTo(AkPlatformInitSettings settings)
		{
			settings.eAudioAPI = (AkAudioAPI)this.m_AudioAPI;
			settings.bGlobalFocus = this.m_GlobalFocus;
		}

		[Tooltip("Main audio API to use. Leave set to \"Default\" for the default audio sink. This value will be ignored if a valid \"AudioDeviceShareset\" is provided.")]
		[AkEnumFlag(typeof(AkWindowsSettings.PlatformAdvancedSettings.AudioAPI))]
		public AkWindowsSettings.PlatformAdvancedSettings.AudioAPI m_AudioAPI = AkWindowsSettings.PlatformAdvancedSettings.AudioAPI.Default;

		[Tooltip("Only used when \"AudioAPI\" is \"DirectSound\", sounds will be muted if set to false when the game loses the focus.")]
		public bool m_GlobalFocus = true;

		public enum AudioAPI
		{
			None,
			Wasapi,
			XAudio2,
			DirectSound = 4,
			Default = -1
		}
	}
}
