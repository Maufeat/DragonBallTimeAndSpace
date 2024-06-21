// dnSpy decompiler from Assembly-CSharp.dll class: AkUnityPlatformSpecificSettings
using System;

public class AkUnityPlatformSpecificSettings : IDisposable
{
	internal AkUnityPlatformSpecificSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkUnityPlatformSpecificSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkUnityPlatformSpecificSettings(), true)
	{
	}

	internal static IntPtr getCPtr(AkUnityPlatformSpecificSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkUnityPlatformSpecificSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkUnityPlatformSpecificSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
