using System;
using Net;

public class stServerP2PReturnLoginFailedCmd : StructCmd
{
    public override OctetsStream ReadStruct(OctetsStream os)
    {
        this.returnCode = os.unmarshal_sbyte();
        this.size = os.unmarshal_ushort();
        if (this.size > 0)
        {
            this.error = os.unmarshal_String((int)this.size);
        }
        return os;
    }

    public sbyte returnCode;

    public ushort size;

    public string error;
}
