// dnSpy decompiler from Assembly-CSharp.dll class: AkMonitoringCallbackInfo
using System;

public class AkMonitoringCallbackInfo : IDisposable
{
	internal AkMonitoringCallbackInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkMonitoringCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkMonitoringCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkMonitoringCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkMonitoringCallbackInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkMonitoringCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkMonitorErrorCode errorCode
	{
		get
		{
			return (AkMonitorErrorCode)AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_errorCode_get(this.swigCPtr);
		}
	}

	public AkMonitorErrorLevel errorLevel
	{
		get
		{
			return (AkMonitorErrorLevel)AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_errorLevel_get(this.swigCPtr);
		}
	}

	public uint playingID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_playingID_get(this.swigCPtr);
		}
	}

	public ulong gameObjID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_gameObjID_get(this.swigCPtr);
		}
	}

	public string message
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrOSString(AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_message_get(this.swigCPtr));
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
