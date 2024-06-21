using System;
using copymap;
using Net;

public class JieWangQuanNetworker : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
    }

    public void ReqOperateData(uint step, bool succ, uint type)
    {
        base.SendMsg<MSG_Req_PlayGameData_CS>(CommandID.MSG_Req_PlayGameData_CS, new MSG_Req_PlayGameData_CS
        {
            step = step,
            success = ((!succ) ? 0U : 1U),
            type = type
        }, false);
    }

    public void ReqGameRetry(uint type)
    {
        base.SendMsg<MSG_Req_PlayGameRetry_CS>(CommandID.MSG_Req_PlayGameRetry_CS, new MSG_Req_PlayGameRetry_CS
        {
            type = type
        }, false);
    }

    public void ReqGameExit()
    {
        MSG_Req_ExitCopymap_SC t = new MSG_Req_ExitCopymap_SC();
        base.SendMsg<MSG_Req_ExitCopymap_SC>(CommandID.MSG_Req_ExitCopymap_SC, t, false);
    }

    public override void UnRegisterMsg()
    {
        base.UnRegisterMsg();
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
    }
}
