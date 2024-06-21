using System;
using Net;

public class stServerReturnLoginFailedCmd_SC : StructCmd
{
    public override OctetsStream ReadStruct(OctetsStream os)
    {
        this.byReturnCode = os.unmarshal_byte();
        return os;
    }

    public byte byReturnCode;
}
