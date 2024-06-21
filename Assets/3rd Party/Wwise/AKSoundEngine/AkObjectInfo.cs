// dnSpy decompiler from Assembly-CSharp.dll class: AkObjectInfo
using System;

public class AkObjectInfo : IDisposable
{
	internal AkObjectInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkObjectInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkObjectInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkObjectInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkObjectInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkObjectInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public uint objID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkObjectInfo_objID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkObjectInfo_objID_set(this.swigCPtr, value);
		}
	}

	public uint parentID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkObjectInfo_parentID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkObjectInfo_parentID_set(this.swigCPtr, value);
		}
	}

	public int iDepth
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkObjectInfo_iDepth_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkObjectInfo_iDepth_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
