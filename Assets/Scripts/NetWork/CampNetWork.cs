using System;
using country;
using Framework.Managers;
using Net;

public class CampNetWork : NetWorkBase
{
    private CampController campController
    {
        get
        {
            return ControllerManager.Instance.GetController<CampController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_JoinCountry_SC>(60046, new ProtoMsgCallback<MSG_Ret_JoinCountry_SC>(this.OnRetJoinCountry));
    }

    public void ReqJoinCountry(uint id)
    {
        FFDebug.Log(this, FFLogType.Network, "ReqJoinCountry " + id);
        base.SendMsg<MSG_Req_JoinCountry_CS>(CommandID.MSG_Req_JoinCountry_CS, new MSG_Req_JoinCountry_CS
        {
            countryid = id
        }, false);
    }

    private void OnRetJoinCountry(MSG_Ret_JoinCountry_SC data)
    {
        FFDebug.Log(this, FFLogType.Network, "OnRetJoinCountry  " + data.retcode);
    }
}
