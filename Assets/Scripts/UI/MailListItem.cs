using System;
using mail;
using UnityEngine;
using UnityEngine.UI;

public class MailListItem
{
    public MailListItem(UI_Mail mailParent, Transform go)
    {
        this.root = go.gameObject;
        this.parent = mailParent;
        this.stayGo = go.Find("img_skill_stay").gameObject;
        this.selectGo = go.Find("img_skill_select").gameObject;
        this.selectGo.SetActive(false);
        this.unreadGo = go.Find("mail_unread").gameObject;
        this.readGo = go.Find("mail_read").gameObject;
        this.titleText = go.Find("txt_title").GetComponent<Text>();
        this.timeText = go.Find("txt_time").GetComponent<Text>();
        Button component = this.root.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.OnSelectMail();
        });
    }

    public string MailID
    {
        get
        {
            return this._mailid;
        }
    }

    public void UpdateMailInfo(mail_item item)
    {
        this._mailid = item.mailid;
        this.titleText.text = item.title;
        GlobalRegister.ProcessMailRemainTime(item.deltime, this.timeText);
        this.unreadGo.SetActive(item.state == 1U);
        this.readGo.SetActive(item.state != 1U);
    }

    public void OnSelectMail()
    {
        this.parent.OnSelectMail(this);
    }

    public void SetSelectShow(bool show)
    {
        this.selectGo.SetActive(show);
    }

    public GameObject root;

    private UI_Mail parent;

    private GameObject stayGo;

    private GameObject selectGo;

    private GameObject unreadGo;

    private GameObject readGo;

    private Text titleText;

    private Text contentText;

    private Text timeText;

    private string _mailid = string.Empty;
}
