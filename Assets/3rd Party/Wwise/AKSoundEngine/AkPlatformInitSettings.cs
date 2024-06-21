// dnSpy decompiler from Assembly-CSharp.dll class: AkPlatformInitSettings
using System;

public class AkPlatformInitSettings : IDisposable
{
	internal AkPlatformInitSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkPlatformInitSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkPlatformInitSettings(), true)
	{
	}

	internal static IntPtr getCPtr(AkPlatformInitSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkPlatformInitSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkPlatformInitSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkThreadProperties threadLEngine
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadLEngine_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkThreadProperties(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadLEngine_set(this.swigCPtr, AkThreadProperties.getCPtr(value));
		}
	}

	public AkThreadProperties threadBankManager
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadBankManager_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkThreadProperties(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadBankManager_set(this.swigCPtr, AkThreadProperties.getCPtr(value));
		}
	}

	public AkThreadProperties threadMonitor
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadMonitor_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkThreadProperties(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_threadMonitor_set(this.swigCPtr, AkThreadProperties.getCPtr(value));
		}
	}

	public uint uLEngineDefaultPoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uLEngineDefaultPoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uLEngineDefaultPoolSize_set(this.swigCPtr, value);
		}
	}

	public float fLEngineDefaultPoolRatioThreshold
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_fLEngineDefaultPoolRatioThreshold_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_fLEngineDefaultPoolRatioThreshold_set(this.swigCPtr, value);
		}
	}

	public ushort uNumRefillsInVoice
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uNumRefillsInVoice_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uNumRefillsInVoice_set(this.swigCPtr, value);
		}
	}

	public uint uSampleRate
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uSampleRate_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_uSampleRate_set(this.swigCPtr, value);
		}
	}

	public AkAudioAPI eAudioAPI
	{
		get
		{
			return (AkAudioAPI)AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_eAudioAPI_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_eAudioAPI_set(this.swigCPtr, (int)value);
		}
	}

	public bool bGlobalFocus
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_bGlobalFocus_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlatformInitSettings_bGlobalFocus_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
