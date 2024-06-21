using System;
using Net;

public class stRetErrorOperationUserCmd_SC : StructCmd
{
    public override OctetsStream ReadStruct(OctetsStream os)
    {
        this.cmd_id = os.unmarshal_uint();
        return os;
    }

    public uint cmd_id;
}
