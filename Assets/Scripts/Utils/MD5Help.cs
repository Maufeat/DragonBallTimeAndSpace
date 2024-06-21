using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class MD5Help
{
    public static string GetMD5ByFile(string fileName)
    {
        string result = null;
        if (MD5Help.provider == null)
        {
            MD5Help.provider = new MD5CryptoServiceProvider();
        }
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
        {
            result = BitConverter.ToString(MD5Help.provider.ComputeHash(fileStream)).Replace("-", string.Empty);
        }
        return result;
    }

    public static string GetMD5ByString(string data)
    {
        string text = string.Empty;
        MD5 md = MD5.Create();
        byte[] array = md.ComputeHash(Encoding.UTF8.GetBytes(data));
        for (int i = 0; i < array.Length; i++)
        {
            text += array[i].ToString("x2");
        }
        return text;
    }

    private static MD5CryptoServiceProvider provider;
}
