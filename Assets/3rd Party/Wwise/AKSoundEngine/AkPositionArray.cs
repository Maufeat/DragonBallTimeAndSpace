// dnSpy decompiler from Assembly-CSharp.dll class: AkPositionArray
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class AkPositionArray : IDisposable
{
	public AkPositionArray(uint in_Count)
	{
		this.m_Buffer = Marshal.AllocHGlobal((int)(in_Count * 4u * 9u));
		this.m_Current = this.m_Buffer;
		this.m_MaxCount = in_Count;
		this.Count = 0u;
	}

	public uint Count { get; private set; }

	public void Dispose()
	{
		if (this.m_Buffer != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.m_Buffer);
			this.m_Buffer = IntPtr.Zero;
			this.m_MaxCount = 0u;
		}
	}

	~AkPositionArray()
	{
		this.Dispose();
	}

	public void Reset()
	{
		this.m_Current = this.m_Buffer;
		this.Count = 0u;
	}

	public void Add(Vector3 in_Pos, Vector3 in_Forward, Vector3 in_Top)
	{
		if (this.Count >= this.m_MaxCount)
		{
			throw new IndexOutOfRangeException("Out of range access in AkPositionArray");
		}
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Forward.x), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Forward.y), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Forward.z), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Top.x), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Top.y), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Top.z), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Pos.x), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Pos.y), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		Marshal.WriteInt32(this.m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Pos.z), 0));
		this.m_Current = (IntPtr)(this.m_Current.ToInt64() + 4L);
		this.Count += 1u;
	}

	public IntPtr m_Buffer;

	private IntPtr m_Current;

	private uint m_MaxCount;
}
