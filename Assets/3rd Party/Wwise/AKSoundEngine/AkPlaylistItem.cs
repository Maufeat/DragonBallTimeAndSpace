// dnSpy decompiler from Assembly-CSharp.dll class: AkPlaylistItem
using System;

public class AkPlaylistItem : IDisposable
{
	internal AkPlaylistItem(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkPlaylistItem() : this(AkSoundEnginePINVOKE.CSharp_new_AkPlaylistItem__SWIG_0(), true)
	{
	}

	public AkPlaylistItem(AkPlaylistItem in_rCopy) : this(AkSoundEnginePINVOKE.CSharp_new_AkPlaylistItem__SWIG_1(AkPlaylistItem.getCPtr(in_rCopy)), true)
	{
	}

	internal static IntPtr getCPtr(AkPlaylistItem obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkPlaylistItem()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkPlaylistItem(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkPlaylistItem Assign(AkPlaylistItem in_rCopy)
	{
		return new AkPlaylistItem(AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_Assign(this.swigCPtr, AkPlaylistItem.getCPtr(in_rCopy)), false);
	}

	public bool IsEqualTo(AkPlaylistItem in_rCopy)
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_IsEqualTo(this.swigCPtr, AkPlaylistItem.getCPtr(in_rCopy));
	}

	public AKRESULT SetExternalSources(uint in_nExternalSrc, AkExternalSourceInfo in_pExternalSrc)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_SetExternalSources(this.swigCPtr, in_nExternalSrc, AkExternalSourceInfo.getCPtr(in_pExternalSrc));
	}

	public uint audioNodeID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_audioNodeID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_audioNodeID_set(this.swigCPtr, value);
		}
	}

	public int msDelay
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_msDelay_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_msDelay_set(this.swigCPtr, value);
		}
	}

	public IntPtr pCustomInfo
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_pCustomInfo_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkPlaylistItem_pCustomInfo_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
