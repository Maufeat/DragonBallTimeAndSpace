using Net;

public class stUserVerifyVerCmd_CS : StructCmd
{
    public stUserVerifyVerCmd_CS() : base(CommandID.stUserVerifyVerCmd_CS)
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
