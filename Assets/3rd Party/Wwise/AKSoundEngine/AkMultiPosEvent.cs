// dnSpy decompiler from Assembly-CSharp.dll class: AkMultiPosEvent
using System;
using System.Collections.Generic;

public class AkMultiPosEvent
{
	public void FinishedPlaying(object in_cookie, AkCallbackType in_type, object in_info)
	{
		this.eventIsPlaying = false;
	}

	public bool eventIsPlaying;

	public List<AkAmbient> list = new List<AkAmbient>();
}
