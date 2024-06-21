using System;
using copymap;
using Framework.Managers;
using Net;
using rankpk_msg;

public class PVPCompetitionNetWork : NetWorkBase
{
    private PVPCompetitionController controller
    {
        get
        {
            if (this._controller == null)
            {
                this._controller = ControllerManager.Instance.GetController<PVPCompetitionController>();
            }
            return this._controller;
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetFightCountDown_SC>(CommandID.MSG_RetFightCountDown_SC, new ProtoMsgCallback<MSG_RetFightCountDown_SC>(this.RetFightCountDown_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetStartFight_SC>(CommandID.MSG_RetStartFight_SC, new ProtoMsgCallback<MSG_RetStartFight_SC>(this.RetStartFight_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetSpeedupFight_SC>(CommandID.MSG_RetSpeedupFight_SC, new ProtoMsgCallback<MSG_RetSpeedupFight_SC>(this.RetSpeedupFight_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetFightFinish_SC>(CommandID.MSG_RetFightFinish_SC, new ProtoMsgCallback<MSG_RetFightFinish_SC>(this.RetFightFinish_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetTeamLeftMemSize_SC>(CommandID.MSG_RetTeamLeftMemSize_SC, new ProtoMsgCallback<MSG_RetTeamLeftMemSize_SC>(this.RetTeamLeftMemSize_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetRankPKCurStage_SC>(CommandID.MSG_RetRankPKCurStage_SC, new ProtoMsgCallback<MSG_RetRankPKCurStage_SC>(this.RetRankPKCurStage_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetPKGeneralConfig_SC>(CommandID.MSG_RetPKGeneralConfig_SC, new ProtoMsgCallback<MSG_RetPKGeneralConfig_SC>(this.RetPKGeneralConfig_SC));
    }

    public void ReqRankPKCurStage_CS()
    {
        MSG_ReqRankPKCurStage_CS t = new MSG_ReqRankPKCurStage_CS();
        base.SendMsg<MSG_ReqRankPKCurStage_CS>(CommandID.MSG_ReqRankPKCurStage_CS, t, false);
    }

    public void Req_ExitCopymap_SC()
    {
        MSG_Req_ExitCopymap_SC t = new MSG_Req_ExitCopymap_SC();
        base.SendMsg<MSG_Req_ExitCopymap_SC>(CommandID.MSG_Req_ExitCopymap_SC, t, false);
    }

    private void RetFightCountDown_SC(MSG_RetFightCountDown_SC msg)
    {
        this.controller.RetFightCountDown_SC(msg.duration);
    }

    private void RetStartFight_SC(MSG_RetStartFight_SC msg)
    {
        this.controller.RetStartFight_SC(msg.duration, msg.score);
    }

    private void RetSpeedupFight_SC(MSG_RetSpeedupFight_SC msg)
    {
        this.controller.RetSpeedupFight_SC(msg.duration);
    }

    private void RetFightFinish_SC(MSG_RetFightFinish_SC msg)
    {
        this.controller.RetFightFinish_SC(msg);
    }

    private void RetTeamLeftMemSize_SC(MSG_RetTeamLeftMemSize_SC msg)
    {
        this.controller.RetTeamLeftMemSize_SC(msg);
    }

    private void RetRankPKCurStage_SC(MSG_RetRankPKCurStage_SC msg)
    {
        this.controller.RetRankPKCurStage_SC(msg);
    }

    private void RetPKGeneralConfig_SC(MSG_RetPKGeneralConfig_SC msg)
    {
        this.controller.RetPKGeneralConfig_SC(msg);
    }

    private PVPCompetitionController _controller;
}
