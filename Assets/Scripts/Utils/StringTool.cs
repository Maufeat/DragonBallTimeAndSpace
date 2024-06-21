using System;
using System.Text;

public class StringTool
{
    public static StringTool Instance
    {
        get
        {
            if (StringTool.instance == null)
            {
                StringTool.instance = new StringTool();
            }
            return StringTool.instance;
        }
    }

    public int StringCount(string str)
    {
        return Encoding.Default.GetByteCount(str) / 2;
    }

    public bool bolNum(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            byte b = Convert.ToByte(str[i]);
            if (b < 48 || b > 57)
            {
                return false;
            }
        }
        return true;
    }

    private static StringTool instance;
}
