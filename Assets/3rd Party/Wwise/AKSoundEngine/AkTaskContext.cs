// dnSpy decompiler from Assembly-CSharp.dll class: AkTaskContext
using System;

public class AkTaskContext : IDisposable
{
	internal AkTaskContext(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkTaskContext() : this(AkSoundEnginePINVOKE.CSharp_new_AkTaskContext(), true)
	{
	}

	internal static IntPtr getCPtr(AkTaskContext obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkTaskContext()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkTaskContext(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public uint uIdxThread
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkTaskContext_uIdxThread_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkTaskContext_uIdxThread_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
