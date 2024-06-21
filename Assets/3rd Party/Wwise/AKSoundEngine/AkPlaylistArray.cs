// dnSpy decompiler from Assembly-CSharp.dll class: AkPlaylistArray
using System;

public class AkPlaylistArray : IDisposable
{
	internal AkPlaylistArray(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkPlaylistArray() : this(AkSoundEnginePINVOKE.CSharp_new_AkPlaylistArray(), true)
	{
	}

	internal static IntPtr getCPtr(AkPlaylistArray obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkPlaylistArray()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkPlaylistArray(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkIterator Begin()
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Begin(this.swigCPtr), true);
	}

	public AkIterator End()
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_End(this.swigCPtr), true);
	}

	public AkIterator FindEx(AkPlaylistItem in_Item)
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_FindEx(this.swigCPtr, AkPlaylistItem.getCPtr(in_Item)), true);
	}

	public AkIterator Erase(AkIterator in_rIter)
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Erase__SWIG_0(this.swigCPtr, AkIterator.getCPtr(in_rIter)), true);
	}

	public void Erase(uint in_uIndex)
	{
		AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Erase__SWIG_1(this.swigCPtr, in_uIndex);
	}

	public AkIterator EraseSwap(AkIterator in_rIter)
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_EraseSwap(this.swigCPtr, AkIterator.getCPtr(in_rIter)), true);
	}

	public AKRESULT Reserve(uint in_ulReserve)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Reserve(this.swigCPtr, in_ulReserve);
	}

	public uint Reserved()
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Reserved(this.swigCPtr);
	}

	public void Term()
	{
		AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Term(this.swigCPtr);
	}

	public uint Length()
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Length(this.swigCPtr);
	}

	public AkPlaylistItem Data()
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Data(this.swigCPtr);
		return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
	}

	public bool IsEmpty()
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_IsEmpty(this.swigCPtr);
	}

	public AkPlaylistItem Exists(AkPlaylistItem in_Item)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Exists(this.swigCPtr, AkPlaylistItem.getCPtr(in_Item));
		return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
	}

	public AkPlaylistItem AddLast()
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_AddLast__SWIG_0(this.swigCPtr);
		return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
	}

	public AkPlaylistItem AddLast(AkPlaylistItem in_rItem)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_AddLast__SWIG_1(this.swigCPtr, AkPlaylistItem.getCPtr(in_rItem));
		return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
	}

	public AkPlaylistItem Last()
	{
		return new AkPlaylistItem(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Last(this.swigCPtr), false);
	}

	public void RemoveLast()
	{
		AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_RemoveLast(this.swigCPtr);
	}

	public AKRESULT Remove(AkPlaylistItem in_rItem)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Remove(this.swigCPtr, AkPlaylistItem.getCPtr(in_rItem));
	}

	public AKRESULT RemoveSwap(AkPlaylistItem in_rItem)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_RemoveSwap(this.swigCPtr, AkPlaylistItem.getCPtr(in_rItem));
	}

	public void RemoveAll()
	{
		AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_RemoveAll(this.swigCPtr);
	}

	public AkPlaylistItem ItemAtIndex(uint uiIndex)
	{
		return new AkPlaylistItem(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_ItemAtIndex(this.swigCPtr, uiIndex), false);
	}

	public AkPlaylistItem Insert(uint in_uIndex)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Insert(this.swigCPtr, in_uIndex);
		return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
	}

	public bool GrowArray(uint in_uGrowBy)
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_GrowArray__SWIG_0(this.swigCPtr, in_uGrowBy);
	}

	public bool GrowArray()
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_GrowArray__SWIG_1(this.swigCPtr);
	}

	public bool Resize(uint in_uiSize)
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Resize(this.swigCPtr, in_uiSize);
	}

	public void Transfer(AkPlaylistArray in_rSource)
	{
		AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Transfer(this.swigCPtr, AkPlaylistArray.getCPtr(in_rSource));
	}

	public AKRESULT Copy(AkPlaylistArray in_rSource)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Copy(this.swigCPtr, AkPlaylistArray.getCPtr(in_rSource));
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
