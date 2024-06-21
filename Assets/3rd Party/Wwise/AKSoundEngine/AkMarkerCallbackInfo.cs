// dnSpy decompiler from Assembly-CSharp.dll class: AkMarkerCallbackInfo
using System;

public class AkMarkerCallbackInfo : AkEventCallbackInfo
{
	internal AkMarkerCallbackInfo(IntPtr cPtr, bool cMemoryOwn) : base(AkSoundEnginePINVOKE.CSharp_AkMarkerCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	public AkMarkerCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkMarkerCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkMarkerCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkMarkerCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	~AkMarkerCallbackInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkMarkerCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	public uint uIdentifier
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMarkerCallbackInfo_uIdentifier_get(this.swigCPtr);
		}
	}

	public uint uPosition
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMarkerCallbackInfo_uPosition_get(this.swigCPtr);
		}
	}

	public string strLabel
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrString(AkSoundEnginePINVOKE.CSharp_AkMarkerCallbackInfo_strLabel_get(this.swigCPtr));
		}
	}

	private IntPtr swigCPtr;
}
