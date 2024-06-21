using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_MessageBox : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        this.lbl_title = root.Find("txt_title").GetComponent<Text>();
        this.lbl_content = root.Find("txt_content").GetComponent<Text>();
        root.Find("btn_ok").GetComponent<Button>().onClick.RemoveAllListeners();
        root.Find("btn_ok").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btnok_on_click));
        root.Find("btn_cancel").GetComponent<Button>().onClick.RemoveAllListeners();
        root.Find("btn_cancel").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btncancel_on_click));
        base.OnInit(root);
        this.ui_root = root;
    }

    public void BtnSwitch(string btnName = "btn_cancel", bool state = false)
    {
        this.ui_root.Find(btnName).gameObject.SetActive(state);
    }

    private void btnok_on_click()
    {
        if (this.okCb != null)
        {
            this.okCb(this.o);
        }
        if (this.okCb2 != null)
        {
            this.okCb2(this.o2);
        }
        if (this.okCallBack != null)
        {
            this.okCallBack();
            this.okCallBack = null;
        }
        UIManager.Instance.DeleteUI<UI_MessageBox>();
    }

    private void btncancel_on_click()
    {
        if (this.cancleCallBack != null)
        {
            this.cancleCallBack();
            this.cancleCallBack = null;
        }
        UIManager.Instance.DeleteUI<UI_MessageBox>();
    }

    public void SetContent(string content, string title = "提示", bool showCancle = true)
    {
        this.lbl_title.text = title;
        this.lbl_content.text = content;
        this.ui_root.Find("btn_cancel").gameObject.SetActive(showCancle);
    }

    public void SetOkCb(MessageOkCb _cb, SaveDataItem _o)
    {
        this.okCb = _cb;
        this.o = _o;
    }

    public void SetOkCb(MessageOkCb2 _cb2, string _o2)
    {
        this.okCb2 = _cb2;
        this.o2 = _o2;
    }

    public void SetOkCb(Action okCallBack)
    {
        this.okCallBack = okCallBack;
    }

    public void SetCancleCb(Action cancleCallBack)
    {
        this.cancleCallBack = cancleCallBack;
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private Text lbl_title;

    private Text lbl_content;

    private SaveDataItem o;

    private string o2;

    private MessageOkCb okCb;

    private MessageOkCb2 okCb2;

    private Transform ui_root;

    private Action okCallBack;

    private Action cancleCallBack;
}

public delegate void MessageOkCb(SaveDataItem o);

public delegate void MessageOkCb2(string data);

public struct SaveDataItem
{
    public int pos;

    public int id;

    public SaveDataItemType type;

    public int subpos;

    public string thisid;

    public string packtype;
}

public enum SaveDataItemType
{
    none,
    skill,
    item
}


