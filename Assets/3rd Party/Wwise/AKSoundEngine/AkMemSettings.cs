// dnSpy decompiler from Assembly-CSharp.dll class: AkMemSettings
using System;

public class AkMemSettings : IDisposable
{
	internal AkMemSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkMemSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkMemSettings(), true)
	{
	}

	internal static IntPtr getCPtr(AkMemSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkMemSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkMemSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public uint uMaxNumPools
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMemSettings_uMaxNumPools_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMemSettings_uMaxNumPools_set(this.swigCPtr, value);
		}
	}

	public uint uDebugFlags
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMemSettings_uDebugFlags_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMemSettings_uDebugFlags_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
