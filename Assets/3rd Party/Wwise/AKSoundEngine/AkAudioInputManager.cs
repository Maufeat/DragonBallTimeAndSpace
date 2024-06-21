// dnSpy decompiler from Assembly-CSharp.dll class: AkAudioInputManager
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AK.Wwise;
using AOT;
using UnityEngine;

public static class AkAudioInputManager
{
	public static uint PostAudioInputEvent(AK.Wwise.Event akEvent, GameObject gameObject, AkAudioInputManager.AudioSamplesDelegate sampleDelegate, AkAudioInputManager.AudioFormatDelegate formatDelegate = null)
	{
		AkAudioInputManager.TryInitialize();
		uint num = akEvent.Post(gameObject, 1u, new AkCallbackManager.EventCallback(AkAudioInputManager.EventCallback), null);
		AkAudioInputManager.AddPlayingID(num, sampleDelegate, formatDelegate);
		return num;
	}

	public static uint PostAudioInputEvent(uint akEventID, GameObject gameObject, AkAudioInputManager.AudioSamplesDelegate sampleDelegate, AkAudioInputManager.AudioFormatDelegate formatDelegate = null)
	{
		AkAudioInputManager.TryInitialize();
		uint num = AkSoundEngine.PostEvent(akEventID, gameObject, 1u, new AkCallbackManager.EventCallback(AkAudioInputManager.EventCallback), null);
		AkAudioInputManager.AddPlayingID(num, sampleDelegate, formatDelegate);
		return num;
	}

	public static uint PostAudioInputEvent(string akEventName, GameObject gameObject, AkAudioInputManager.AudioSamplesDelegate sampleDelegate, AkAudioInputManager.AudioFormatDelegate formatDelegate = null)
	{
		AkAudioInputManager.TryInitialize();
		uint num = AkSoundEngine.PostEvent(akEventName, gameObject, 1u, new AkCallbackManager.EventCallback(AkAudioInputManager.EventCallback), null);
		AkAudioInputManager.AddPlayingID(num, sampleDelegate, formatDelegate);
		return num;
	}

	[MonoPInvokeCallback(typeof(AkAudioInputManager.AudioSamplesInteropDelegate))]
	private static bool InternalAudioSamplesDelegate(uint playingID, float[] samples, uint channelIndex, uint frames)
	{
		return AkAudioInputManager.audioSamplesDelegates.ContainsKey(playingID) && AkAudioInputManager.audioSamplesDelegates[playingID](playingID, channelIndex, samples);
	}

	[MonoPInvokeCallback(typeof(AkAudioInputManager.AudioFormatInteropDelegate))]
	private static void InternalAudioFormatDelegate(uint playingID, IntPtr format)
	{
		if (AkAudioInputManager.audioFormatDelegates.ContainsKey(playingID))
		{
			AkAudioInputManager.audioFormat.setCPtr(format);
			AkAudioInputManager.audioFormatDelegates[playingID](playingID, AkAudioInputManager.audioFormat);
		}
	}

	private static void TryInitialize()
	{
		if (!AkAudioInputManager.initialized)
		{
			AkAudioInputManager.initialized = true;
			AkSoundEngine.SetAudioInputCallbacks(AkAudioInputManager.audioSamplesDelegate, AkAudioInputManager.audioFormatDelegate);
		}
	}

	private static void AddPlayingID(uint playingID, AkAudioInputManager.AudioSamplesDelegate sampleDelegate, AkAudioInputManager.AudioFormatDelegate formatDelegate)
	{
		if (playingID == 0u || sampleDelegate == null)
		{
			return;
		}
		AkAudioInputManager.audioSamplesDelegates.Add(playingID, sampleDelegate);
		if (formatDelegate != null)
		{
			AkAudioInputManager.audioFormatDelegates.Add(playingID, formatDelegate);
		}
	}

	private static void EventCallback(object cookie, AkCallbackType type, AkCallbackInfo callbackInfo)
	{
		if (type == AkCallbackType.AK_EndOfEvent)
		{
			AkEventCallbackInfo akEventCallbackInfo = callbackInfo as AkEventCallbackInfo;
			if (akEventCallbackInfo != null)
			{
				AkAudioInputManager.audioSamplesDelegates.Remove(akEventCallbackInfo.playingID);
				AkAudioInputManager.audioFormatDelegates.Remove(akEventCallbackInfo.playingID);
			}
		}
	}

	private static bool initialized;

	private static readonly Dictionary<uint, AkAudioInputManager.AudioSamplesDelegate> audioSamplesDelegates = new Dictionary<uint, AkAudioInputManager.AudioSamplesDelegate>();

	private static readonly Dictionary<uint, AkAudioInputManager.AudioFormatDelegate> audioFormatDelegates = new Dictionary<uint, AkAudioInputManager.AudioFormatDelegate>();

	private static readonly AkAudioFormat audioFormat = new AkAudioFormat();

	private static readonly AkAudioInputManager.AudioSamplesInteropDelegate audioSamplesDelegate = new AkAudioInputManager.AudioSamplesInteropDelegate(AkAudioInputManager.InternalAudioSamplesDelegate);

	private static readonly AkAudioInputManager.AudioFormatInteropDelegate audioFormatDelegate = new AkAudioInputManager.AudioFormatInteropDelegate(AkAudioInputManager.InternalAudioFormatDelegate);

	public delegate void AudioFormatDelegate(uint playingID, AkAudioFormat format);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void AudioFormatInteropDelegate(uint playingID, IntPtr format);

	public delegate bool AudioSamplesDelegate(uint playingID, uint channelIndex, float[] samples);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate bool AudioSamplesInteropDelegate(uint playingID, [MarshalAs(UnmanagedType.LPArray, SizeConst = 0, SizeParamIndex = 3)] [In] [Out] float[] samples, uint channelIndex, uint frames);
}
