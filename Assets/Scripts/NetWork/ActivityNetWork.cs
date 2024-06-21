using System;
using Framework.Managers;
using massive;
using Net;

public class ActivityNetWork : NetWorkBase
{
    private ActivityController activityController
    {
        get
        {
            return ControllerManager.Instance.GetController<ActivityController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetVitalityDegree_SC>(2245, new ProtoMsgCallback<MSG_RetVitalityDegree_SC>(this.OnRetVitalityDegree));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetGetVitalityAward_SC>(2247, new ProtoMsgCallback<MSG_RetGetVitalityAward_SC>(this.OnRetGetVitalityAward));
    }

    public void ReqActivityDegree()
    {
        MSG_ReqVitalityDegree_CS t = new MSG_ReqVitalityDegree_CS();
        base.SendMsg<MSG_ReqVitalityDegree_CS>(CommandID.MSG_ReqVitalityDegree_CS, t, false);
        FFDebug.Log(this, FFLogType.Network, "ReqVitalityDegree ------ ");
    }

    private void OnRetVitalityDegree(MSG_RetVitalityDegree_SC message)
    {
        FFDebug.Log(this, FFLogType.Network, string.Format("OnRetVitalityDegree ------ VitItem.Count = {0} ; RewardItem.Count = {1} ", message.info.item.Count, message.info.reward.Count));
        this.activityController.OnRetVitalityDegree(message);
    }

    public void ReqGetActivityAward(uint vitality)
    {
        base.SendMsg<MSG_ReqGetVitalityAward_CS>(CommandID.MSG_ReqGetVitalityAward_CS, new MSG_ReqGetVitalityAward_CS
        {
            degree = vitality
        }, false);
        FFDebug.Log(this, FFLogType.Network, string.Format("ReqGetVitalityAward ------ vitality = {0}", vitality));
    }

    public void OnRetGetVitalityAward(MSG_RetGetVitalityAward_SC message)
    {
        FFDebug.Log(this, FFLogType.Network, string.Format("OnRetGetVitalityAward ------ result = {0} degree={1}", message.errcode, message.degree));
        this.activityController.OnRetGetVitalityAward(message.errcode, message.degree);
    }
}
