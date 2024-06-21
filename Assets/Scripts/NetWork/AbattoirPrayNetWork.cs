using System;
using Framework.Managers;
using mobapk;
using Net;

public class AbattoirPrayNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_StartPray_SC>(2581, new ProtoMsgCallback<MSG_StartPray_SC>(this.OnStartPray));
    }

    private void OnStartPray(MSG_StartPray_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirPrayController>().OnStartPray(MsgData);
    }

    private object ToEmty(object obj)
    {
        return string.Empty;
    }

    private object ToJson(object obj, string from = "")
    {
        return obj;
    }

    public void SendPray(uint lv1Index, uint lv2Index, uint lv3Index)
    {
        base.SendMsg<MSG_SelectHopes_CS>(CommandID.MSG_SelectHopes_CS, new MSG_SelectHopes_CS
        {
            idx_1st = lv1Index,
            idx_2nd = lv2Index,
            idx_3rd = lv3Index
        }, false);
    }
}
