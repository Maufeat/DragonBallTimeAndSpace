// dnSpy decompiler from Assembly-CSharp.dll class: AkSegmentInfo
using System;

public class AkSegmentInfo : IDisposable
{
	internal AkSegmentInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkSegmentInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkSegmentInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkSegmentInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkSegmentInfo()
	{
		this.Dispose();
	}

	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkSegmentInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public int iCurrentPosition
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iCurrentPosition_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iCurrentPosition_set(this.swigCPtr, value);
		}
	}

	public int iPreEntryDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iPreEntryDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iPreEntryDuration_set(this.swigCPtr, value);
		}
	}

	public int iActiveDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iActiveDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iActiveDuration_set(this.swigCPtr, value);
		}
	}

	public int iPostExitDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iPostExitDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iPostExitDuration_set(this.swigCPtr, value);
		}
	}

	public int iRemainingLookAheadTime
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iRemainingLookAheadTime_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_iRemainingLookAheadTime_set(this.swigCPtr, value);
		}
	}

	public float fBeatDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fBeatDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fBeatDuration_set(this.swigCPtr, value);
		}
	}

	public float fBarDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fBarDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fBarDuration_set(this.swigCPtr, value);
		}
	}

	public float fGridDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fGridDuration_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fGridDuration_set(this.swigCPtr, value);
		}
	}

	public float fGridOffset
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fGridOffset_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSegmentInfo_fGridOffset_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
