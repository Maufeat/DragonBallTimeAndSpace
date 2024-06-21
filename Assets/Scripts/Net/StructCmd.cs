namespace Net
{
    public class StructCmd : NullCmd
    {
        public StructCmd()
        {
        }

        public StructCmd(CommandID id) : base(id)
        {
        }

        public StructCmd(byte cmd, byte para) : base(cmd, para)
        {
        }

        public virtual OctetsStream WriteStruct(OctetsStream os)
        {
            return os;
        }

        public virtual OctetsStream ReadStruct(OctetsStream os)
        {
            return os;
        }
    }
}
