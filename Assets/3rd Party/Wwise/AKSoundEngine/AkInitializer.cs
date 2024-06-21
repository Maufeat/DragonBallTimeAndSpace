// dnSpy decompiler from Assembly-CSharp.dll class: AkInitializer
using System;
using UnityEngine;

[AddComponentMenu("Wwise/AkInitializer")]
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class AkInitializer : MonoBehaviour
{
	private void Awake()
	{
		if (AkInitializer.ms_Instance)
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		AkInitializer.ms_Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		this.InitializationSettings = AkWwiseInitializationSettings.Instance;
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.Init(this);
		}
	}

	private void OnDisable()
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.OnDisable();
		}
	}

	private void OnDestroy()
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkInitializer.ms_Instance = null;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.OnApplicationPause(pauseStatus);
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.OnApplicationFocus(focus);
		}
	}

	private void OnApplicationQuit()
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.Terminate();
		}
	}

	private void LateUpdate()
	{
		if (AkInitializer.ms_Instance == this)
		{
			AkSoundEngineController.Instance.LateUpdate();
		}
	}

	private static AkInitializer ms_Instance;

	public AkWwiseInitializationSettings InitializationSettings;
}
