using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AbattoirTips : UIPanelBase
{
    private AbattoirMatchController teamController
    {
        get
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>();
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.InitGameObject(root);
        this.InitEvent();
    }

    private void InitGameObject(Transform root)
    {
        this.mRoot = root;
        this.Panel_Window = this.mRoot.Find("Offset_Confirm/Panel_Window");
        this.Panel_StayTips = this.mRoot.Find("Offset_Confirm/Panel_StayTips");
        this.window_content = this.mRoot.Find("Offset_Confirm/Panel_Window/txt_info").GetComponent<Text>();
        this.btnSure = this.mRoot.Find("Offset_Confirm/Panel_Window/btns/btn_ok");
        this.staytips_content = this.mRoot.Find("Offset_Confirm/Panel_StayTips/txt_tips").GetComponent<Text>();
        this.Panel_Window.gameObject.SetActive(false);
        this.Panel_StayTips.gameObject.SetActive(false);
        this.btnSure.gameObject.SetActive(false);
    }

    private void InitEvent()
    {
        UIEventListener.Get(this.btnSure.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSure);
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.DisposeEvent();
        this.DisposeGameObject();
    }

    private void DisposeEvent()
    {
    }

    private void OnClickSure(PointerEventData eventData)
    {
    }

    private void DisposeGameObject()
    {
        if (this.mRoot != null)
        {
            UnityEngine.Object.Destroy(this.mRoot.gameObject);
            this.mRoot = null;
        }
    }

    public void ShowTime(int getReliveRestTime)
    {
        this.Panel_Window.gameObject.SetActive(true);
        this.Panel_StayTips.gameObject.SetActive(false);
        if (this.type != UI_AbattoirTips.TipsType.ReliveTime)
        {
            this.type = UI_AbattoirTips.TipsType.ReliveTime;
        }
        if (this.lasttime == getReliveRestTime)
        {
            return;
        }
        this.lasttime = getReliveRestTime;
        this.window_content.text = string.Format("{0}秒后复活", getReliveRestTime);
    }

    public void ShowStay(string content)
    {
        this.Panel_Window.gameObject.SetActive(false);
        this.Panel_StayTips.gameObject.SetActive(true);
        this.staytips_content.text = content;
    }

    public void ShowOutArea()
    {
        if (this.type == UI_AbattoirTips.TipsType.OutArea)
        {
            return;
        }
        this.lasttime = -1;
        this.type = UI_AbattoirTips.TipsType.OutArea;
        this.window_content.text = "不在复活区域";
    }

    public Transform mRoot;

    public Transform Panel_Window;

    public Transform Panel_StayTips;

    public Text window_content;

    public Transform btnSure;

    private UI_AbattoirTips.TipsType type;

    private int lasttime = -1;

    private Text staytips_content;

    private enum TipsType
    {
        None,
        ReliveTime,
        OutArea
    }
}
