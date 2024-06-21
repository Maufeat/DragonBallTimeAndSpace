using System;
using copymap;
using Net;
using quest;

public class YuanQiDanNetwork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
    }

    public void CommitYQDData_CS(int step, bool succ, uint type)
    {
        base.SendMsg<MSG_Req_CommitYQDData_CS>(CommandID.MSG_Req_CommitYQDData_CS, new MSG_Req_CommitYQDData_CS
        {
            step = (uint)step,
            success = ((!succ) ? 0U : 1U),
            type = type
        }, false);
    }

    public void Req_PlayYQDRetry_CS(uint type)
    {
        base.SendMsg<MSG_Req_PlayYQDRetry_CS>(CommandID.MSG_Req_PlayYQDRetry_CS, new MSG_Req_PlayYQDRetry_CS
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
