using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_CountDwon : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.btnClose = root.Find("Offset/Panel_title/CloseButton");
        this.btnEnsure = root.Find("Offset/Panel_title/Ensure");
        this.btnCancel = root.Find("Offset/Panel_title/Cancel");
        this.txtTip = root.Find("Offset/Panel_title/txt_tip");
        this.txtCountDwon = root.Find("Offset/Panel_title/txt_countdown");
        Button component = this.btnEnsure.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.Ensure));
        Button component2 = this.btnCancel.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(new UnityAction(this.Cancel));
        Button component3 = this.btnClose.GetComponent<Button>();
        component3.onClick.RemoveAllListeners();
        component3.onClick.AddListener(new UnityAction(this.Close));
    }

    private void Ensure()
    {
        if (this.ensure != null)
        {
            this.ensure();
        }
        this.Close();
    }

    private void Cancel()
    {
        if (this.cancel != null)
        {
            this.cancel();
        }
        this.Close();
    }

    private void Close()
    {
        UIManager.Instance.DeleteUI<UI_CountDwon>();
    }

    private void End()
    {
        if (this.end != null)
        {
            this.end();
        }
        this.Close();
    }

    public void SetMessage(string message, float duation, Action onEnsure, Action onEnd = null, Action onCancel = null, bool isShowCloseButton = false)
    {
        this.txtTip.GetComponent<Text>().text = message;
        this.txtCountDwon.GetComponent<Text>().text = duation.ToString("f0");
        this.ensure = onEnsure;
        this.end = onEnd;
        this.cancel = onCancel;
        this.timer = duation;
        this.btnClose.gameObject.SetActive(isShowCloseButton);
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.CheckTime));
    }

    public override void OnDispose()
    {
        base.OnDispose();
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.CheckTime));
        this.ensure = null;
        this.cancel = null;
        this.end = null;
        this.timer = 0f;
    }

    private void CheckTime()
    {
        if (this.timer > 0f)
        {
            this.timer -= Time.deltaTime;
            this.txtCountDwon.GetComponent<Text>().text = (int)(this.timer / 60f) + ":" + (this.timer % 60f).ToString("f0");
        }
        else
        {
            this.End();
        }
    }

    private Transform btnClose;

    private Transform btnEnsure;

    private Transform btnCancel;

    private Transform txtTip;

    private Transform txtCountDwon;

    private Action ensure;

    private Action cancel;

    private Action end;

    private float timer;
}
