using System;
using Framework.Managers;
using msg;
using Net;

public class SecondPwdNetWork : NetWorkBase
{
    private SecondPwdControl secondPwdController
    {
        get
        {
            return ControllerManager.Instance.GetController<SecondPwdControl>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ACCOUNT_SEC_PASSWD_SC>(2322, new ProtoMsgCallback<MSG_ACCOUNT_SEC_PASSWD_SC>(this.ON_ACCOUNT_SEC_PASSWD_SC));
    }

    public void ON_ACCOUNT_SEC_PASSWD_SC(MSG_ACCOUNT_SEC_PASSWD_SC msgb)
    {
        SecondPwdControl secondPwdController = this.secondPwdController;
        secondPwdController.PlayerSecondPwd = msgb.sec_passwd;
        if (!string.IsNullOrEmpty(msgb.sec_passwd))
        {
            secondPwdController.CloseUI();
        }
        if (!msgb.isonline)
        {
            TipsWindow.ShowWindow(3118U);
        }
    }

    public void ReqSetSecondPwd(string pwd, string oldPwd)
    {
        base.SendMsg<MSG_USER_REQ_SETPASSWD_CS>(CommandID.MSG_USER_REQ_SETPASSWD_CS, new MSG_USER_REQ_SETPASSWD_CS
        {
            old_passwd = oldPwd,
            new_passwd = pwd
        }, false);
    }
}
