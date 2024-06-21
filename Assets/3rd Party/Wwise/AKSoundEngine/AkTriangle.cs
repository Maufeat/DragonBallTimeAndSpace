// dnSpy decompiler from Assembly-CSharp.dll class: AkTriangle
using System;

public class AkTriangle : IDisposable
{
	internal AkTriangle(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkTriangle() : this(AkSoundEnginePINVOKE.CSharp_new_AkTriangle__SWIG_0(), true)
	{
	}

	public AkTriangle(ushort in_pt0, ushort in_pt1, ushort in_pt2, ushort in_surfaceInfo) : this(AkSoundEnginePINVOKE.CSharp_new_AkTriangle__SWIG_1(in_pt0, in_pt1, in_pt2, in_surfaceInfo), true)
	{
	}

	internal static IntPtr getCPtr(AkTriangle obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkTriangle()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkTriangle(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public ushort point0
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkTriangle_point0_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTriangle_point0_set(this.swigCPtr, value);
		}
	}

	public ushort point1
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkTriangle_point1_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTriangle_point1_set(this.swigCPtr, value);
		}
	}

	public ushort point2
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkTriangle_point2_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTriangle_point2_set(this.swigCPtr, value);
		}
	}

	public ushort surface
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkTriangle_surface_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTriangle_surface_set(this.swigCPtr, value);
		}
	}

	public void Clear()
	{
		AkSoundEnginePINVOKE.CSharp_AkTriangle_Clear(this.swigCPtr);
	}

	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkTriangle_GetSizeOf();
	}

	public void Clone(AkTriangle other)
	{
		AkSoundEnginePINVOKE.CSharp_AkTriangle_Clone(this.swigCPtr, AkTriangle.getCPtr(other));
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
