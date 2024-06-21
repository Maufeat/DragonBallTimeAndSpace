// dnSpy decompiler from Assembly-CSharp.dll class: AkAuxSendArray
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class AkAuxSendArray : IDisposable
{
	public AkAuxSendArray()
	{
		this.m_Buffer = Marshal.AllocHGlobal(4 * this.SIZE_OF_AKAUXSENDVALUE);
		this.m_Count = 0;
	}

	public AkAuxSendValue this[int index]
	{
		get
		{
			if (index >= this.m_Count)
			{
				throw new IndexOutOfRangeException("Out of range access in AkAuxSendArray");
			}
			return new AkAuxSendValue(this.GetObjectPtr(index), false);
		}
	}

	public bool isFull
	{
		get
		{
			return this.m_Count >= 4 || this.m_Buffer == IntPtr.Zero;
		}
	}

	public void Dispose()
	{
		if (this.m_Buffer != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.m_Buffer);
			this.m_Buffer = IntPtr.Zero;
			this.m_Count = 0;
		}
	}

	~AkAuxSendArray()
	{
		this.Dispose();
	}

	public void Reset()
	{
		this.m_Count = 0;
	}

	public bool Add(GameObject in_listenerGameObj, uint in_AuxBusID, float in_fValue)
	{
		if (this.isFull)
		{
			return false;
		}
		AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_Set(this.GetObjectPtr(this.m_Count), AkSoundEngine.GetAkGameObjectID(in_listenerGameObj), in_AuxBusID, in_fValue);
		this.m_Count++;
		return true;
	}

	public bool Add(uint in_AuxBusID, float in_fValue)
	{
		if (this.isFull)
		{
			return false;
		}
		AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_Set(this.GetObjectPtr(this.m_Count), ulong.MaxValue, in_AuxBusID, in_fValue);
		this.m_Count++;
		return true;
	}

	public bool Contains(GameObject in_listenerGameObj, uint in_AuxBusID)
	{
		if (this.m_Buffer == IntPtr.Zero)
		{
			return false;
		}
		for (int i = 0; i < this.m_Count; i++)
		{
			if (AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_IsSame(this.GetObjectPtr(i), AkSoundEngine.GetAkGameObjectID(in_listenerGameObj), in_AuxBusID))
			{
				return true;
			}
		}
		return false;
	}

	public bool Contains(uint in_AuxBusID)
	{
		if (this.m_Buffer == IntPtr.Zero)
		{
			return false;
		}
		for (int i = 0; i < this.m_Count; i++)
		{
			if (AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_IsSame(this.GetObjectPtr(i), 18446744073709551615UL, in_AuxBusID))
			{
				return true;
			}
		}
		return false;
	}

	public AKRESULT SetValues(GameObject gameObject)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_SetGameObjectAuxSendValues(this.m_Buffer, AkSoundEngine.GetAkGameObjectID(gameObject), (uint)this.m_Count);
	}

	public AKRESULT GetValues(GameObject gameObject)
	{
		uint count = 4u;
		AKRESULT result = (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_GetGameObjectAuxSendValues(this.m_Buffer, AkSoundEngine.GetAkGameObjectID(gameObject), ref count);
		this.m_Count = (int)count;
		return result;
	}

	public IntPtr GetBuffer()
	{
		return this.m_Buffer;
	}

	public int Count()
	{
		return this.m_Count;
	}

	private IntPtr GetObjectPtr(int index)
	{
		return (IntPtr)(this.m_Buffer.ToInt64() + (long)(this.SIZE_OF_AKAUXSENDVALUE * index));
	}

	private const int MAX_COUNT = 4;

	private readonly int SIZE_OF_AKAUXSENDVALUE = AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_GetSizeOf();

	private IntPtr m_Buffer;

	private int m_Count;
}
