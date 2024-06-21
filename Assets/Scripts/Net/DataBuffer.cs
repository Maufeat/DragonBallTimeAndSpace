using System;

public class DataBuffer
{
    public DataBuffer(int size)
    {
        this.Buffer = new byte[size];
    }

    public void Clear()
    {
        this.Length = 0;
        for (int i = 0; i < this.Buffer.Length; i++)
        {
            this.Buffer[i] = 0;
        }
    }

    public byte[] Buffer;

    public int Length;
}
