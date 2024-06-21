// dnSpy decompiler from Assembly-CSharp.dll class: AkVector
using System;

public class AkVector : IDisposable
{
	internal AkVector(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkVector() : this(AkSoundEnginePINVOKE.CSharp_new_AkVector(), true)
	{
	}

	internal static IntPtr getCPtr(AkVector obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkVector()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkVector(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public void Zero()
	{
		AkSoundEnginePINVOKE.CSharp_AkVector_Zero(this.swigCPtr);
	}

	public float X
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkVector_X_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkVector_X_set(this.swigCPtr, value);
		}
	}

	public float Y
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkVector_Y_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkVector_Y_set(this.swigCPtr, value);
		}
	}

	public float Z
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkVector_Z_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkVector_Z_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
