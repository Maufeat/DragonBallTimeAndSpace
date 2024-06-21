// dnSpy decompiler from Assembly-CSharp.dll class: AkBaseArray`1
using System;
using System.Runtime.InteropServices;

public abstract class AkBaseArray<T> : IDisposable
{
	public AkBaseArray(int capacity)
	{
		this.m_Buffer = Marshal.AllocHGlobal(capacity * this.StructureSize);
		if (this.m_Buffer != IntPtr.Zero)
		{
			this.Capacity = capacity;
			for (int i = 0; i < capacity; i++)
			{
				this.ClearAtIntPtr(this.GetObjectPtr(i));
			}
		}
	}

	public void Dispose()
	{
		if (this.m_Buffer != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.m_Buffer);
			this.m_Buffer = IntPtr.Zero;
			this.Capacity = 0;
		}
	}

	~AkBaseArray()
	{
		this.Dispose();
	}

	public int Capacity { get; private set; }

	public virtual int Count()
	{
		return this.Capacity;
	}

	protected abstract int StructureSize { get; }

	protected virtual void ClearAtIntPtr(IntPtr address)
	{
	}

	protected abstract T CreateNewReferenceFromIntPtr(IntPtr address);

	protected abstract void CloneIntoReferenceFromIntPtr(IntPtr address, T other);

	public T this[int index]
	{
		get
		{
			return this.CreateNewReferenceFromIntPtr(this.GetObjectPtr(index));
		}
		set
		{
			this.CloneIntoReferenceFromIntPtr(this.GetObjectPtr(index), value);
		}
	}

	public IntPtr GetBuffer()
	{
		return this.m_Buffer;
	}

	protected IntPtr GetObjectPtr(int index)
	{
		if (index >= this.Capacity)
		{
			throw new IndexOutOfRangeException("Out of range access in " + base.GetType().Name);
		}
		return (IntPtr)(this.m_Buffer.ToInt64() + (long)(this.StructureSize * index));
	}

	private IntPtr m_Buffer;
}
