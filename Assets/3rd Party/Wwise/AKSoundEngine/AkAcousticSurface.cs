using System;

public class AkAcousticSurface : IDisposable
{
	internal AkAcousticSurface(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkAcousticSurface() : this(AkSoundEnginePINVOKE.CSharp_new_AkAcousticSurface(), true)
	{
	}

	internal static IntPtr getCPtr(AkAcousticSurface obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkAcousticSurface()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkAcousticSurface(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public uint textureID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_textureID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_textureID_set(this.swigCPtr, value);
		}
	}

	public uint reflectorChannelMask
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_reflectorChannelMask_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_reflectorChannelMask_set(this.swigCPtr, value);
		}
	}

	public string strName
	{
		get
		{
			return AkSoundEngine.StringFromIntPtrString(AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_strName_get(this.swigCPtr));
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_strName_set(this.swigCPtr, value);
		}
	}

	public void Clear()
	{
		AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_Clear(this.swigCPtr);
	}

	public void DeleteName()
	{
		AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_DeleteName(this.swigCPtr);
	}

	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_GetSizeOf();
	}

	public void Clone(AkAcousticSurface other)
	{
		AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_Clone(this.swigCPtr, AkAcousticSurface.getCPtr(other));
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
