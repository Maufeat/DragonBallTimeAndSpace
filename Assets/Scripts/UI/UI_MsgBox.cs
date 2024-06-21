using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MsgBox : UIPanelBase
{
    private MsgBoxController controller
    {
        get
        {
            return ControllerManager.Instance.GetController<MsgBoxController>();
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.Root = root;
        this.InitGameObject();
    }

    private void InitGameObject()
    {
        this.btn_Confirm = this.Root.transform.Find("Offset_Confirm/Panel_Window/btns/btn_ok").gameObject;
        this.btn_Cancel = this.Root.transform.Find("Offset_Confirm/Panel_Window/btns/btn_cancel").gameObject;
        this.lb_describle = this.Root.transform.Find("Offset_Confirm/Panel_Window/txt_info").GetComponent<Text>();
        this.lb_title = this.Root.transform.Find("Offset_Confirm/Panel_Window/title/txt_title").GetComponent<Text>();
        this.lb_title_en = this.Root.transform.Find("Offset_Confirm/Panel_Window/title/txt_title_en").GetComponent<Text>();
        this.btn_Confirm.SetActive(true);
        this.btn_Cancel.SetActive(false);
    }

    public void RefreshDescrible(string desc)
    {
        this.lb_describle.text = desc;
    }

    public void ShowMsgBox(string s_title, string s_title_en, string s_describle, string s_confirm, string s_cancel, Action confirm, Action cancel, Action close = null)
    {
        this.btn_Confirm.SetActive(true);
        this.btn_Cancel.SetActive(true);
        this.lb_title.text = s_title;
        this.lb_describle.text = s_describle;
        this.lb_title_en.text = s_title_en;
        this.btn_Confirm.transform.Find("Text_s").GetComponent<Text>().text = s_confirm;
        this.btn_Confirm.transform.Find("Text_p").GetComponent<Text>().text = s_confirm;
        this.btn_Confirm.transform.Find("Text").GetComponent<Text>().text = s_confirm;
        this.btn_Cancel.transform.Find("Text").GetComponent<Text>().text = s_cancel;
        this.btn_Cancel.transform.Find("Text_s").GetComponent<Text>().text = s_cancel;
        this.btn_Cancel.transform.Find("Text_p").GetComponent<Text>().text = s_cancel;
        UIEventListener.Get(this.btn_Confirm).onClick = delegate (PointerEventData data)
        {
            this.controller.CloseMsgBox();
            confirm();
        };
        UIEventListener.Get(this.btn_Cancel).onClick = delegate (PointerEventData data)
        {
            this.controller.CloseMsgBox();
            if (cancel != null)
            {
                cancel();
            }
        };
    }

    public void ShowMsgBox(string s_title, string s_title_en, string s_describle, string s_confirm, Action confirm, Action close = null)
    {
        this.btn_Confirm.SetActive(true);
        this.btn_Cancel.SetActive(false);
        this.lb_title.text = s_title;
        this.lb_describle.text = s_describle;
        this.lb_title_en.text = s_title_en;
        this.btn_Confirm.transform.Find("Text").GetComponent<Text>().text = s_confirm;
        UIEventListener.Get(this.btn_Confirm).onClick = delegate (PointerEventData data)
        {
            this.controller.CloseMsgBox();
            if (confirm != null)
            {
                confirm();
            }
        };
    }

    public void ShowMsgBox(string s_title, string s_title_en, string s_describle, Action close = null)
    {
        this.btn_Confirm.SetActive(false);
        this.btn_Cancel.SetActive(false);
        this.lb_title.text = s_title;
        this.lb_describle.text = s_describle;
        this.lb_title_en.text = s_title_en;
    }

    public override void OnDispose()
    {
        base.OnDispose();
        GameObject gameObject = this.Root.gameObject;
        this.Root = null;
        UnityEngine.Object.Destroy(gameObject);
    }

    private Transform Root;

    private GameObject btn_Confirm;

    private GameObject btn_Cancel;

    private Text lb_describle;

    private Text lb_title;

    private Text lb_title_en;
}
