using System;
using battle;
using copymap;
using Framework.Managers;
using Net;

public class DuoQiNetworker : NetWorkBase
{
    private DuoQiController mController
    {
        get
        {
            return ControllerManager.Instance.GetController<DuoQiController>();
        }
    }

    private UIMapController mMapController
    {
        get
        {
            return ControllerManager.Instance.GetController<UIMapController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetHoldFlagReport_SC>(CommandID.MSG_RetHoldFlagReport_SC, new ProtoMsgCallback<MSG_RetHoldFlagReport_SC>(this.RetHoldFlagReport));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetHoldFlagTeamScore_SC>(CommandID.MSG_RetHoldFlagTeamScore_SC, new ProtoMsgCallback<MSG_RetHoldFlagTeamScore_SC>(this.RetHoldFlagTeamScore));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetHoldFlagDBState_SC>(CommandID.MSG_RetHoldFlagDBState_SC, new ProtoMsgCallback<MSG_RetHoldFlagDBState_SC>(this.RetHoldFlagDBState));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetHoldFlagCountDown_SC>(CommandID.MSG_RetHoldFlagCountDown_SC, new ProtoMsgCallback<MSG_RetHoldFlagCountDown_SC>(this.RetHoldFlagCountDown));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetHoldFlagAccount_SC>(CommandID.MSG_RetHoldFlagAccount_SC, new ProtoMsgCallback<MSG_RetHoldFlagAccount_SC>(this.RetHoldFlagAccount));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetHoldFlagEvent_SC>(CommandID.MSG_RetHoldFlagEvent_SC, new ProtoMsgCallback<MSG_RetHoldFlagEvent_SC>(this.RetHoldFlagEvent));
    }

    private void RetHoldFlagEvent(MSG_RetHoldFlagEvent_SC data)
    {
        string arg = string.Empty;
        if (this.mController.IsSameCamp(data.camp))
        {
            arg = "<color=#14A7FFFF>" + data.userName + "</color>";
        }
        else
        {
            arg = "<color=#ff0000>" + data.userName + "</color>";
        }
        string text = string.Format(CommonTools.GetTextById((ulong)data.tipId), arg);
        TipsWindow.ShowWindow(text);
        ControllerManager.Instance.GetController<ChatControl>().AddChatItemBySystem(text);
    }

    private void RetHoldFlagAccount(MSG_RetHoldFlagAccount_SC data)
    {
        this.mController.OpenAccountPanel(data.winCampId, data.reports);
    }

    public void ReqHoldFlagCaptureDB(ulong npcid)
    {
        base.SendMsg<MSG_ReqHoldFlagCaptureDB_CS>(CommandID.MSG_ReqHoldFlagCaptureDB_CS, new MSG_ReqHoldFlagCaptureDB_CS
        {
            npcid = npcid
        }, false);
    }

    public void ReqHoldFlagPutDownDB()
    {
        MSG_ReqHoldFlagPutDownDB_CS t = new MSG_ReqHoldFlagPutDownDB_CS();
        base.SendMsg<MSG_ReqHoldFlagPutDownDB_CS>(CommandID.MSG_ReqHoldFlagPutDownDB_CS, t, false);
    }

    public void ReqHoldFlagReport()
    {
        MSG_ReqHoldFlagReport_CS t = new MSG_ReqHoldFlagReport_CS();
        base.SendMsg<MSG_ReqHoldFlagReport_CS>(CommandID.MSG_ReqHoldFlagReport_CS, t, false);
    }

    private void RetHoldFlagReport(MSG_RetHoldFlagReport_SC data)
    {
        this.mController.RetHoldFlagReport(data.reports);
    }

    private void RetHoldFlagTeamScore(MSG_RetHoldFlagTeamScore_SC data)
    {
        this.mMapController.RetHoldFlagTeamScore(data.campScore);
    }

    private void RetHoldFlagDBState(MSG_RetHoldFlagDBState_SC data)
    {
        this.mMapController.RetHoldFlagDBState(data.DBStates);
    }

    private void RetHoldFlagCountDown(MSG_RetHoldFlagCountDown_SC data)
    {
        this.mMapController.RetHoldFlagCountDown(data.endTimeStamp);
    }

    public void ReqExitCopy()
    {
        MSG_Req_ExitCopymap_SC t = new MSG_Req_ExitCopymap_SC();
        base.SendMsg<MSG_Req_ExitCopymap_SC>(CommandID.MSG_Req_ExitCopymap_SC, t, false);
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
