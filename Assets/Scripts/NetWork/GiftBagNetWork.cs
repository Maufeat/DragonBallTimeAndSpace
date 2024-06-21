using System;
using Framework.Managers;
using massive;
using Net;

public class GiftBagNetWork : NetWorkBase
{
    private GiftBagController GiftBagController
    {
        get
        {
            return ControllerManager.Instance.GetController<GiftBagController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_SuccessOpenGift_SC>(2243, new ProtoMsgCallback<MSG_Ret_SuccessOpenGift_SC>(this.RetGiftBag));
    }

    public void RetGiftBag(MSG_Ret_SuccessOpenGift_SC msgInfo)
    {
        this.GiftBagController.RetGiftBag(msgInfo);
    }
}
