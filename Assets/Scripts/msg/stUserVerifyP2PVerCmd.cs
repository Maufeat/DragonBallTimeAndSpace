using Net;

public class stUserVerifyP2PVerCmd : StructCmd
{
    public stUserVerifyP2PVerCmd() : base(104, 120)
    {
    }

    public override OctetsStream WriteStruct(OctetsStream os)
    {
        os.marshal_uint(this.reserve);
        os.marshal_uint(this.version);
        return os;
    }

    public uint reserve;

    public uint version;
}
