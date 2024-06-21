using System;
using Framework.Managers;
using mail;
using Net;

public class MailNetWork : NetWorkBase
{
    private MailControl mailControl
    {
        get
        {
            return ControllerManager.Instance.GetController<MailControl>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MailList_SC>(CommandID.MSG_Ret_MailList_SC, new ProtoMsgCallback<MSG_Ret_MailList_SC>(this.OnRetMailList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_RefreshMail_SC>(CommandID.MSG_Ret_RefreshMail_SC, new ProtoMsgCallback<MSG_Ret_RefreshMail_SC>(this.OnRetRefreshMail));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_RefreshMailState_SC>(CommandID.MSG_Ret_RefreshMailState_SC, new ProtoMsgCallback<MSG_Ret_RefreshMailState_SC>(this.OnRetRefreshMailState));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GetAttachment_SC>(CommandID.MSG_Ret_GetAttachment_SC, new ProtoMsgCallback<MSG_Ret_GetAttachment_SC>(this.OnRetGetAttachment));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_DeleteMail_SC>(CommandID.MSG_Ret_DeleteMail_SC, new ProtoMsgCallback<MSG_Ret_DeleteMail_SC>(this.OnRetDeleteMail));
    }

    public void OnRetMailList(MSG_Ret_MailList_SC msgb)
    {
        this.mailControl.OnRetMailList(msgb);
    }

    public void OnRetRefreshMail(MSG_Ret_RefreshMail_SC msgb)
    {
        this.mailControl.RefreshMailItem(msgb);
    }

    public void OnRetRefreshMailState(MSG_Ret_RefreshMailState_SC msgb)
    {
        this.mailControl.OnUpdateMailState(msgb);
    }

    public void OnRetGetAttachment(MSG_Ret_GetAttachment_SC msgb)
    {
        this.mailControl.OnGetAttachment(msgb);
    }

    public void OnRetDeleteMail(MSG_Ret_DeleteMail_SC msgb)
    {
        this.mailControl.OnDeleteMail(msgb);
    }

    public void ReqMailList()
    {
        MSG_Req_MailList_CS t = new MSG_Req_MailList_CS();
        base.SendMsg<MSG_Req_MailList_CS>(CommandID.MSG_Req_MailList_CS, t, false);
    }

    public void ReqAllAttachment()
    {
        MSG_Req_GetAllAttachment_CS t = new MSG_Req_GetAllAttachment_CS();
        base.SendMsg<MSG_Req_GetAllAttachment_CS>(CommandID.MSG_Req_GetAllAttachment_CS, t, false);
    }

    public void ReqDeleteAllMail()
    {
        MSG_Req_DeleteAllMail_CS t = new MSG_Req_DeleteAllMail_CS();
        base.SendMsg<MSG_Req_DeleteAllMail_CS>(CommandID.MSG_Req_DeleteAllMail_CS, t, false);
    }

    public void ReqGetAttachment(string mailid)
    {
        base.SendMsg<MSG_Req_GetAttachment_CS>(CommandID.MSG_Req_GetAttachment_CS, new MSG_Req_GetAttachment_CS
        {
            mailid = mailid
        }, false);
    }

    public void ReqDelMail(ulong mailid)
    {
        base.SendMsg<MSG_Req_DeleteMail_CS>(CommandID.MSG_Req_DeleteMail_CS, new MSG_Req_DeleteMail_CS
        {
            mailid = mailid
        }, false);
    }

    public void ReqOpenMail(ulong mailid)
    {
        base.SendMsg<MSG_Req_OpenMail_CS>(CommandID.MSG_Req_OpenMail_CS, new MSG_Req_OpenMail_CS
        {
            mailid = mailid
        }, false);
    }
}
