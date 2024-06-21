namespace Net
{
    public class NullCmd
    {
        public static bool IS_COMPRESS;
        public const int HEAD_SIZE = 4;
        public const bool IS_ENCRYPT = false;
        public ushort Msgid;
        public byte MsgCmd;
        public byte MsgParam;
        public object Data;
        public byte[] BufferData;

        public NullCmd()
        {
        }

        public NullCmd(CommandID id)
        {
            this.Msgid = (ushort)id;
        }

        public NullCmd(byte cmd, byte param)
        {
            this.MsgCmd = cmd;
            this.MsgParam = param;
        }

        public static CmdHead unmarshal_head(ref OctetsStream os)
        {
            CmdHead cmdHead = new CmdHead();
            int num = os.position();
            cmdHead.MsgLength = (int)os.unmarshal_byte() + ((int)os.unmarshal_byte() << 8);
            uint num2 = (uint)((int)os.getByte(num) + ((int)os.getByte(num + 1) << 8) + ((int)os.getByte(num + 2) << 16) + ((int)os.getByte(num + 3) << 24));
            os.unmarshal_ushort();
            int num3 = 1073741824;
            if (num3 == (num3 & (int)num2))
            {
                cmdHead.IsCompress = true;
            }
            else
            {
                cmdHead.IsCompress = false;
            }
            return cmdHead;
        }
    }
}
