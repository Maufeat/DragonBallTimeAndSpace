using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AbattoirMatch : UIPanelBase
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
        this.content = this.mRoot.Find("Offset_Confirm/Panel_Window/txt_info").GetComponent<Text>();
        this.btnReady = this.mRoot.Find("Offset_Confirm/Panel_Window/btns/btn_ok");
        this.btnCancelReady = this.mRoot.Find("Offset_Confirm/Panel_Window/btns/btn_cancel");
        this.btnCancelReady.gameObject.SetActive(false);
    }

    private void InitEvent()
    {
        UIEventListener.Get(this.btnReady.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickReady);
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.DisposeEvent();
        this.DisposeGameObject();
    }

    private void DisposeEvent()
    {
        if (this.dele != null)
        {
            Scheduler.Instance.RemoveTimer(this.dele);
        }
    }

    private void DisposeGameObject()
    {
        if (this.mRoot != null)
        {
            UnityEngine.Object.Destroy(this.mRoot.gameObject);
            this.mRoot = null;
        }
    }

    private void OnClickReady(PointerEventData eventData)
    {
        this.SetClicked(false);
        this.teamController.SendMatchReady();
    }

    public void OpenShow(uint restTime, uint readyNum, uint matchNum, bool ready)
    {
        if (this.dele != null)
        {
            Scheduler.Instance.RemoveTimer(this.dele);
        }
        if (this.content != null)
        {
            uint time = restTime;
            this.content.text = string.Format("{0}\n{1}/{2}", restTime, readyNum, matchNum);
            this.dele = delegate ()
            {
                if (time > 0U && this.content != null)
                {
                    time -= 1U;
                    this.content.text = string.Format("{0}\n{1}/{2}", time, readyNum, matchNum);
                }
                else
                {
                    Scheduler.Instance.RemoveTimer(this.dele);
                    this.dele = null;
                }
            };
            Scheduler.Instance.AddTimer(1f, true, this.dele);
        }
        this.SetClicked(!ready);
    }

    public void SetClicked(bool showBtn)
    {
        if (this.btnReady != null)
        {
            this.btnReady.gameObject.SetActive(showBtn);
        }
    }

    public Transform mRoot;

    public Text content;

    public Transform btnReady;

    public Transform btnCancelReady;

    private Scheduler.OnScheduler dele;
}
