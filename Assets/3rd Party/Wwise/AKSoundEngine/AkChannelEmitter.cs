// dnSpy decompiler from Assembly-CSharp.dll class: AkChannelEmitter
using System;

public class AkChannelEmitter : IDisposable
{
	internal AkChannelEmitter(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkChannelEmitter() : this(AkSoundEnginePINVOKE.CSharp_new_AkChannelEmitter(), true)
	{
	}

	internal static IntPtr getCPtr(AkChannelEmitter obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkChannelEmitter()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkChannelEmitter(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkTransform position
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkChannelEmitter_position_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkTransform(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkChannelEmitter_position_set(this.swigCPtr, AkTransform.getCPtr(value));
		}
	}

	public uint uInputChannels
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkChannelEmitter_uInputChannels_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkChannelEmitter_uInputChannels_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
