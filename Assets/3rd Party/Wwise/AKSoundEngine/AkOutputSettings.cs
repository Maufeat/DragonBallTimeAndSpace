// dnSpy decompiler from Assembly-CSharp.dll class: AkOutputSettings
using System;

public class AkOutputSettings : IDisposable
{
	internal AkOutputSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkOutputSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkOutputSettings__SWIG_0(), true)
	{
	}

	public AkOutputSettings(string in_szDeviceShareSet, uint in_idDevice, AkChannelConfig in_channelConfig, AkPanningRule in_ePanning) : this(AkSoundEnginePINVOKE.CSharp_new_AkOutputSettings__SWIG_1(in_szDeviceShareSet, in_idDevice, AkChannelConfig.getCPtr(in_channelConfig), (int)in_ePanning), true)
	{
	}

	public AkOutputSettings(string in_szDeviceShareSet, uint in_idDevice, AkChannelConfig in_channelConfig) : this(AkSoundEnginePINVOKE.CSharp_new_AkOutputSettings__SWIG_2(in_szDeviceShareSet, in_idDevice, AkChannelConfig.getCPtr(in_channelConfig)), true)
	{
	}

	public AkOutputSettings(string in_szDeviceShareSet, uint in_idDevice) : this(AkSoundEnginePINVOKE.CSharp_new_AkOutputSettings__SWIG_3(in_szDeviceShareSet, in_idDevice), true)
	{
	}

	public AkOutputSettings(string in_szDeviceShareSet) : this(AkSoundEnginePINVOKE.CSharp_new_AkOutputSettings__SWIG_4(in_szDeviceShareSet), true)
	{
	}

	internal static IntPtr getCPtr(AkOutputSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkOutputSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkOutputSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public uint audioDeviceShareset
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkOutputSettings_audioDeviceShareset_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkOutputSettings_audioDeviceShareset_set(this.swigCPtr, value);
		}
	}

	public uint idDevice
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkOutputSettings_idDevice_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkOutputSettings_idDevice_set(this.swigCPtr, value);
		}
	}

	public AkPanningRule ePanningRule
	{
		get
		{
			return (AkPanningRule)AkSoundEnginePINVOKE.CSharp_AkOutputSettings_ePanningRule_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkOutputSettings_ePanningRule_set(this.swigCPtr, (int)value);
		}
	}

	public AkChannelConfig channelConfig
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkOutputSettings_channelConfig_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkChannelConfig(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkOutputSettings_channelConfig_set(this.swigCPtr, AkChannelConfig.getCPtr(value));
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
