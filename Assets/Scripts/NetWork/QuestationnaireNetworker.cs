using System;
using Framework.Managers;
using massive;
using Net;

public class QuestationnaireNetworker : NetWorkBase
{
    private QuestationnaireController mController
    {
        get
        {
            return ControllerManager.Instance.GetController<QuestationnaireController>();
        }
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_Survey_Info_SC>(CommandID.MSG_Ret_Survey_Info_SC, new ProtoMsgCallback<MSG_Ret_Survey_Info_SC>(this.ReqQuesListCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GetSurveyReward_SC>(CommandID.MSG_Ret_GetSurveyReward_SC, new ProtoMsgCallback<MSG_Ret_GetSurveyReward_SC>(this.ReqSurveyRewardCb));
        base.RegisterMsg();
    }

    public void ReqQuesList()
    {
        MSG_Req_Survey_Info_CS t = new MSG_Req_Survey_Info_CS();
        base.SendMsg<MSG_Req_Survey_Info_CS>(CommandID.MSG_Req_Survey_Info_CS, t, false);
    }

    private void ReqQuesListCb(MSG_Ret_Survey_Info_SC data)
    {
        if (this.mController != null)
        {
            this.mController.ReqQuesListCb(data.survey_id);
        }
    }

    public void ReqSurveyReward(uint quesid)
    {
        base.SendMsg<MSG_Req_GetSurveyReward_CS>(CommandID.MSG_Req_GetSurveyReward_CS, new MSG_Req_GetSurveyReward_CS
        {
            survey_id = quesid
        }, false);
    }

    private void ReqSurveyRewardCb(MSG_Ret_GetSurveyReward_SC data)
    {
        if (this.mController != null)
        {
            this.mController.ReqSurveyRewardCb(data.survey_id, data.retcode);
        }
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
    }

    public override void UnRegisterMsg()
    {
        base.UnRegisterMsg();
    }
}
