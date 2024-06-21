// dnSpy decompiler from Assembly-CSharp.dll class: AkCallbackManager
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class AkCallbackManager
{
	private static bool IsLoggingEnabled { get; set; }

	public static void RemoveEventCallback(uint in_playingID)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, AkCallbackManager.EventCallbackPackage> keyValuePair in AkCallbackManager.m_mapEventCallbacks)
		{
			if (keyValuePair.Value.m_playingID == in_playingID)
			{
				list.Add(keyValuePair.Key);
				break;
			}
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			AkCallbackManager.m_mapEventCallbacks.Remove(list[i]);
		}
		AkSoundEnginePINVOKE.CSharp_CancelEventCallback(in_playingID);
	}

	public static void RemoveEventCallbackCookie(object in_cookie)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, AkCallbackManager.EventCallbackPackage> keyValuePair in AkCallbackManager.m_mapEventCallbacks)
		{
			if (keyValuePair.Value.m_Cookie == in_cookie)
			{
				list.Add(keyValuePair.Key);
			}
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			int num = list[i];
			AkCallbackManager.m_mapEventCallbacks.Remove(num);
			AkSoundEnginePINVOKE.CSharp_CancelEventCallbackCookie((IntPtr)num);
		}
	}

	public static void RemoveBankCallback(object in_cookie)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, AkCallbackManager.BankCallbackPackage> keyValuePair in AkCallbackManager.m_mapBankCallbacks)
		{
			if (keyValuePair.Value.m_Cookie == in_cookie)
			{
				list.Add(keyValuePair.Key);
			}
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			int num = list[i];
			AkCallbackManager.m_mapBankCallbacks.Remove(num);
			AkSoundEnginePINVOKE.CSharp_CancelBankCallbackCookie((IntPtr)num);
		}
	}

	public static void SetLastAddedPlayingID(uint in_playingID)
	{
		if (AkCallbackManager.m_LastAddedEventPackage != null && AkCallbackManager.m_LastAddedEventPackage.m_playingID == 0u)
		{
			AkCallbackManager.m_LastAddedEventPackage.m_playingID = in_playingID;
		}
	}

	public static AKRESULT Init(AkCallbackManager.InitializationSettings settings)
	{
		AkCallbackManager.IsLoggingEnabled = settings.IsLoggingEnabled;
		AkCallbackManager.m_pNotifMem = ((settings.BufferSize <= 0) ? IntPtr.Zero : Marshal.AllocHGlobal(settings.BufferSize));
		return AkCallbackSerializer.Init(AkCallbackManager.m_pNotifMem, (uint)settings.BufferSize);
	}

	public static void Term()
	{
		if (AkCallbackManager.m_pNotifMem != IntPtr.Zero)
		{
			AkCallbackSerializer.Term();
			Marshal.FreeHGlobal(AkCallbackManager.m_pNotifMem);
			AkCallbackManager.m_pNotifMem = IntPtr.Zero;
		}
	}

	public static void SetMonitoringCallback(AkMonitorErrorLevel in_Level, AkCallbackManager.MonitoringCallback in_CB)
	{
		AkCallbackSerializer.SetLocalOutput((uint)((in_CB == null) ? ((AkMonitorErrorLevel)0) : in_Level));
		AkCallbackManager.m_MonitoringCB = in_CB;
	}

	public static void SetBGMCallback(AkCallbackManager.BGMCallback in_CB, object in_cookie)
	{
		AkCallbackManager.ms_sourceChangeCallbackPkg = new AkCallbackManager.BGMCallbackPackage
		{
			m_Callback = in_CB,
			m_Cookie = in_cookie
		};
	}

	public static int PostCallbacks()
	{
		if (AkCallbackManager.m_pNotifMem == IntPtr.Zero)
		{
			return 0;
		}
		int result;
		try
		{
			int num = 0;
			IntPtr intPtr = AkCallbackSerializer.Lock();
			while (intPtr != IntPtr.Zero)
			{
				IntPtr intPtr2 = AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_pPackage_get(intPtr);
				AkCallbackType akCallbackType = (AkCallbackType)AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_eType_get(intPtr);
				IntPtr cptr = AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_GetData(intPtr);
				AkCallbackType akCallbackType2 = akCallbackType;
				if (akCallbackType2 != AkCallbackType.AK_Monitoring)
				{
					if (akCallbackType2 != AkCallbackType.AK_AudioInterruption)
					{
						if (akCallbackType2 != AkCallbackType.AK_AudioSourceChange)
						{
							if (akCallbackType2 != AkCallbackType.AK_Bank)
							{
								AkCallbackManager.EventCallbackPackage eventCallbackPackage = null;
								if (!AkCallbackManager.m_mapEventCallbacks.TryGetValue((int)intPtr2, out eventCallbackPackage))
								{
									UnityEngine.Debug.LogError("WwiseUnity: EventCallbackPackage not found for <" + intPtr2 + ">.");
									return num;
								}
								AkCallbackInfo akCallbackInfo = null;
								AkCallbackType akCallbackType3 = akCallbackType;
								switch (akCallbackType3)
								{
								case AkCallbackType.AK_EndOfEvent:
									AkCallbackManager.m_mapEventCallbacks.Remove(eventCallbackPackage.GetHashCode());
									if (eventCallbackPackage.m_bNotifyEndOfEvent)
									{
										AkCallbackManager.AkEventCallbackInfo.setCPtr(cptr);
										akCallbackInfo = AkCallbackManager.AkEventCallbackInfo;
									}
									break;
								case AkCallbackType.AK_EndOfDynamicSequenceItem:
									AkCallbackManager.AkDynamicSequenceItemCallbackInfo.setCPtr(cptr);
									akCallbackInfo = AkCallbackManager.AkDynamicSequenceItemCallbackInfo;
									break;
								default:
									if (akCallbackType3 != AkCallbackType.AK_MusicPlaylistSelect)
									{
										if (akCallbackType3 != AkCallbackType.AK_MusicPlayStarted)
										{
											if (akCallbackType3 != AkCallbackType.AK_MusicSyncBeat && akCallbackType3 != AkCallbackType.AK_MusicSyncBar && akCallbackType3 != AkCallbackType.AK_MusicSyncEntry && akCallbackType3 != AkCallbackType.AK_MusicSyncExit && akCallbackType3 != AkCallbackType.AK_MusicSyncGrid && akCallbackType3 != AkCallbackType.AK_MusicSyncUserCue && akCallbackType3 != AkCallbackType.AK_MusicSyncPoint)
											{
												if (akCallbackType3 != AkCallbackType.AK_MIDIEvent)
												{
													UnityEngine.Debug.LogError("WwiseUnity: PostCallbacks aborted due to error: Undefined callback type <" + akCallbackType + "> found. Callback object possibly corrupted.");
													return num;
												}
												AkCallbackManager.AkMIDIEventCallbackInfo.setCPtr(cptr);
												akCallbackInfo = AkCallbackManager.AkMIDIEventCallbackInfo;
											}
											else
											{
												AkCallbackManager.AkMusicSyncCallbackInfo.setCPtr(cptr);
												akCallbackInfo = AkCallbackManager.AkMusicSyncCallbackInfo;
											}
										}
										else
										{
											AkCallbackManager.AkEventCallbackInfo.setCPtr(cptr);
											akCallbackInfo = AkCallbackManager.AkEventCallbackInfo;
										}
									}
									else
									{
										AkCallbackManager.AkMusicPlaylistCallbackInfo.setCPtr(cptr);
										akCallbackInfo = AkCallbackManager.AkMusicPlaylistCallbackInfo;
									}
									break;
								case AkCallbackType.AK_Marker:
									AkCallbackManager.AkMarkerCallbackInfo.setCPtr(cptr);
									akCallbackInfo = AkCallbackManager.AkMarkerCallbackInfo;
									break;
								case AkCallbackType.AK_Duration:
									AkCallbackManager.AkDurationCallbackInfo.setCPtr(cptr);
									akCallbackInfo = AkCallbackManager.AkDurationCallbackInfo;
									break;
								}
								if (akCallbackInfo != null)
								{
									eventCallbackPackage.m_Callback(eventCallbackPackage.m_Cookie, akCallbackType, akCallbackInfo);
								}
							}
							else
							{
								AkCallbackManager.BankCallbackPackage bankCallbackPackage = null;
								if (!AkCallbackManager.m_mapBankCallbacks.TryGetValue((int)intPtr2, out bankCallbackPackage))
								{
									UnityEngine.Debug.LogError("WwiseUnity: BankCallbackPackage not found for <" + intPtr2 + ">.");
									return num;
								}
								AkCallbackManager.m_mapBankCallbacks.Remove((int)intPtr2);
								if (bankCallbackPackage != null && bankCallbackPackage.m_Callback != null)
								{
									AkCallbackManager.AkBankCallbackInfo.setCPtr(cptr);
									bankCallbackPackage.m_Callback(AkCallbackManager.AkBankCallbackInfo.bankID, AkCallbackManager.AkBankCallbackInfo.inMemoryBankPtr, AkCallbackManager.AkBankCallbackInfo.loadResult, (uint)AkCallbackManager.AkBankCallbackInfo.memPoolId, bankCallbackPackage.m_Cookie);
								}
							}
						}
						else if (AkCallbackManager.ms_sourceChangeCallbackPkg != null && AkCallbackManager.ms_sourceChangeCallbackPkg.m_Callback != null)
						{
							AkCallbackManager.AkAudioSourceChangeCallbackInfo.setCPtr(cptr);
							AkCallbackManager.ms_sourceChangeCallbackPkg.m_Callback(AkCallbackManager.AkAudioSourceChangeCallbackInfo.bOtherAudioPlaying, AkCallbackManager.ms_sourceChangeCallbackPkg.m_Cookie);
						}
					}
				}
				else if (AkCallbackManager.m_MonitoringCB != null)
				{
					AkCallbackManager.AkMonitoringCallbackInfo.setCPtr(cptr);
					AkCallbackManager.m_MonitoringCB(AkCallbackManager.AkMonitoringCallbackInfo.errorCode, AkCallbackManager.AkMonitoringCallbackInfo.errorLevel, AkCallbackManager.AkMonitoringCallbackInfo.playingID, AkCallbackManager.AkMonitoringCallbackInfo.gameObjID, AkCallbackManager.AkMonitoringCallbackInfo.message);
				}
				intPtr = AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_pNext_get(intPtr);
				num++;
			}
			result = num;
		}
		finally
		{
			AkCallbackSerializer.Unlock();
		}
		return result;
	}

	private static readonly AkEventCallbackInfo AkEventCallbackInfo = new AkEventCallbackInfo(IntPtr.Zero, false);

	private static readonly AkDynamicSequenceItemCallbackInfo AkDynamicSequenceItemCallbackInfo = new AkDynamicSequenceItemCallbackInfo(IntPtr.Zero, false);

	private static readonly AkMIDIEventCallbackInfo AkMIDIEventCallbackInfo = new AkMIDIEventCallbackInfo(IntPtr.Zero, false);

	private static readonly AkMarkerCallbackInfo AkMarkerCallbackInfo = new AkMarkerCallbackInfo(IntPtr.Zero, false);

	private static readonly AkDurationCallbackInfo AkDurationCallbackInfo = new AkDurationCallbackInfo(IntPtr.Zero, false);

	private static readonly AkMusicSyncCallbackInfo AkMusicSyncCallbackInfo = new AkMusicSyncCallbackInfo(IntPtr.Zero, false);

	private static readonly AkMusicPlaylistCallbackInfo AkMusicPlaylistCallbackInfo = new AkMusicPlaylistCallbackInfo(IntPtr.Zero, false);

	private static readonly AkAudioSourceChangeCallbackInfo AkAudioSourceChangeCallbackInfo = new AkAudioSourceChangeCallbackInfo(IntPtr.Zero, false);

	private static readonly AkMonitoringCallbackInfo AkMonitoringCallbackInfo = new AkMonitoringCallbackInfo(IntPtr.Zero, false);

	private static readonly AkBankCallbackInfo AkBankCallbackInfo = new AkBankCallbackInfo(IntPtr.Zero, false);

	private static readonly Dictionary<int, AkCallbackManager.EventCallbackPackage> m_mapEventCallbacks = new Dictionary<int, AkCallbackManager.EventCallbackPackage>();

	private static readonly Dictionary<int, AkCallbackManager.BankCallbackPackage> m_mapBankCallbacks = new Dictionary<int, AkCallbackManager.BankCallbackPackage>();

	private static AkCallbackManager.EventCallbackPackage m_LastAddedEventPackage;

	private static IntPtr m_pNotifMem = IntPtr.Zero;

	private static AkCallbackManager.MonitoringCallback m_MonitoringCB;

	private static AkCallbackManager.BGMCallbackPackage ms_sourceChangeCallbackPkg;

	public class EventCallbackPackage
	{
		public static AkCallbackManager.EventCallbackPackage Create(AkCallbackManager.EventCallback in_cb, object in_cookie, ref uint io_Flags)
		{
			if (io_Flags == 0u || in_cb == null)
			{
				io_Flags = 0u;
				return null;
			}
			AkCallbackManager.EventCallbackPackage eventCallbackPackage = new AkCallbackManager.EventCallbackPackage();
			eventCallbackPackage.m_Callback = in_cb;
			eventCallbackPackage.m_Cookie = in_cookie;
			eventCallbackPackage.m_bNotifyEndOfEvent = ((io_Flags & 1u) != 0u);
			io_Flags |= 1u;
			AkCallbackManager.m_mapEventCallbacks[eventCallbackPackage.GetHashCode()] = eventCallbackPackage;
			AkCallbackManager.m_LastAddedEventPackage = eventCallbackPackage;
			return eventCallbackPackage;
		}

		~EventCallbackPackage()
		{
			if (this.m_Cookie != null)
			{
				AkCallbackManager.RemoveEventCallbackCookie(this.m_Cookie);
			}
		}

		public bool m_bNotifyEndOfEvent;

		public AkCallbackManager.EventCallback m_Callback;

		public object m_Cookie;

		public uint m_playingID;
	}

	public class BankCallbackPackage
	{
		public BankCallbackPackage(AkCallbackManager.BankCallback in_cb, object in_cookie)
		{
			this.m_Callback = in_cb;
			this.m_Cookie = in_cookie;
			AkCallbackManager.m_mapBankCallbacks[this.GetHashCode()] = this;
		}

		public AkCallbackManager.BankCallback m_Callback;

		public object m_Cookie;
	}

	public class BGMCallbackPackage
	{
		public AkCallbackManager.BGMCallback m_Callback;

		public object m_Cookie;
	}

	public class InitializationSettings
	{
		public static int DefaultBufferSize = 4096;

		public static bool DefaultIsLoggingEnabled = true;

		public int BufferSize = AkCallbackManager.InitializationSettings.DefaultBufferSize;

		public bool IsLoggingEnabled = AkCallbackManager.InitializationSettings.DefaultIsLoggingEnabled;
	}

	public delegate void EventCallback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info);

	public delegate void MonitoringCallback(AkMonitorErrorCode in_errorCode, AkMonitorErrorLevel in_errorLevel, uint in_playingID, ulong in_gameObjID, string in_msg);

	public delegate void BankCallback(uint in_bankID, IntPtr in_InMemoryBankPtr, AKRESULT in_eLoadResult, uint in_memPoolId, object in_Cookie);

	public delegate AKRESULT BGMCallback(bool in_bOtherAudioPlaying, object in_Cookie);
}
