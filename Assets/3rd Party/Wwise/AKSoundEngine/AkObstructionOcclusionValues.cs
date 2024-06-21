// dnSpy decompiler from Assembly-CSharp.dll class: AkObstructionOcclusionValues
using System;

public class AkObstructionOcclusionValues : IDisposable
{
	internal AkObstructionOcclusionValues(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkObstructionOcclusionValues() : this(AkSoundEnginePINVOKE.CSharp_new_AkObstructionOcclusionValues(), true)
	{
	}

	internal static IntPtr getCPtr(AkObstructionOcclusionValues obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkObstructionOcclusionValues()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkObstructionOcclusionValues(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public float occlusion
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkObstructionOcclusionValues_occlusion_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkObstructionOcclusionValues_occlusion_set(this.swigCPtr, value);
		}
	}

	public float obstruction
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkObstructionOcclusionValues_obstruction_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkObstructionOcclusionValues_obstruction_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
