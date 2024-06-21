// dnSpy decompiler from Assembly-CSharp.dll class: AkInitSettings
using System;

public class AkInitSettings : IDisposable
{
	internal AkInitSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkInitSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkInitSettings(), true)
	{
	}

	internal static IntPtr getCPtr(AkInitSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkInitSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkInitSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public int pfnAssertHook
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public uint uMaxNumPaths
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxNumPaths_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxNumPaths_set(this.swigCPtr, value);
		}
	}

	public uint uDefaultPoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uDefaultPoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uDefaultPoolSize_set(this.swigCPtr, value);
		}
	}

	public float fDefaultPoolRatioThreshold
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_fDefaultPoolRatioThreshold_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_fDefaultPoolRatioThreshold_set(this.swigCPtr, value);
		}
	}

	public uint uCommandQueueSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uCommandQueueSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uCommandQueueSize_set(this.swigCPtr, value);
		}
	}

	public int uPrepareEventMemoryPoolID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uPrepareEventMemoryPoolID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uPrepareEventMemoryPoolID_set(this.swigCPtr, value);
		}
	}

	public bool bEnableGameSyncPreparation
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_bEnableGameSyncPreparation_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_bEnableGameSyncPreparation_set(this.swigCPtr, value);
		}
	}

	public uint uContinuousPlaybackLookAhead
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uContinuousPlaybackLookAhead_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uContinuousPlaybackLookAhead_set(this.swigCPtr, value);
		}
	}

	public uint uNumSamplesPerFrame
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uNumSamplesPerFrame_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uNumSamplesPerFrame_set(this.swigCPtr, value);
		}
	}

	public uint uMonitorPoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorPoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorPoolSize_set(this.swigCPtr, value);
		}
	}

	public uint uMonitorQueuePoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorQueuePoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorQueuePoolSize_set(this.swigCPtr, value);
		}
	}

	public AkOutputSettings settingsMainOutput
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkInitSettings_settingsMainOutput_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkOutputSettings(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_settingsMainOutput_set(this.swigCPtr, AkOutputSettings.getCPtr(value));
		}
	}

	public uint uMaxHardwareTimeoutMs
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxHardwareTimeoutMs_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxHardwareTimeoutMs_set(this.swigCPtr, value);
		}
	}

	public bool bUseSoundBankMgrThread
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_bUseSoundBankMgrThread_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_bUseSoundBankMgrThread_set(this.swigCPtr, value);
		}
	}

	public bool bUseLEngineThread
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitSettings_bUseLEngineThread_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_bUseLEngineThread_set(this.swigCPtr, value);
		}
	}

	public string szPluginDLLPath
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrOSString(AkSoundEnginePINVOKE.CSharp_AkInitSettings_szPluginDLLPath_get(this.swigCPtr));
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_szPluginDLLPath_set(this.swigCPtr, value);
		}
	}

	public AkFloorPlane eFloorPlane
	{
		get
		{
			return (AkFloorPlane)AkSoundEnginePINVOKE.CSharp_AkInitSettings_eFloorPlane_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitSettings_eFloorPlane_set(this.swigCPtr, (int)value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
