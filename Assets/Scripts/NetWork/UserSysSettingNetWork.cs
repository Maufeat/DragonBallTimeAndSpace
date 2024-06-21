using System;
using Framework.Managers;
using massive;
using Net;

public class UserSysSettingNetWork : NetWorkBase
{
    private UserSysSettingController syssettingController
    {
        get
        {
            return ControllerManager.Instance.GetController<UserSysSettingController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetUserSysSetting_SC>(2241, new ProtoMsgCallback<MSG_RetUserSysSetting_SC>(this.OnRetUserSysSetting));
    }

    public void ReqUserSysSetting(SYSSETTING type, bool state)
    {
        base.SendMsg<MSG_ReqUserSysSetting_CS>(CommandID.MSG_ReqUserSysSetting_CS, new MSG_ReqUserSysSetting_CS
        {
            id = (uint)type,
            set = state
        }, false);
    }

    private void OnRetUserSysSetting(MSG_RetUserSysSetting_SC data)
    {
        this.syssettingController.WriteSysSettingData(data.setting);
    }
}
