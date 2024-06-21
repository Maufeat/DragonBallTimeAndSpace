using System;
using Net;

public class stIphoneUserRequestLoginCmd : StructCmd
{
    public stIphoneUserRequestLoginCmd() : base(104, 1)
    {
    }

    public override OctetsStream WriteStruct(OctetsStream os)
    {
        os.marshal_ushort(this.userType);
        os.marshal_string(this.account, 48);
        os.marshal_string(string.Empty, 33);
        os.marshal_ushort(this.game);
        os.marshal_ushort(this.zone);
        os.marshal_ushort(this.wdNetType);
        os.marshal_ulong(this.uid);
        os.marshal_string(this.token, 32);
        os.marshal_string(this.phone_uuid, 32);
        return os;
    }

    public ushort userType;

    public string account;

    public string pstrPassword;

    public ushort game;

    public ushort zone;

    public ushort wdNetType;

    public ulong uid;

    public string token;

    public string phone_uuid;
}
