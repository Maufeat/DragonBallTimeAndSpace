using System;
using System.Runtime.InteropServices;
using Net;

public class MessageEncrypt
{
    [DllImport("CEncryptLib", CallingConvention = CallingConvention.Cdecl)]
    private static extern void setEncMethod(int method);

    [DllImport("CEncryptLib", CallingConvention = CallingConvention.Cdecl)]
    private static extern void set_key_des(byte[] key);

    [DllImport("CEncryptLib", CallingConvention = CallingConvention.Cdecl)]
    private static extern void set_key_rc5(byte[] key, int len, int rounds);

    [DllImport("CEncryptLib", CallingConvention = CallingConvention.Cdecl)]
    private static extern int encdec(byte[] data, uint index, uint nLen, bool enc);

    [DllImport("CEncryptLib", CallingConvention = CallingConvention.Cdecl)]
    private static extern int encryptStr(byte[] data, byte[] clientdata, int len);

    public static void InitEncrypt()
    {
        MessageEncrypt.setEncMethod(1);
        NullCmd.IS_COMPRESS = true;
        byte[] key_des = new byte[]
        {
            97,
            97,
            97,
            97,
            97,
            97,
            97,
            97
        };
        MessageEncrypt.set_key_des(key_des);
    }

    public static void InitFirEncrypt()
    {
        MessageEncrypt.setEncMethod(2);
        NullCmd.IS_COMPRESS = false;
        byte[] key = new byte[]
        {
            63,
            121,
            213,
            226,
            74,
            140,
            182,
            193,
            175,
            49,
            94,
            199,
            235,
            157,
            110,
            203
        };
        MessageEncrypt.set_key_rc5(key, 16, 12);
    }

    public static void DESEncrypt(int offset, int length, ref OctetsStream os)
    {
        MessageEncrypt.encdec(os.buffer(), (uint)offset, (uint)length, true);
    }

    public static void DESDencrypt(byte[] data, int index, int nLen, bool enc)
    {
        MessageEncrypt.encdec(data, (uint)index, (uint)nLen, false);
    }

    public static void EncryptStr(byte[] data, byte[] clientData, int len)
    {
        MessageEncrypt.encryptStr(data, clientData, len);
    }

    public static int FillZero(int offset, ref OctetsStream os)
    {
        int num = (os.size() - offset) % 8;
        if (num != 0)
        {
            for (int i = 0; i < 8 - num; i++)
            {
                os.push_back(0);
            }
        }
        if (num == 0)
        {
            return 0;
        }
        return 8 - num;
    }
}
