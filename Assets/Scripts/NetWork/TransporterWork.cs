using System;
using msg;
using Net;

public class TransporterWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
    }

    public void ReqMSG_Req_TELE_PORT_CS(ulong tid)
    {
        base.SendMsg<MSG_Req_TELE_PORT_CS>(CommandID.MSG_Req_TELE_PORT_CS, new MSG_Req_TELE_PORT_CS
        {
            teleportid = tid
        }, false);
    }
}
