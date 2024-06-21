// dnSpy decompiler from Assembly-CSharp.dll class: AkDiffractionPathInfo
using System;

public class AkDiffractionPathInfo : IDisposable
{
	internal AkDiffractionPathInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkDiffractionPathInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkDiffractionPathInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkDiffractionPathInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkDiffractionPathInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkDiffractionPathInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkTransform virtualPos
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_virtualPos_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkTransform(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_virtualPos_set(this.swigCPtr, AkTransform.getCPtr(value));
		}
	}

	public uint nodeCount
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_nodeCount_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_nodeCount_set(this.swigCPtr, value);
		}
	}

	public float diffraction
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_diffraction_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_diffraction_set(this.swigCPtr, value);
		}
	}

	public float totLength
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_totLength_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_totLength_set(this.swigCPtr, value);
		}
	}

	public float obstructionValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_obstructionValue_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_obstructionValue_set(this.swigCPtr, value);
		}
	}

	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_GetSizeOf();
	}

	public AkVector GetNodes(uint idx)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_GetNodes(this.swigCPtr, idx);
		return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
	}

	public float GetAngles(uint idx)
	{
		return AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_GetAngles(this.swigCPtr, idx);
	}

	public void Clone(AkDiffractionPathInfo other)
	{
		AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_Clone(this.swigCPtr, AkDiffractionPathInfo.getCPtr(other));
	}

	public const uint kMaxNodes = 8u;

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
