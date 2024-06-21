// dnSpy decompiler from Assembly-CSharp.dll class: AkMIDIEventCallbackInfo
using System;

public class AkMIDIEventCallbackInfo : AkEventCallbackInfo
{
	internal AkMIDIEventCallbackInfo(IntPtr cPtr, bool cMemoryOwn) : base(AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	public AkMIDIEventCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEventCallbackInfo(), true)
	{
	}

	internal static IntPtr getCPtr(AkMIDIEventCallbackInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	~AkMIDIEventCallbackInfo()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEventCallbackInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	public byte byChan
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byChan_get(this.swigCPtr);
		}
	}

	public byte byParam1
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byParam1_get(this.swigCPtr);
		}
	}

	public byte byParam2
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byParam2_get(this.swigCPtr);
		}
	}

	public AkMIDIEventTypes byType
	{
		get
		{
			return (AkMIDIEventTypes)AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byType_get(this.swigCPtr);
		}
	}

	public byte byOnOffNote
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byOnOffNote_get(this.swigCPtr);
		}
	}

	public byte byVelocity
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byVelocity_get(this.swigCPtr);
		}
	}

	public AkMIDICcTypes byCc
	{
		get
		{
			return (AkMIDICcTypes)AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byCc_get(this.swigCPtr);
		}
	}

	public byte byCcValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byCcValue_get(this.swigCPtr);
		}
	}

	public byte byValueLsb
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byValueLsb_get(this.swigCPtr);
		}
	}

	public byte byValueMsb
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byValueMsb_get(this.swigCPtr);
		}
	}

	public byte byAftertouchNote
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byAftertouchNote_get(this.swigCPtr);
		}
	}

	public byte byNoteAftertouchValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byNoteAftertouchValue_get(this.swigCPtr);
		}
	}

	public byte byChanAftertouchValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byChanAftertouchValue_get(this.swigCPtr);
		}
	}

	public byte byProgramNum
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEventCallbackInfo_byProgramNum_get(this.swigCPtr);
		}
	}

	private IntPtr swigCPtr;
}
