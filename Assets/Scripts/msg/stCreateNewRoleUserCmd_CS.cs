using System;
using Net;

public class stCreateNewRoleUserCmd_CS : StructCmd
{
    public stCreateNewRoleUserCmd_CS() : base(CommandID.stCreateNewRoleUserCmd_CS)
    {
    }

    public override OctetsStream WriteStruct(OctetsStream os)
    {
        os.marshal_string(this.strRoleName, 32);
        os.marshal_byte(this.bySex);
        os.marshal_uint(this.flatid);
        os.marshal(this.phone);
        os.marshal_uint(this.heroid);
        return os;
    }

    public string strRoleName;

    public byte bySex;

    public uint flatid;

    public stPhoneInfo phone;

    public uint heroid;
}
