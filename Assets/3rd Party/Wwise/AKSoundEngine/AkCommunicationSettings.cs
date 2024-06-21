// dnSpy decompiler from Assembly-CSharp.dll class: AkCommunicationSettings
using System;

public class AkCommunicationSettings : IDisposable
{
	internal AkCommunicationSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkCommunicationSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkCommunicationSettings(), true)
	{
	}

	internal static IntPtr getCPtr(AkCommunicationSettings obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkCommunicationSettings()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkCommunicationSettings(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public uint uPoolSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_uPoolSize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_uPoolSize_set(this.swigCPtr, value);
		}
	}

	public ushort uDiscoveryBroadcastPort
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_uDiscoveryBroadcastPort_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_uDiscoveryBroadcastPort_set(this.swigCPtr, value);
		}
	}

	public ushort uCommandPort
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_uCommandPort_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_uCommandPort_set(this.swigCPtr, value);
		}
	}

	public ushort uNotificationPort
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_uNotificationPort_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_uNotificationPort_set(this.swigCPtr, value);
		}
	}

	public bool bInitSystemLib
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_bInitSystemLib_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_bInitSystemLib_set(this.swigCPtr, value);
		}
	}

	public string szAppNetworkName
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrString(AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_szAppNetworkName_get(this.swigCPtr));
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkCommunicationSettings_szAppNetworkName_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
