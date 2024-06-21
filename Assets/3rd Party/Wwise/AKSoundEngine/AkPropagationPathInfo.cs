// dnSpy decompiler from Assembly-CSharp.dll class: AkPropagationPathInfo
using System;

public class AkPropagationPathInfo : IDisposable
{
	internal AkPropagationPathInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkPropagationPathInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkPropagationPathInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkPropagationPathInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkPropagationPathInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkPropagationPathInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkVector nodePoint
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_nodePoint_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_nodePoint_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	public uint numNodes
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_numNodes_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_numNodes_set(this.swigCPtr, value);
		}
	}

	public float length
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_length_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_length_set(this.swigCPtr, value);
		}
	}

	public float gain
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_gain_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_gain_set(this.swigCPtr, value);
		}
	}

	public float dryDiffraction
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_dryDiffraction_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_dryDiffraction_set(this.swigCPtr, value);
		}
	}

	public float wetDiffraction
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_wetDiffraction_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_wetDiffraction_set(this.swigCPtr, value);
		}
	}

	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_GetSizeOf();
	}

	public AkVector GetNodePoint(uint idx)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_GetNodePoint(this.swigCPtr, idx);
		return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
	}

	public void Clone(AkPropagationPathInfo other)
	{
		AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_Clone(this.swigCPtr, AkPropagationPathInfo.getCPtr(other));
	}

	public const uint kMaxNodes = 8u;

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
