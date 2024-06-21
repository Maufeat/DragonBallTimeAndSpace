using Net;

public class stUserRequestIPCmd : StructCmd
{
    public stUserRequestIPCmd() : base(104, 15)
    {
    }

    public override OctetsStream WriteStruct(OctetsStream os)
    {
        return os;
    }

    public uint version;
}
