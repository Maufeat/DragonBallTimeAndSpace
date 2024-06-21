// dnSpy decompiler from Assembly-CSharp.dll class: AkRoomParams
using System;

public class AkRoomParams : IDisposable
{
	internal AkRoomParams(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkRoomParams() : this(AkSoundEnginePINVOKE.CSharp_new_AkRoomParams(), true)
	{
	}

	internal static IntPtr getCPtr(AkRoomParams obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkRoomParams()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkRoomParams(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkVector Up
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkRoomParams_Up_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_Up_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	public AkVector Front
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkRoomParams_Front_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkVector(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_Front_set(this.swigCPtr, AkVector.getCPtr(value));
		}
	}

	public uint ReverbAuxBus
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_ReverbAuxBus_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_ReverbAuxBus_set(this.swigCPtr, value);
		}
	}

	public float ReverbLevel
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_ReverbLevel_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_ReverbLevel_set(this.swigCPtr, value);
		}
	}

	public float WallOcclusion
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_WallOcclusion_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_WallOcclusion_set(this.swigCPtr, value);
		}
	}

	public float RoomGameObj_AuxSendLevelToSelf
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_RoomGameObj_AuxSendLevelToSelf_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_RoomGameObj_AuxSendLevelToSelf_set(this.swigCPtr, value);
		}
	}

	public bool RoomGameObj_KeepRegistered
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkRoomParams_RoomGameObj_KeepRegistered_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkRoomParams_RoomGameObj_KeepRegistered_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
