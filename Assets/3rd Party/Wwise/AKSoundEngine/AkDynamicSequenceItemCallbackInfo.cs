// dnSpy decompiler from Assembly-CSharp.dll class: AkDynamicSequenceItemCallbackInfo
using System;

public class AkDynamicSequenceItemCallbackInfo : AkCallbackInfo
{
	internal AkDynamicSequenceItemCallbackInfo(IntPtr cPtr, bool cMemoryOwn) : base(AkSoundEnginePINVOKE.CSharp_AkDynamicSequenceItemCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	public AkDynamicSequenceItemCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkDynamicSequenceItemCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkDynamicSequenceItemCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkDynamicSequenceItemCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	~AkDynamicSequenceItemCallbackInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkDynamicSequenceItemCallbackInfo(this.swigCPtr);
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
			return AkSoundEnginePINVOKE.CSharp_AkDynamicSequenceItemCallbackInfo_playingID_get(this.swigCPtr);
		}
	}

	public uint audioNodeID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDynamicSequenceItemCallbackInfo_audioNodeID_get(this.swigCPtr);
		}
	}

	public IntPtr pCustomInfo
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDynamicSequenceItemCallbackInfo_pCustomInfo_get(this.swigCPtr);
		}
	}

	private IntPtr swigCPtr;
}
