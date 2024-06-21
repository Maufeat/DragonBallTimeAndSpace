using System;
using Framework.Managers;
using Net;
using npc;

public class PickDropNetWork : NetWorkBase
{
    private PickDropController pickDropController
    {
        get
        {
            return ControllerManager.Instance.GetController<PickDropController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RefreshSceneBag_SC>(2336, new ProtoMsgCallback<MSG_RefreshSceneBag_SC>(this.OnRetMSG_RefreshSceneBag_SC));
    }

    public void MSG_ReqWatchSceneBag_CS(ulong npcId)
    {
        base.SendMsg<MSG_ReqWatchSceneBag_CS>(CommandID.MSG_ReqWatchSceneBag_CS, new MSG_ReqWatchSceneBag_CS
        {
            tempid = npcId
        }, false);
    }

    public void MSG_ReqPickObjFromSceneBag_CS(ulong npcId, params uint[] itemIds)
    {
        MSG_ReqPickObjFromSceneBag_CS msg_ReqPickObjFromSceneBag_CS = new MSG_ReqPickObjFromSceneBag_CS();
        msg_ReqPickObjFromSceneBag_CS.tempid = npcId;
        for (int i = 0; i < itemIds.Length; i++)
        {
            msg_ReqPickObjFromSceneBag_CS.thisids.Add(itemIds[i]);
        }
        base.SendMsg<MSG_ReqPickObjFromSceneBag_CS>(CommandID.MSG_ReqPickObjFromSceneBag_CS, msg_ReqPickObjFromSceneBag_CS, false);
    }

    public void MSG_ReqPickAllSceneBag_CS(ulong npcId)
    {
        base.SendMsg<MSG_ReqPickAllSceneBag_CS>(CommandID.MSG_ReqPickAllSceneBag_CS, new MSG_ReqPickAllSceneBag_CS
        {
            tempid = npcId
        }, false);
    }

    private void OnRetMSG_RefreshSceneBag_SC(MSG_RefreshSceneBag_SC msg)
    {
        this.pickDropController.OnRetMSG_RefreshSceneBag_SC(msg);
    }

    public Action<bool> OnHoldOnFinish;
}
