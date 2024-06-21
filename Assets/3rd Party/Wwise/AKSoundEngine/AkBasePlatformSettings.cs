// dnSpy decompiler from Assembly-CSharp.dll class: AkBasePlatformSettings
using System;
using UnityEngine;

public class AkBasePlatformSettings : ScriptableObject
{
	public virtual AkInitializationSettings AkInitializationSettings
	{
		get
		{
			return new AkInitializationSettings();
		}
	}

	public virtual AkSpatialAudioInitSettings AkSpatialAudioInitSettings
	{
		get
		{
			return new AkSpatialAudioInitSettings();
		}
	}

	public virtual AkCallbackManager.InitializationSettings CallbackManagerInitializationSettings
	{
		get
		{
			return new AkCallbackManager.InitializationSettings();
		}
	}

	public virtual string InitialLanguage
	{
		get
		{
			return "English(US)";
		}
	}

	public virtual string SoundbankPath
	{
		get
		{
			return AkBasePathGetter.DefaultBasePath;
		}
	}

	public virtual AkCommunicationSettings AkCommunicationSettings
	{
		get
		{
			return new AkCommunicationSettings();
		}
	}
}
