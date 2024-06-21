using System;
using avatar;
using Framework.Managers;
using Net;

public class FashionNetWorker : NetWorkBase
{
    private FashionController fashionController
    {
        get
        {
            return ControllerManager.Instance.GetController<FashionController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetUserAvatars_SC>(CommandID.MSG_RetUserAvatars_SC, new ProtoMsgCallback<MSG_RetUserAvatars_SC>(this.OnRetAllFasionInfo));
    }

    public override void UnRegisterMsg()
    {
        base.UnRegisterMsg();
        LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2530);
    }

    public void OnRetAllFasionInfo(MSG_RetUserAvatars_SC msg)
    {
        this.fashionController.AllFasion = msg;
    }

    public void ReqEquipFasion(uint id)
    {
        base.SendMsg<MSG_ReqEquipAvatar_CS>(CommandID.MSG_ReqEquipAvatar_CS, new MSG_ReqEquipAvatar_CS
        {
            avatarId = id
        }, false);
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
    }
}
