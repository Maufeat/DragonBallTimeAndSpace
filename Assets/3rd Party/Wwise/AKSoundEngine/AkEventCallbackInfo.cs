// dnSpy decompiler from Assembly-CSharp.dll class: AkEventCallbackInfo
using System;

public class AkEventCallbackInfo : AkCallbackInfo
{
	internal AkEventCallbackInfo(IntPtr cPtr, bool cMemoryOwn) : base(AkSoundEnginePINVOKE.CSharp_AkEventCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	public AkEventCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkEventCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkEventCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkEventCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	~AkEventCallbackInfo()
	{
		this.Dispose();
	}

	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkEventCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	public uint playingID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEventCallbackInfo_playingID_get(this.swigCPtr);
		}
	}

	public uint eventID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkEventCallbackInfo_eventID_get(this.swigCPtr);
		}
	}

	private IntPtr swigCPtr;
}
