// dnSpy decompiler from Assembly-CSharp.dll class: AkMusicSyncCallbackInfo
using System;

public class AkMusicSyncCallbackInfo : AkCallbackInfo
{
	internal AkMusicSyncCallbackInfo(IntPtr cPtr, bool cMemoryOwn) : base(AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	public AkMusicSyncCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkMusicSyncCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkMusicSyncCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	~AkMusicSyncCallbackInfo()
	{
		this.Dispose();
	}

	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMusicSyncCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	public uint playingID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_playingID_get(this.swigCPtr);
		}
	}

	public int segmentInfo_iCurrentPosition
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_iCurrentPosition_get(this.swigCPtr);
		}
	}

	public int segmentInfo_iPreEntryDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_iPreEntryDuration_get(this.swigCPtr);
		}
	}

	public int segmentInfo_iActiveDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_iActiveDuration_get(this.swigCPtr);
		}
	}

	public int segmentInfo_iPostExitDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_iPostExitDuration_get(this.swigCPtr);
		}
	}

	public int segmentInfo_iRemainingLookAheadTime
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_iRemainingLookAheadTime_get(this.swigCPtr);
		}
	}

	public float segmentInfo_fBeatDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_fBeatDuration_get(this.swigCPtr);
		}
	}

	public float segmentInfo_fBarDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_fBarDuration_get(this.swigCPtr);
		}
	}

	public float segmentInfo_fGridDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_fGridDuration_get(this.swigCPtr);
		}
	}

	public float segmentInfo_fGridOffset
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_segmentInfo_fGridOffset_get(this.swigCPtr);
		}
	}

	public AkCallbackType musicSyncType
	{
		get
		{
			return (AkCallbackType)AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_musicSyncType_get(this.swigCPtr);
		}
	}

	public string userCueName
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrString(AkSoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_userCueName_get(this.swigCPtr));
		}
	}

	private IntPtr swigCPtr;
}
