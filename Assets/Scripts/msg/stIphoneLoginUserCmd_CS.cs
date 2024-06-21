using System;
using Net;

public class stIphoneLoginUserCmd_CS : StructCmd
{
    public stIphoneLoginUserCmd_CS() : base(CommandID.stIphoneLoginUserCmd_CS)
    {
    }

    public override OctetsStream WriteStruct(OctetsStream os)
    {
        os.marshal_uint(this.accid);
        os.marshal_ushort(this.user_type);
        os.marshal_uint(this.loginTempID);
        os.marshal_string(this.account, 48);
        os.marshal_string(this.password, 16);
        os.marshal_string(this.szMAC, 24);
        os.marshal_string(this.szFlat, 100);
        os.marshal(this.phone);
        return os;
    }

    public uint accid;

    public ushort user_type;

    public uint loginTempID;

    public string account;

    public string password;

    public string szMAC;

    public string szFlat;

    public stPhoneInfo phone;
}
