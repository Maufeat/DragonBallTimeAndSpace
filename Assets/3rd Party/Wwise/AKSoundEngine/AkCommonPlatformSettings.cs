// dnSpy decompiler from Assembly-CSharp.dll class: AkCommonPlatformSettings
using System;

public abstract class AkCommonPlatformSettings : AkBasePlatformSettings
{
	protected abstract AkCommonUserSettings GetUserSettings();

	protected abstract AkCommonAdvancedSettings GetAdvancedSettings();

	protected abstract AkCommonCommSettings GetCommsSettings();

	public override AkInitializationSettings AkInitializationSettings
	{
		get
		{
			AkInitializationSettings akInitializationSettings = base.AkInitializationSettings;
			AkCommonUserSettings userSettings = this.GetUserSettings();
			userSettings.CopyTo(akInitializationSettings.memSettings);
			userSettings.CopyTo(akInitializationSettings.deviceSettings);
			userSettings.CopyTo(akInitializationSettings.streamMgrSettings);
			userSettings.CopyTo(akInitializationSettings.initSettings);
			userSettings.CopyTo(akInitializationSettings.platformSettings);
			userSettings.CopyTo(akInitializationSettings.musicSettings);
			akInitializationSettings.preparePoolSize = userSettings.m_PreparePoolSize;
			AkCommonAdvancedSettings advancedSettings = this.GetAdvancedSettings();
			advancedSettings.CopyTo(akInitializationSettings.deviceSettings);
			advancedSettings.CopyTo(akInitializationSettings.initSettings);
			return akInitializationSettings;
		}
	}

	public override AkSpatialAudioInitSettings AkSpatialAudioInitSettings
	{
		get
		{
			AkSpatialAudioInitSettings akSpatialAudioInitSettings = base.AkSpatialAudioInitSettings;
			this.GetUserSettings().CopyTo(akSpatialAudioInitSettings);
			this.GetAdvancedSettings().CopyTo(akSpatialAudioInitSettings);
			return akSpatialAudioInitSettings;
		}
	}

	public override AkCallbackManager.InitializationSettings CallbackManagerInitializationSettings
	{
		get
		{
			AkCommonUserSettings userSettings = this.GetUserSettings();
			return new AkCallbackManager.InitializationSettings
			{
				BufferSize = userSettings.m_CallbackManagerBufferSize,
				IsLoggingEnabled = userSettings.m_EngineLogging
			};
		}
	}

	public override string InitialLanguage
	{
		get
		{
			return this.GetUserSettings().m_StartupLanguage;
		}
	}

	public override string SoundbankPath
	{
		get
		{
			return this.GetUserSettings().m_BasePath;
		}
	}

	public override AkCommunicationSettings AkCommunicationSettings
	{
		get
		{
			AkCommunicationSettings akCommunicationSettings = base.AkCommunicationSettings;
			this.GetCommsSettings().CopyTo(akCommunicationSettings);
			return akCommunicationSettings;
		}
	}
}
