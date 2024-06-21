using System;
using System.Text;
using Net;

namespace map
{
    public class StringableStructData : StructData
    {
        public void MarshalString(OctetsStream ot, string content, string charset = null)
        {
            Encoding encoding = Encoding.UTF8;
            if (charset != null)
            {
                encoding = Encoding.GetEncoding(charset);
            }
            int byteCount = encoding.GetByteCount(content);
            ot.marshal_int(byteCount);
            if (byteCount > 0)
            {
                byte[] bytes = encoding.GetBytes(content);
                for (int i = 0; i < byteCount; i++)
                {
                    ot.marshal_byte(bytes[i]);
                }
            }
        }

        public string UnmarshalString(OctetsStream ot, string charset = null)
        {
            string result;
            try
            {
                Encoding encoding = Encoding.UTF8;
                if (charset != null)
                {
                    encoding = Encoding.GetEncoding(charset);
                }
                int num = ot.unmarshal_int();
                string text = string.Empty;
                if (num > 0)
                {
                    byte[] array = new byte[num];
                    for (int i = 0; i < num; i++)
                    {
                        array[i] = ot.unmarshal_byte();
                    }
                    text = encoding.GetString(array);
                }
                result = text;
            }
            catch (Exception str)
            {
                FFDebug.LogError(this, str);
                result = string.Empty;
            }
            return result;
        }
    }
}
