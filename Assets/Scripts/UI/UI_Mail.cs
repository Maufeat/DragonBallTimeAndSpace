using System;
using System.Collections.Generic;
using Framework.Managers;
using mail;
using UnityEngine;
using UnityEngine.UI;

public class UI_Mail : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.control = ControllerManager.Instance.GetController<MailControl>();
        this.btnDelAll = root.Find("Offset_Mail/Panel_Mail/mail/btn_delall").gameObject;
        Button component = this.btnDelAll.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.onClickDelAll();
        });
        this.btnGetAll = root.Find("Offset_Mail/Panel_Mail/mail/btn_getall").gameObject;
        component = this.btnGetAll.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.onClickGetAll();
        });
        this.btnGet = root.Find("Offset_Mail/Panel_Mail/mail/Panel_Content/btn_get").gameObject;
        component = this.btnGet.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.onClickGetAttachment();
        });
        this.btnDel = root.Find("Offset_Mail/Panel_Mail/mail/Panel_Content/btn_delete").gameObject;
        component = this.btnDel.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.onClickDeleteMail();
        });
        this.listSample = root.Find("Offset_Mail/Panel_Mail/mail/Scroll View/Viewport/Content/list_mail").gameObject;
        this.listSample.SetActive(false);
        root.Find("Offset_Mail/Panel_Mail/mail/Panel_Content/Panel_Award").gameObject.SetActive(false);
        this.itemSample = root.Find("Offset_Mail/Panel_Mail/mail/Panel_Content/AwardItems/Items/Rect/img_item").gameObject;
        this.itemSample.SetActive(false);
        this.contentText = root.Find("Offset_Mail/Panel_Mail/mail/Panel_Content/txt_content").GetComponent<Text>();
        this.contentText.text = string.Empty;
        this.leftMailText = root.Find("Offset_Mail/Panel_Mail/background/Panel_title/txt_mail").GetComponent<Text>();
        this.leftMailText.text = "(0/0)";
        this.title = root.Find("Offset_Mail/Panel_Mail/mail/Panel_Content/txt_title").GetComponent<Text>();
        this.sendtime = root.Find("Offset_Mail/Panel_Mail/mail/Panel_Content/txt_date/text").GetComponent<Text>();
        this.sender = root.Find("Offset_Mail/Panel_Mail/mail/Panel_Content/txt_sender/text").GetComponent<Text>();
        GameObject gameObject = root.Find("Offset_Mail/Panel_Mail/background/Panel_title/btn_close").gameObject;
        component = gameObject.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.close();
        });
        this.Root = root;
        this.resetMailInfo();
        this.RefreshMailList();
    }

    public void RefreshMailList()
    {
        int num = 0;
        int i = 0;
        if (this.control.MailList == null)
        {
            return;
        }
        for (int j = 0; j < this.control.MailList.items.Count; j++)
        {
            if (this.control.MailList.items[j].state == 1U || this.control.MailList.items[j].itemgot == 2U)
            {
                num++;
            }
            if (i < this._allitems.Count)
            {
                this._allitems[i].UpdateMailInfo(this.control.MailList.items[j]);
                this._allitems[i].root.SetActive(true);
            }
            else
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.listSample);
                gameObject.SetActive(true);
                gameObject.transform.parent = this.listSample.transform.parent;
                gameObject.transform.localScale = Vector3.one;
                MailListItem mailListItem = new MailListItem(this, gameObject.transform);
                mailListItem.UpdateMailInfo(this.control.MailList.items[j]);
                this._allitems.Add(mailListItem);
            }
            i++;
        }
        while (i < this._allitems.Count)
        {
            this._allitems[i].root.SetActive(false);
            i++;
        }
        this.btnDelAll.gameObject.SetActive(this.control.MailList.items.Count > 0);
        this.btnGetAll.gameObject.SetActive(this.control.MailList.items.Count > 0);
        this.leftMailText.text = string.Format("({0}/{1})", num, this.control.MailList.items.Count);
        if (string.IsNullOrEmpty(this._lastSelectMail) && this._allitems.Count > 0)
        {
            this.OnSelectMail(this._allitems[0]);
        }
    }

    public void close()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Mail");
    }

    public void RefreshMailItem(string mailid)
    {
        if (this._lastSelectMail == mailid)
        {
            this.showSelectMailInfo();
        }
    }

    public void OnDeleteMail(string mailid)
    {
        if (this._lastSelectMail == mailid)
        {
            this._lastSelectMail = string.Empty;
            if (this._allitems.Count > 0 && this.control.MailList != null && this.control.MailList.items.Count > 0)
            {
                this.OnSelectMail(this._allitems[0]);
            }
            else
            {
                this.resetMailInfo();
            }
        }
    }

    public void OnSelectMail(MailListItem item)
    {
        if (item.MailID == this._lastSelectMail)
        {
            return;
        }
        this._lastSelectMail = item.MailID;
        for (int i = 0; i < this._allitems.Count; i++)
        {
            this._allitems[i].SetSelectShow(this._allitems[i].MailID == this._lastSelectMail);
        }
        this.showSelectMailInfo();
    }

    private void resetMailInfo()
    {
        this.contentText.text = string.Empty;
        this.sender.text = string.Empty;
        this.title.text = string.Empty;
        this.sendtime.text = string.Empty;
        for (int i = 0; i < this._allObjects.Count; i++)
        {
            this._allitems[i].root.SetActive(false);
        }
        this.btnGet.SetActive(false);
        for (int j = 0; j < this._allObjects.Count; j++)
        {
            this._allObjects[j].root.SetActive(false);
        }
    }

    private void showSelectMailInfo()
    {
        mail_item itemByMailId = this.control.GetItemByMailId(this._lastSelectMail);
        if (itemByMailId == null)
        {
            return;
        }
        this.contentText.text = GlobalRegister.ConfigColorToRichTextFormat(itemByMailId.text);
        this.sender.text = itemByMailId.fromname;
        this.title.text = itemByMailId.title;
        int time = 0;
        int.TryParse(itemByMailId.createtime, out time);
        this.sendtime.text = GlobalRegister.GetServerDateTimeByTimeStamp(time);
        if (itemByMailId.state == 1U)
        {
            this.control.ReqOpenMail(this._lastSelectMail);
        }
        int num = itemByMailId.hero_list.Count + itemByMailId.obj_list.Count;
        bool flag = num > 0 && itemByMailId.itemgot == 2U;
        this.btnGet.gameObject.SetActive(flag);
        this.showMailObjectInfo(itemByMailId, flag);
    }

    private void showMailObjectInfo(mail_item item, bool canget)
    {
        int i = 0;
        if (canget)
        {
            for (int j = 0; j < item.obj_list.Count; j++)
            {
                if (i < this._allObjects.Count)
                {
                    this._allObjects[i].UpdateMailItemInfo(item.obj_list[j]);
                    this._allObjects[i].root.SetActive(true);
                }
                else
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.itemSample);
                    gameObject.SetActive(true);
                    gameObject.transform.parent = this.itemSample.transform.parent;
                    gameObject.transform.localScale = Vector3.one;
                    MailObjectItem mailObjectItem = new MailObjectItem(this, gameObject.transform);
                    mailObjectItem.UpdateMailItemInfo(item.obj_list[j]);
                    this._allObjects.Add(mailObjectItem);
                }
                i++;
            }
            for (int k = 0; k < item.hero_list.Count; k++)
            {
                if (i < this._allObjects.Count)
                {
                    this._allObjects[i].UpdateMailItemInfo(item.hero_list[k]);
                    this._allObjects[i].root.SetActive(true);
                }
                else
                {
                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.itemSample);
                    gameObject2.SetActive(true);
                    gameObject2.transform.parent = this.itemSample.transform.parent;
                    gameObject2.transform.localScale = Vector3.one;
                    MailObjectItem mailObjectItem2 = new MailObjectItem(this, gameObject2.transform);
                    mailObjectItem2.UpdateMailItemInfo(item.hero_list[k]);
                    this._allObjects.Add(mailObjectItem2);
                }
                i++;
            }
        }
        while (i < this._allObjects.Count)
        {
            this._allObjects[i].root.SetActive(false);
            i++;
        }
    }

    private void onClickDelAll()
    {
        this.control.ReqDeleteAllMail();
    }

    private void onClickGetAll()
    {
        this.control.ReqAllAttachment();
    }

    private void onClickGetAttachment()
    {
        if (string.IsNullOrEmpty(this._lastSelectMail))
        {
            return;
        }
        this.control.ReqGetAttachment(this._lastSelectMail);
    }

    private void onClickDeleteMail()
    {
        if (string.IsNullOrEmpty(this._lastSelectMail))
        {
            return;
        }
        this.control.ReqDeleteMail(this._lastSelectMail);
    }

    public override void OnDispose()
    {
    }

    public Transform Root;

    private MailControl control;

    private GameObject btnGetAll;

    private GameObject btnDelAll;

    private GameObject btnGet;

    private GameObject btnDel;

    private GameObject itemSample;

    private GameObject listSample;

    private Text contentText;

    private List<MailListItem> _allitems = new List<MailListItem>();

    private string _lastSelectMail = string.Empty;

    private List<MailObjectItem> _allObjects = new List<MailObjectItem>();

    private Text leftMailText;

    private Text sender;

    private Text sendtime;

    private Text title;
}
