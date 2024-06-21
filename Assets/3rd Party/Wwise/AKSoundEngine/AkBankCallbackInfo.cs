// dnSpy decompiler from Assembly-CSharp.dll class: AkBankCallbackInfo
using System;

public class AkBankCallbackInfo : IDisposable
{
	internal AkBankCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkBankCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkBankCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkBankCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkBankCallbackInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkBankCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public uint bankID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_bankID_get(this.swigCPtr);
		}
	}

	public IntPtr inMemoryBankPtr
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_inMemoryBankPtr_get(this.swigCPtr);
		}
	}

	public AKRESULT loadResult
	{
		get
		{
			return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_loadResult_get(this.swigCPtr);
		}
	}

	public int memPoolId
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkBankCallbackInfo_memPoolId_get(this.swigCPtr);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
