// dnSpy decompiler from Assembly-CSharp.dll class: AkEmitterSettings
using System;

public class AkEmitterSettings : IDisposable
{
	internal AkEmitterSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkEmitterSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkEmitterSettings(), true)
	{
	}

	internal static IntPtr getCPtr(AkEmitterSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkEmitterSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkEmitterSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public uint reflectAuxBusID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectAuxBusID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectAuxBusID_set(this.swigCPtr, value);
		}
	}

	public float reflectionMaxPathLength
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionMaxPathLength_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionMaxPathLength_set(this.swigCPtr, value);
		}
	}

	public float reflectionsAuxBusGain
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionsAuxBusGain_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionsAuxBusGain_set(this.swigCPtr, value);
		}
	}

	public uint reflectionsOrder
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionsOrder_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectionsOrder_set(this.swigCPtr, value);
		}
	}

	public uint reflectorFilterMask
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectorFilterMask_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_reflectorFilterMask_set(this.swigCPtr, value);
		}
	}

	public float roomReverbAuxBusGain
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_roomReverbAuxBusGain_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_roomReverbAuxBusGain_set(this.swigCPtr, value);
		}
	}

	public uint diffractionMaxEdges
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_diffractionMaxEdges_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_diffractionMaxEdges_set(this.swigCPtr, value);
		}
	}

	public uint diffractionMaxPaths
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_diffractionMaxPaths_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_diffractionMaxPaths_set(this.swigCPtr, value);
		}
	}

	public float diffractionMaxPathLength
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_diffractionMaxPathLength_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_diffractionMaxPathLength_set(this.swigCPtr, value);
		}
	}

	public byte useImageSources
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_useImageSources_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkEmitterSettings_useImageSources_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
