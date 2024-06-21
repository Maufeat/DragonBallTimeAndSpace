// dnSpy decompiler from Assembly-CSharp.dll class: AkMIDIPostArray
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class AkMIDIPostArray
{
	public AkMIDIPostArray(int size)
	{
		this.m_Count = size;
		this.m_Buffer = Marshal.AllocHGlobal(this.m_Count * this.SIZE_OF);
	}

	public AkMIDIPost this[int index]
	{
		get
		{
			if (index >= this.m_Count)
			{
				throw new IndexOutOfRangeException("Out of range access in AkMIDIPostArray");
			}
			return new AkMIDIPost(this.GetObjectPtr(index), false);
		}
		set
		{
			if (index >= this.m_Count)
			{
				throw new IndexOutOfRangeException("Out of range access in AkMIDIPostArray");
			}
			AkSoundEnginePINVOKE.CSharp_AkMIDIPost_Clone(this.GetObjectPtr(index), AkMIDIPost.getCPtr(value));
		}
	}

	~AkMIDIPostArray()
	{
		Marshal.FreeHGlobal(this.m_Buffer);
		this.m_Buffer = IntPtr.Zero;
	}

	public void PostOnEvent(uint in_eventID, GameObject gameObject)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
		AkSoundEngine.PreGameObjectAPICall(gameObject, akGameObjectID);
		AkSoundEnginePINVOKE.CSharp_AkMIDIPost_PostOnEvent(this.m_Buffer, in_eventID, akGameObjectID, (uint)this.m_Count);
	}

	public void PostOnEvent(uint in_eventID, GameObject gameObject, int count)
	{
		if (count >= this.m_Count)
		{
			throw new IndexOutOfRangeException("Out of range access in AkMIDIPostArray");
		}
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
		AkSoundEngine.PreGameObjectAPICall(gameObject, akGameObjectID);
		AkSoundEnginePINVOKE.CSharp_AkMIDIPost_PostOnEvent(this.m_Buffer, in_eventID, akGameObjectID, (uint)count);
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
		return (IntPtr)(this.m_Buffer.ToInt64() + (long)(this.SIZE_OF * index));
	}

	private readonly int m_Count;

	private readonly int SIZE_OF = AkSoundEnginePINVOKE.CSharp_AkMIDIPost_GetSizeOf();

	private IntPtr m_Buffer = IntPtr.Zero;
}
