// dnSpy decompiler from Assembly-CSharp.dll class: AkSourceSettings
using System;

public class AkSourceSettings : IDisposable
{
	internal AkSourceSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkSourceSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkSourceSettings(), true)
	{
	}

	internal static IntPtr getCPtr(AkSourceSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkSourceSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkSourceSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public uint sourceID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSourceSettings_sourceID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSourceSettings_sourceID_set(this.swigCPtr, value);
		}
	}

	public IntPtr pMediaMemory
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSourceSettings_pMediaMemory_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSourceSettings_pMediaMemory_set(this.swigCPtr, value);
		}
	}

	public uint uMediaSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkSourceSettings_uMediaSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkSourceSettings_uMediaSize_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
