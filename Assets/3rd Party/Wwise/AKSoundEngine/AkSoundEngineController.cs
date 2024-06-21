// dnSpy decompiler from Assembly-CSharp.dll class: AkSoundEngineController
using System;
using System.IO;
using UnityEngine;

public class AkSoundEngineController
{
	private AkSoundEngineController()
	{
	}

	public static AkSoundEngineController Instance
	{
		get
		{
			if (AkSoundEngineController.ms_Instance == null)
			{
				AkSoundEngineController.ms_Instance = new AkSoundEngineController();
			}
			return AkSoundEngineController.ms_Instance;
		}
	}

	~AkSoundEngineController()
	{
		if (AkSoundEngineController.ms_Instance == this)
		{
			AkSoundEngineController.ms_Instance = null;
		}
	}

	public static string GetDecodedBankFolder()
	{
		return "DecodedBanks";
	}

	public static string GetDecodedBankFullPath()
	{
		return Path.Combine(AkBasePathGetter.GetPlatformBasePath(), AkSoundEngineController.GetDecodedBankFolder());
	}

	public void LateUpdate()
	{
		AkCallbackManager.PostCallbacks();
		AkBankManager.DoUnloadBanks();
		AkSoundEngine.RenderAudio();
	}

	public void Init(AkInitializer akInitializer)
	{
		if (akInitializer == null)
		{
			UnityEngine.Debug.LogError("WwiseUnity: AkInitializer must not be null. Sound engine will not be initialized.");
			return;
		}
		bool flag = AkSoundEngine.IsInitialized();
		AkLogger.Instance.Init();
		if (flag)
		{
			UnityEngine.Debug.LogError("WwiseUnity: Sound engine is already initialized.");
			return;
		}
		if (!AkWwiseInitializationSettings.InitializeSoundEngine())
		{
			return;
		}
	}

	public void OnDisable()
	{
	}

	public void Terminate()
	{
		AkWwiseInitializationSettings.TerminateSoundEngine();
	}

	public void OnApplicationPause(bool pauseStatus)
	{
		this.ActivateAudio(!pauseStatus);
	}

	public void OnApplicationFocus(bool focus)
	{
		this.ActivateAudio(focus);
	}

	private void ActivateAudio(bool activate)
	{
		if (AkSoundEngine.IsInitialized())
		{
			if (activate)
			{
				AkSoundEngine.WakeupFromSuspend();
			}
			else
			{
				AkSoundEngine.Suspend();
			}
			AkSoundEngine.RenderAudio();
		}
	}

	private static AkSoundEngineController ms_Instance;
}
