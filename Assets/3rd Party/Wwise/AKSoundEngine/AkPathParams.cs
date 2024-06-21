// dnSpy decompiler from Assembly-CSharp.dll class: AkPathParams
using System;

public class AkPathParams : IDisposable
{
	internal AkPathParams(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkPathParams() : this(AkSoundEnginePINVOKE.CSharp_new_AkPathParams(), true)
	{
	}

	internal static IntPtr getCPtr(AkPathParams obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkPathParams()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkPathParams(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkVector listenerPos
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPathParams_listenerPos_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPathParams_listenerPos_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	public AkVector emitterPos
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPathParams_emitterPos_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPathParams_emitterPos_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	public uint numValidPaths
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPathParams_numValidPaths_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPathParams_numValidPaths_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
