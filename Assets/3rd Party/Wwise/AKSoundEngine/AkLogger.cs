// dnSpy decompiler from Assembly-CSharp.dll class: AkLogger
using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

public class AkLogger
{
	private AkLogger()
	{
		if (AkLogger.ms_Instance == null)
		{
			AkLogger.ms_Instance = this;
			AkSoundEngine.SetErrorLogger(this.errorLoggerDelegate);
		}
	}

	public static AkLogger Instance
	{
		get
		{
			return AkLogger.ms_Instance;
		}
	}

	~AkLogger()
	{
		if (AkLogger.ms_Instance == this)
		{
			AkLogger.ms_Instance = null;
			this.errorLoggerDelegate = null;
			AkSoundEngine.SetErrorLogger();
		}
	}

	public void Init()
	{
	}

	[MonoPInvokeCallback(typeof(AkLogger.ErrorLoggerInteropDelegate))]
	public static void WwiseInternalLogError(string message)
	{
		UnityEngine.Debug.LogError("Wwise: " + message);
	}

	public static void Message(string message)
	{
		UnityEngine.Debug.Log("WwiseUnity: " + message);
	}

	public static void Warning(string message)
	{
		UnityEngine.Debug.LogWarning("WwiseUnity: " + message);
	}

	public static void Error(string message)
	{
		UnityEngine.Debug.LogError("WwiseUnity: " + message);
	}

	private static AkLogger ms_Instance = new AkLogger();

	private AkLogger.ErrorLoggerInteropDelegate errorLoggerDelegate = new AkLogger.ErrorLoggerInteropDelegate(AkLogger.WwiseInternalLogError);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void ErrorLoggerInteropDelegate([MarshalAs(UnmanagedType.LPStr)] string message);
}
