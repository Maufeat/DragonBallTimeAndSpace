// dnSpy decompiler from Assembly-CSharp.dll class: AkImageSourceParams
using System;

public class AkImageSourceParams : IDisposable
{
	internal AkImageSourceParams(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkImageSourceParams() : this(AkSoundEnginePINVOKE.CSharp_new_AkImageSourceParams__SWIG_0(), true)
	{
	}

	public AkImageSourceParams(AkVector in_sourcePosition, float in_fDistanceScalingFactor, float in_fLevel) : this(AkSoundEnginePINVOKE.CSharp_new_AkImageSourceParams__SWIG_1(AkVector.getCPtr(in_sourcePosition), in_fDistanceScalingFactor, in_fLevel), true)
	{
	}

	internal static IntPtr getCPtr(AkImageSourceParams obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkImageSourceParams()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkImageSourceParams(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkVector sourcePosition
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_sourcePosition_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_sourcePosition_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	public float fDistanceScalingFactor
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_fDistanceScalingFactor_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_fDistanceScalingFactor_set(this.swigCPtr, value);
		}
	}

	public float fLevel
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_fLevel_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_fLevel_set(this.swigCPtr, value);
		}
	}

	public float fDiffraction
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_fDiffraction_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_fDiffraction_set(this.swigCPtr, value);
		}
	}

	public bool bDiffractedEmitterSide
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_bDiffractedEmitterSide_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_bDiffractedEmitterSide_set(this.swigCPtr, value);
		}
	}

	public bool bDiffractedListenerSide
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_bDiffractedListenerSide_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkImageSourceParams_bDiffractedListenerSide_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
