// dnSpy decompiler from Assembly-CSharp.dll class: AkAuxSendValue
using System;
using UnityEngine;

public class AkAuxSendValue : IDisposable
{
	internal AkAuxSendValue(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	internal static IntPtr getCPtr(AkAuxSendValue obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkAuxSendValue()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkAuxSendValue(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public ulong listenerID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_listenerID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_listenerID_set(this.swigCPtr, value);
		}
	}

	public uint auxBusID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_auxBusID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_auxBusID_set(this.swigCPtr, value);
		}
	}

	public float fControlValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_fControlValue_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_fControlValue_set(this.swigCPtr, value);
		}
	}

	public void Set(GameObject listener, uint id, float value)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(listener);
		AkSoundEngine.PreGameObjectAPICall(listener, akGameObjectID);
		AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_Set(this.swigCPtr, akGameObjectID, id, value);
	}

	public bool IsSame(GameObject listener, uint id)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(listener);
		AkSoundEngine.PreGameObjectAPICall(listener, akGameObjectID);
		return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_IsSame(this.swigCPtr, akGameObjectID, id);
	}

	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_GetSizeOf();
	}

	public AKRESULT SetGameObjectAuxSendValues(GameObject in_gameObjectID, uint in_uNumSendValues)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_SetGameObjectAuxSendValues(this.swigCPtr, akGameObjectID, in_uNumSendValues);
	}

	public AKRESULT GetGameObjectAuxSendValues(GameObject in_gameObjectID, ref uint io_ruNumSendValues)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_GetGameObjectAuxSendValues(this.swigCPtr, akGameObjectID, ref io_ruNumSendValues);
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
