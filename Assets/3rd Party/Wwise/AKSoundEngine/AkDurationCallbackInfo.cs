// dnSpy decompiler from Assembly-CSharp.dll class: AkDurationCallbackInfo
using System;

public class AkDurationCallbackInfo : AkEventCallbackInfo
{
	internal AkDurationCallbackInfo(IntPtr cPtr, bool cMemoryOwn) : base(AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	public AkDurationCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkDurationCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkDurationCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	~AkDurationCallbackInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkDurationCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	public float fDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_fDuration_get(this.swigCPtr);
		}
	}

	public float fEstimatedDuration
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_fEstimatedDuration_get(this.swigCPtr);
		}
	}

	public uint audioNodeID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_audioNodeID_get(this.swigCPtr);
		}
	}

	public uint mediaID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_mediaID_get(this.swigCPtr);
		}
	}

	public bool bStreaming
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDurationCallbackInfo_bStreaming_get(this.swigCPtr);
		}
	}

	private IntPtr swigCPtr;
}
