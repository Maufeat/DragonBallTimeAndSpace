using System;
using Framework.Managers;
using magic;
using msg;
using Net;

public class FightModelNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_SwitchPKMode_SC>(2215, new ProtoMsgCallback<MSG_Ret_SwitchPKMode_SC>(this.SwitchPKMode_SC));
    }

    public void SwitchPKMode_SC(MSG_Ret_SwitchPKMode_SC model)
    {
        MainPlayer.Self.MainPlayeData.RefreshPKModel((uint)model.newmode);
        ControllerManager.Instance.GetController<UIHpSystem>().ResetPKModel();
    }

    public void ReqSwitchPKMode_CS(PKMode mode)
    {
    }
}
