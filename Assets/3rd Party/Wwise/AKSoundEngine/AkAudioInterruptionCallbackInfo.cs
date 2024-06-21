// dnSpy decompiler from Assembly-CSharp.dll class: AkAudioInterruptionCallbackInfo
using System;

public class AkAudioInterruptionCallbackInfo : IDisposable
{
	internal AkAudioInterruptionCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkAudioInterruptionCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkAudioInterruptionCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkAudioInterruptionCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkAudioInterruptionCallbackInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkAudioInterruptionCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public bool bEnterInterruption
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioInterruptionCallbackInfo_bEnterInterruption_get(this.swigCPtr);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
