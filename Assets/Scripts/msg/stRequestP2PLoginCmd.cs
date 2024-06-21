using System;
using Net;

public class stRequestP2PLoginCmd : StructCmd
{
    public stRequestP2PLoginCmd() : base(104, 2)
    {
    }

    public override OctetsStream WriteStruct(OctetsStream os)
    {
        os.marshal_string(this.pstrName, 48);
        os.insert(os.size(), this.pstrPassword);
        os.marshal_ushort(this.game);
        os.marshal_ushort(this.zone);
        os.marshal_string(this.jpegPassport, 7);
        os.marshal_string(this.maxAddr, 13);
        os.insert(os.size(), this.uuid);
        os.marshal_ushort(this.wdNetType);
        os.marshal_string(this.passpodPwd, 9);
        os.marshal_ushort(this.userType);
        return os;
    }

    public string pstrName;

    public byte[] pstrPassword;

    public ushort game;

    public ushort zone;

    public string jpegPassport;

    public string maxAddr;

    public byte[] uuid;

    public ushort wdNetType;

    public string passpodPwd;

    public ushort userType;
}
