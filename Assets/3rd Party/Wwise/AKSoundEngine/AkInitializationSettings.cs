// dnSpy decompiler from Assembly-CSharp.dll class: AkInitializationSettings
using System;

public class AkInitializationSettings : IDisposable
{
	internal AkInitializationSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkInitializationSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkInitializationSettings(), true)
	{
	}

	internal static IntPtr getCPtr(AkInitializationSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkInitializationSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkInitializationSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkMemSettings memSettings
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_memSettings_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMemSettings(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_memSettings_set(this.swigCPtr, AkMemSettings.getCPtr(value));
		}
	}

	public AkStreamMgrSettings streamMgrSettings
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_streamMgrSettings_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkStreamMgrSettings(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_streamMgrSettings_set(this.swigCPtr, AkStreamMgrSettings.getCPtr(value));
		}
	}

	public AkDeviceSettings deviceSettings
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_deviceSettings_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkDeviceSettings(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_deviceSettings_set(this.swigCPtr, AkDeviceSettings.getCPtr(value));
		}
	}

	public AkInitSettings initSettings
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_initSettings_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkInitSettings(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_initSettings_set(this.swigCPtr, AkInitSettings.getCPtr(value));
		}
	}

	public AkPlatformInitSettings platformSettings
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_platformSettings_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkPlatformInitSettings(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_platformSettings_set(this.swigCPtr, AkPlatformInitSettings.getCPtr(value));
		}
	}

	public AkMusicSettings musicSettings
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_musicSettings_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMusicSettings(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_musicSettings_set(this.swigCPtr, AkMusicSettings.getCPtr(value));
		}
	}

	public uint preparePoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_preparePoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_preparePoolSize_set(this.swigCPtr, value);
		}
	}

	public AkCommunicationSettings communicationSettings
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_communicationSettings_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkCommunicationSettings(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_communicationSettings_set(this.swigCPtr, AkCommunicationSettings.getCPtr(value));
		}
	}

	public AkUnityPlatformSpecificSettings unityPlatformSpecificSettings
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_unityPlatformSpecificSettings_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkUnityPlatformSpecificSettings(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkInitializationSettings_unityPlatformSpecificSettings_set(this.swigCPtr, AkUnityPlatformSpecificSettings.getCPtr(value));
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
