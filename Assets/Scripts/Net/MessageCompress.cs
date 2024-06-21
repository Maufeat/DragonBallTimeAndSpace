using System;
using System.Runtime.InteropServices;
using Net;

public class MessageCompress
{
    [DllImport("zlib1", CallingConvention = CallingConvention.Cdecl)]
    private static extern int compress(byte[] dest, ref ulong destLen, byte[] source, ulong sourceLen);

    [DllImport("zlib1", CallingConvention = CallingConvention.Cdecl)]
    private static extern int uncompress(byte[] dest, ref ulong destLen, byte[] source, ulong sourceLen);

    [DllImport("zlib1", CallingConvention = CallingConvention.Cdecl)]
    private static extern int compressBound(ulong sourceLen);

    private static int Compress(byte[] dest, ref ulong destLen, byte[] source, ulong sourceLen)
    {
        return MessageCompress.compress(dest, ref destLen, source, sourceLen);
    }

    private static int CompressBound(ulong sourceLen)
    {
        return MessageCompress.compressBound(sourceLen);
    }

    private static int Uncompress(byte[] dest, ref ulong destLen, byte[] source, ulong sourceLen)
    {
        return MessageCompress.uncompress(dest, ref destLen, source, sourceLen);
    }

    public static bool MsgCompress(int offset, int length, ref OctetsStream os)
    {
        ulong num = (ulong)((long)MessageCompress.CompressBound((ulong)((long)length)));
        MessageCompress.compressSourceDatabuffer.Length = length;
        Array.Copy(os.buffer(), offset, MessageCompress.compressSourceDatabuffer.Buffer, 0, length);
        int num2 = MessageCompress.Compress(MessageCompress.compressedDatabuffer.Buffer, ref num, MessageCompress.compressSourceDatabuffer.Buffer, (ulong)((long)length));
        if (num2 != 0)
        {
            FFDebug.LogWarning("MessageCompress", "Compress failed error code: " + num2);
            return false;
        }
        MessageCompress.compressedDatabuffer.Length = (int)num;
        os.erase(offset, os.size());
        os.insert(offset, MessageCompress.compressedDatabuffer.Buffer, 0, MessageCompress.compressedDatabuffer.Length);
        return true;
    }

    public static bool DeCompress(int offset, ref OctetsStream os)
    {
        ulong num = (ulong)((long)MessageCompress.decompressedDatabuffer.Buffer.Length);
        int num2 = MessageCompress.Uncompress(MessageCompress.decompressedDatabuffer.Buffer, ref num, os.buffer(), (ulong)((long)os.size()));
        if (num2 != 0)
        {
            FFDebug.LogWarning("MessageCompress", "DeCompress failed error code: " + num2);
            return false;
        }
        os.clear();
        os.insert(offset, MessageCompress.decompressedDatabuffer.Buffer);
        return true;
    }

    public static bool IsCompress(OctetsBuffer buffer)
    {
        if (buffer.size() < 4)
        {
            return false;
        }
        uint num = (uint)((int)buffer.getByte(0) + ((int)buffer.getByte(1) << 8) + ((int)buffer.getByte(2) << 16) + ((int)buffer.getByte(3) << 24));
        uint num2 = 1073741824U;
        return num2 == (num2 & num);
    }

    private static DataBuffer compressSourceDatabuffer = new DataBuffer(65536);

    private static DataBuffer compressedDatabuffer = new DataBuffer(65536);

    private static DataBuffer decompressedDatabuffer = new DataBuffer(65536);
}
