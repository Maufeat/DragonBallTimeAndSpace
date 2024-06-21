// dnSpy decompiler from Assembly-CSharp.dll class: AkVertex
using System;

public class AkVertex : IDisposable
{
	internal AkVertex(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkVertex() : this(AkSoundEnginePINVOKE.CSharp_new_AkVertex__SWIG_0(), true)
	{
	}

	public AkVertex(float in_X, float in_Y, float in_Z) : this(AkSoundEnginePINVOKE.CSharp_new_AkVertex__SWIG_1(in_X, in_Y, in_Z), true)
	{
	}

	internal static IntPtr getCPtr(AkVertex obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkVertex()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkVertex(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public float X
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkVertex_X_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkVertex_X_set(this.swigCPtr, value);
		}
	}

	public float Y
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkVertex_Y_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkVertex_Y_set(this.swigCPtr, value);
		}
	}

	public float Z
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkVertex_Z_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkVertex_Z_set(this.swigCPtr, value);
		}
	}

	public void Clear()
	{
		AkSoundEnginePINVOKE.CSharp_AkVertex_Clear(this.swigCPtr);
	}

	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkVertex_GetSizeOf();
	}

	public void Clone(AkVertex other)
	{
		AkSoundEnginePINVOKE.CSharp_AkVertex_Clone(this.swigCPtr, AkVertex.getCPtr(other));
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
