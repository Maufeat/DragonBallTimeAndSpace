// dnSpy decompiler from Assembly-CSharp.dll class: AkCallbackInfo
using System;

public class AkCallbackInfo : IDisposable
{
	internal AkCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkCallbackInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public IntPtr pCookie
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkCallbackInfo_pCookie_get(this.swigCPtr);
		}
	}

	public ulong gameObjID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkCallbackInfo_gameObjID_get(this.swigCPtr);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
