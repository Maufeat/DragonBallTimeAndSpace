using System;
using Framework.Managers;
using mail;
using Models;

internal class MailControl : ControllerBase
{
    public UI_Mail UIMail
    {
        get
        {
            return UIManager.GetUIObject<UI_Mail>();
        }
    }

    public MSG_Ret_MailList_SC MailList
    {
        get
        {
            return this._mailList;
        }
    }

    public override void Awake()
    {
        this.mailNetWork = new MailNetWork();
        this.mailNetWork.Initialize();
    }

    public override string ControllerName
    {
        get
        {
            return "mail_controller";
        }
    }

    public void ShowMailUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Mail>("UI_Mail", null, UIManager.ParentType.CommonUI, false);
    }

    public void OnRetMailList(MSG_Ret_MailList_SC list)
    {
        this._mailList = list;
        if (null != this.UIMail)
        {
            this.UIMail.RefreshMailList();
        }
        this.UpdateBubbleOfMail(true);
    }

    public void CloseUI()
    {
        if (null != this.UIMail)
        {
            this.UIMail.close();
        }
    }

    public mail_item GetItemByMailId(string mailid)
    {
        for (int i = 0; i < this._mailList.items.Count; i++)
        {
            if (mailid == this._mailList.items[i].mailid)
            {
                return this._mailList.items[i];
            }
        }
        return null;
    }

    private void removeByMailid(string mailid)
    {
        for (int i = 0; i < this._mailList.items.Count; i++)
        {
            if (mailid == this._mailList.items[i].mailid)
            {
                this._mailList.items.RemoveAt(i);
                break;
            }
        }
    }

    public void UpdateBubbleOfMail(bool open)
    {
        if (this._mailList == null)
        {
            return;
        }
        bool flag = false;
        for (int i = 0; i < this._mailList.items.Count; i++)
        {
            if (this._mailList.items[i].state == 1U || this._mailList.items[i].itemgot == 2U)
            {
                flag = true;
                break;
            }
        }
        if (flag)
        {
            if (open)
            {
                GlobalRegister.ActiveNewMailTips(1);
            }
        }
        else
        {
            GlobalRegister.ActiveNewMailTips(0);
        }
    }

    public void ReqMailList()
    {
        this.mailNetWork.ReqMailList();
    }

    public void ReqAllAttachment()
    {
        this.mailNetWork.ReqAllAttachment();
    }

    private void RefreshMailData(mail_item item)
    {
        for (int i = 0; i < this._mailList.items.Count; i++)
        {
            if (this._mailList.items[i].mailid == item.mailid)
            {
                this._mailList.items[i].fromid = item.fromid;
                this._mailList.items[i].fromname = item.fromname;
                this._mailList.items[i].title = item.title;
                this._mailList.items[i].text = item.text;
                this._mailList.items[i].createtime = item.createtime;
                this._mailList.items[i].deltime = item.deltime;
                this._mailList.items[i].state = item.state;
                this._mailList.items[i].itemgot = item.itemgot;
                this._mailList.items[i].obj_list.Clear();
                for (int j = 0; j < item.obj_list.Count; j++)
                {
                    this._mailList.items[i].obj_list.Add(item.obj_list[j]);
                }
                for (int k = 0; k < item.hero_list.Count; k++)
                {
                    this._mailList.items[i].hero_list.Add(item.hero_list[k]);
                }
                break;
            }
        }
    }

    public void RefreshMailItem(MSG_Ret_RefreshMail_SC item)
    {
        if (item.item == null)
        {
            return;
        }
        mail_item itemByMailId = this.GetItemByMailId(item.item.mailid);
        if (itemByMailId != null)
        {
            this.RefreshMailData(item.item);
            if (itemByMailId.state == 2U)
            {
                this.removeByMailid(itemByMailId.mailid);
                this.UpdateBubbleOfMail(false);
            }
            else
            {
                this.UpdateBubbleOfMail(true);
            }
        }
        else if (item.item.state != 2U)
        {
            this._mailList.items.Add(item.item);
            this.UpdateBubbleOfMail(true);
        }
        if (null != this.UIMail)
        {
            this.UIMail.RefreshMailList();
        }
    }

    public void ReqDeleteAllMail()
    {
        this.mailNetWork.ReqDeleteAllMail();
    }

    public void ReqGetAttachment(string mailid)
    {
        this.mailNetWork.ReqGetAttachment(mailid);
    }

    public void ReqDeleteMail(string mailid)
    {
        ulong num = 0UL;
        ulong.TryParse(mailid, out num);
        if (num <= 0UL)
        {
            return;
        }
        this.mailNetWork.ReqDelMail(num);
    }

    public void ReqOpenMail(string mailid)
    {
        ulong num = 0UL;
        ulong.TryParse(mailid, out num);
        if (num <= 0UL)
        {
            return;
        }
        this.mailNetWork.ReqOpenMail(num);
    }

    public void OnGetAttachment(MSG_Ret_GetAttachment_SC msg)
    {
        this.updateMailGetState(msg.mailid.ToString(), 1U);
        this.UIMail.RefreshMailItem(msg.mailid.ToString());
    }

    private void updateMailGetState(string mailid, uint state)
    {
        for (int i = 0; i < this._mailList.items.Count; i++)
        {
            if (mailid == this._mailList.items[i].mailid)
            {
                this._mailList.items[i].itemgot = state;
                break;
            }
        }
        if (null != this.UIMail)
        {
            this.UIMail.RefreshMailList();
        }
        this.UpdateBubbleOfMail(false);
    }

    public void OnUpdateMailState(MSG_Ret_RefreshMailState_SC msg)
    {
        FFDebug.LogError(this, "OnUpdateMailState = " + msg.state);
        this.updateMailState(msg.mailid.ToString(), msg.state);
        if (null != this.UIMail)
        {
            this.UIMail.RefreshMailList();
        }
        this.UpdateBubbleOfMail(false);
    }

    private void updateMailState(string mailid, uint state)
    {
        for (int i = 0; i < this._mailList.items.Count; i++)
        {
            if (mailid == this._mailList.items[i].mailid)
            {
                this._mailList.items[i].state = state;
                break;
            }
        }
    }

    public void OnDeleteMail(MSG_Ret_DeleteMail_SC msg)
    {
        for (int i = 0; i < msg.mailid.Count; i++)
        {
            this.removeByMailid(msg.mailid[i]);
        }
        if (null != this.UIMail)
        {
            this.UIMail.RefreshMailList();
            for (int j = 0; j < msg.mailid.Count; j++)
            {
                this.UIMail.OnDeleteMail(msg.mailid[j]);
            }
        }
        this.UpdateBubbleOfMail(true);
    }

    private MSG_Ret_MailList_SC _mailList;

    private MailNetWork mailNetWork;
}
