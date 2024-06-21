// dnSpy decompiler from Assembly-CSharp.dll class: AkSpatialAudioInitSettings
using System;

public class AkSpatialAudioInitSettings : IDisposable
{
	internal AkSpatialAudioInitSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkSpatialAudioInitSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkSpatialAudioInitSettings(), true)
	{
	}

	internal static IntPtr getCPtr(AkSpatialAudioInitSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkSpatialAudioInitSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkSpatialAudioInitSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public int uPoolID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolID_set(this.swigCPtr, value);
		}
	}

	public uint uPoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolSize_set(this.swigCPtr, value);
		}
	}

	public uint uMaxSoundPropagationDepth
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uMaxSoundPropagationDepth_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uMaxSoundPropagationDepth_set(this.swigCPtr, value);
		}
	}

	public uint uDiffractionFlags
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uDiffractionFlags_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uDiffractionFlags_set(this.swigCPtr, value);
		}
	}

	public float fDiffractionShadowAttenFactor
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowAttenFactor_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowAttenFactor_set(this.swigCPtr, value);
		}
	}

	public float fDiffractionShadowDegrees
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowDegrees_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowDegrees_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
