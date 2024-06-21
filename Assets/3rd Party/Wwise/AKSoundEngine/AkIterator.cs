// dnSpy decompiler from Assembly-CSharp.dll class: AkIterator
using System;

public class AkIterator : IDisposable
{
	internal AkIterator(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkIterator() : this(AkSoundEnginePINVOKE.CSharp_new_AkIterator(), true)
	{
	}

	internal static IntPtr getCPtr(AkIterator obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkIterator()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkIterator(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkPlaylistItem pItem
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkIterator_pItem_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkIterator_pItem_set(this.swigCPtr, AkPlaylistItem.getCPtr(value));
		}
	}

	public AkIterator NextIter()
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkIterator_NextIter(this.swigCPtr), false);
	}

	public AkIterator PrevIter()
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkIterator_PrevIter(this.swigCPtr), false);
	}

	public AkPlaylistItem GetItem()
	{
		return new AkPlaylistItem(AkSoundEnginePINVOKE.CSharp_AkIterator_GetItem(this.swigCPtr), false);
	}

	public bool IsEqualTo(AkIterator in_rOp)
	{
		return AkSoundEnginePINVOKE.CSharp_AkIterator_IsEqualTo(this.swigCPtr, AkIterator.getCPtr(in_rOp));
	}

	public bool IsDifferentFrom(AkIterator in_rOp)
	{
		return AkSoundEnginePINVOKE.CSharp_AkIterator_IsDifferentFrom(this.swigCPtr, AkIterator.getCPtr(in_rOp));
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
