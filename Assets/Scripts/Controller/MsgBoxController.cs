using System;
using Framework.Managers;
using Models;

public class MsgBoxController : ControllerBase
{
    public UI_MsgBox msgBox
    {
        get
        {
            return UIManager.GetUIObject<UI_MsgBox>();
        }
    }

    public void ShowMsgBox(string s_title, string s_describle, string s_confirm, string s_cancel, UIManager.ParentType parentType, Action confirm, Action cancel, Action close = null)
    {
        s_describle = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(s_describle);
        if (this.msgBox == null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_MsgBox>("UI_Confirm", delegate ()
            {
                if (this.msgBox != null)
                {
                    this.msgBox.ShowMsgBox(s_title, this.GetEnTitle(s_title), s_describle, s_confirm, s_cancel, confirm, cancel, close);
                }
            }, parentType, false);
        }
        else
        {
            this.msgBox.ShowMsgBox(s_title, this.GetEnTitle(s_title), s_describle, s_confirm, s_cancel, confirm, cancel, close);
        }
    }

    public void ShowMsgBox(string s_title, string s_describle, string s_confirm, UIManager.ParentType parentType, Action confirm, Action close = null)
    {
        s_describle = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(s_describle);
        if (this.msgBox == null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_MsgBox>("UI_Confirm", delegate ()
            {
                if (this.msgBox != null)
                {
                    this.msgBox.ShowMsgBox(s_title, this.GetEnTitle(s_title), s_describle, s_confirm, confirm, close);
                }
            }, parentType, false);
        }
        else if (this.msgBox != null)
        {
            this.msgBox.ShowMsgBox(s_title, this.GetEnTitle(s_title), s_describle, s_confirm, confirm, close);
        }
    }

    public void CloseMsgBox()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Confirm");
    }

    public override void Awake()
    {
        MsgBoxController.MsgLevelNormal = CommonUtil.GetText(dynamic_textid.BaseIDs.prompt);
        MsgBoxController.MsgLevelSystem = CommonUtil.GetText(dynamic_textid.BaseIDs.system_prompt);
        MsgBoxController.MsgLevelSystemNotification = CommonUtil.GetText(dynamic_textid.BaseIDs.system_notification);
        MsgBoxController.MsgLevelGetway = CommonUtil.GetText(dynamic_textid.BaseIDs.server_getway);
        MsgBoxController.MsgLevelENNormal = CommonUtil.GetText(dynamic_textid.BaseIDs.prompt_en);
        MsgBoxController.MsgLevelENSystem = CommonUtil.GetText(dynamic_textid.BaseIDs.system_prompt_en);
        MsgBoxController.MsgOptionConfirm = CommonUtil.GetText(dynamic_textid.BaseIDs.confirm);
        MsgBoxController.MsgOptionCancel = CommonUtil.GetText(dynamic_textid.BaseIDs.cancel);
        MsgBoxController.MsgOptionCreate = CommonUtil.GetText(dynamic_textid.BaseIDs.create);
        MsgBoxController.MsgOptionReconnect = CommonUtil.GetText(dynamic_textid.BaseIDs.reconnect);
        MsgBoxController.MsgOptionRetry = CommonUtil.GetText(dynamic_textid.BaseIDs.retry);
        MsgBoxController.MsgOptionLeave = CommonUtil.GetText(dynamic_textid.BaseIDs.leave);
        MsgBoxController.MsgOptionYes = CommonUtil.GetText(dynamic_textid.BaseIDs.yes);
        MsgBoxController.MsgOptionNo = CommonUtil.GetText(dynamic_textid.BaseIDs.no);
    }

    public override void OnUpdate()
    {
    }

    private string GetEnTitle(string title)
    {
        if (title == MsgBoxController.MsgLevelNormal)
        {
            return MsgBoxController.MsgLevelENNormal;
        }
        if (title == MsgBoxController.MsgLevelSystem || title == MsgBoxController.MsgLevelSystemNotification || title == MsgBoxController.MsgLevelGetway)
        {
            return MsgBoxController.MsgLevelENSystem;
        }
        return MsgBoxController.MsgLevelENNormal;
    }

    public override string ControllerName
    {
        get
        {
            return "msgbox";
        }
    }

    public static string MsgLevelNormal;

    public static string MsgLevelSystem;

    public static string MsgLevelSystemNotification;

    public static string MsgLevelGetway;

    private static string MsgLevelENNormal;

    private static string MsgLevelENSystem;

    private static string MsgLevelENGetway;

    public static string MsgOptionConfirm;

    public static string MsgOptionCreate;

    public static string MsgOptionCancel;

    public static string MsgOptionReconnect;

    public static string MsgOptionRetry;

    public static string MsgOptionLeave;

    public static string MsgOptionYes;

    public static string MsgOptionNo;

    public bool starton;
}
