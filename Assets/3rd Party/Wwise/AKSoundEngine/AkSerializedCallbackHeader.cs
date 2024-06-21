// dnSpy decompiler from Assembly-CSharp.dll class: AkSerializedCallbackHeader
using System;

public class AkSerializedCallbackHeader : IDisposable
{
	internal AkSerializedCallbackHeader(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkSerializedCallbackHeader() : this(AkSoundEnginePINVOKE.CSharp_new_AkSerializedCallbackHeader(), true)
	{
	}

	internal static IntPtr getCPtr(AkSerializedCallbackHeader obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkSerializedCallbackHeader()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkSerializedCallbackHeader(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public IntPtr pPackage
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_pPackage_get(this.swigCPtr);
		}
	}

	public AkSerializedCallbackHeader pNext
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_pNext_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkSerializedCallbackHeader(intPtr, false) : null;
		}
	}

	public AkCallbackType eType
	{
		get
		{
			return (AkCallbackType)AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_eType_get(this.swigCPtr);
		}
	}

	public IntPtr GetData()
	{
		return AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_GetData(this.swigCPtr);
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
