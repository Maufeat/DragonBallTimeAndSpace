// dnSpy decompiler from Assembly-CSharp.dll class: AkAudioSourceChangeCallbackInfo
using System;

public class AkAudioSourceChangeCallbackInfo : IDisposable
{
	internal AkAudioSourceChangeCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkAudioSourceChangeCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkAudioSourceChangeCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkAudioSourceChangeCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkAudioSourceChangeCallbackInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkAudioSourceChangeCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public bool bOtherAudioPlaying
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioSourceChangeCallbackInfo_bOtherAudioPlaying_get(this.swigCPtr);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
