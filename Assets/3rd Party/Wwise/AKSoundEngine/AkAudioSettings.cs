// dnSpy decompiler from Assembly-CSharp.dll class: AkAudioSettings
using System;

public class AkAudioSettings : IDisposable
{
	internal AkAudioSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkAudioSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkAudioSettings(), true)
	{
	}

	internal static IntPtr getCPtr(AkAudioSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkAudioSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkAudioSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public uint uNumSamplesPerFrame
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioSettings_uNumSamplesPerFrame_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAudioSettings_uNumSamplesPerFrame_set(this.swigCPtr, value);
		}
	}

	public uint uNumSamplesPerSecond
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAudioSettings_uNumSamplesPerSecond_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAudioSettings_uNumSamplesPerSecond_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
