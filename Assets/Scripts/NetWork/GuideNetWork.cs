using System;
using Framework.Managers;
using massive;
using Net;

public class GuideNetWork : NetWorkBase
{
    private GuideController guildController
    {
        get
        {
            return ControllerManager.Instance.GetController<GuideController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuideStart_SC>(2233, new ProtoMsgCallback<MSG_Ret_GuideStart_SC>(this.OnReturnGuide));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuideOver_SC>(2235, new ProtoMsgCallback<MSG_Ret_GuideOver_SC>(this.Ret_GuideOver_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuideHistory_SC>(2232, new ProtoMsgCallback<MSG_Ret_GuideHistory_SC>(this.Ret_GuideHistory_SC));
    }

    private void OnReturnGuide(MSG_Ret_GuideStart_SC msgInfo)
    {
        this.guildController.ViewGuideUI(msgInfo.guideid, false);
    }

    private void Ret_GuideOver_SC(MSG_Ret_GuideOver_SC msgInfo)
    {
        this.guildController.Ret_GuideOver_SC(msgInfo.guideid);
    }

    private void Ret_GuideHistory_SC(MSG_Ret_GuideHistory_SC msgInfo)
    {
        this.guildController.OnHistoryBack(msgInfo.guideids);
    }

    public void Req_StartGuide_CS(uint id)
    {
        base.SendMsg<MSG_Req_GuideStart_CS>(CommandID.MSG_Req_GuideStart_CS, new MSG_Req_GuideStart_CS
        {
            guideid = id * 100U + 1U
        }, false);
        this.guildController.TryCacheGuidedID(id);
    }

    public void Req_GuideOver_CS(uint id)
    {
        base.SendMsg<MSG_Req_GuideOver_CS>(CommandID.MSG_Req_GuideOver_CS, new MSG_Req_GuideOver_CS
        {
            guideid = id
        }, false);
        this.guildController.TryCacheGuidedID(id);
    }
}
