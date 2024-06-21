// dnSpy decompiler from Assembly-CSharp.dll class: AkGlobalCallbackLocation
using System;

public enum AkGlobalCallbackLocation
{
	AkGlobalCallbackLocation_Register = 1,
	AkGlobalCallbackLocation_Begin,
	AkGlobalCallbackLocation_PreProcessMessageQueueForRender = 4,
	AkGlobalCallbackLocation_PostMessagesProcessed = 8,
	AkGlobalCallbackLocation_BeginRender = 16,
	AkGlobalCallbackLocation_EndRender = 32,
	AkGlobalCallbackLocation_End = 64,
	AkGlobalCallbackLocation_Term = 128,
	AkGlobalCallbackLocation_Monitor = 256,
	AkGlobalCallbackLocation_MonitorRecap = 512,
	AkGlobalCallbackLocation_Num = 10
}
