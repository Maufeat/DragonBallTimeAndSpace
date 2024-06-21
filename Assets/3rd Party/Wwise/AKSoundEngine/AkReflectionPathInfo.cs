// dnSpy decompiler from Assembly-CSharp.dll class: AkReflectionPathInfo
using System;

public class AkReflectionPathInfo : IDisposable
{
	internal AkReflectionPathInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkReflectionPathInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkReflectionPathInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkReflectionPathInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkReflectionPathInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkReflectionPathInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkVector imageSource
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_imageSource_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_imageSource_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	public uint numPathPoints
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_numPathPoints_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_numPathPoints_set(this.swigCPtr, value);
		}
	}

	public uint numReflections
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_numReflections_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_numReflections_set(this.swigCPtr, value);
		}
	}

	public AkVector occlusionPoint
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_occlusionPoint_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_occlusionPoint_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	public float level
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_level_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_level_set(this.swigCPtr, value);
		}
	}

	public bool isOccluded
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_isOccluded_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_isOccluded_set(this.swigCPtr, value);
		}
	}

	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_GetSizeOf();
	}

	public AkVector GetPathPoint(uint idx)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_GetPathPoint(this.swigCPtr, idx);
		return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
	}

	public AkAcousticSurface GetAcousticSurface(uint idx)
	{
		return new AkAcousticSurface(AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_GetAcousticSurface(this.swigCPtr, idx), false);
	}

	public float GetDiffraction(uint idx)
	{
		return AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_GetDiffraction(this.swigCPtr, idx);
	}

	public void Clone(AkReflectionPathInfo other)
	{
		AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_Clone(this.swigCPtr, AkReflectionPathInfo.getCPtr(other));
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
