using System;
using Framework.Managers;
using UnityEngine;

public class UI_Alert : UIPanelBase
{
    private AlertController alertController
    {
        get
        {
            return ControllerManager.Instance.GetController<AlertController>();
        }
    }

    public override void OnDispose()
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckHP));
        base.Dispose();
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.rootObject = root.gameObject;
        this.image = root.Find("img_red").gameObject;
        this.rootObject.SetActive(false);
        this.mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
    }

    public void Show()
    {
        this.image.GetComponent<TweenAlpha>().Reset();
        this.rootObject.SetActive(true);
        this.image.SetActive(true);
        Scheduler.Instance.AddTimer(0.5f, true, new Scheduler.OnScheduler(this.CheckHP));
    }

    public void Hide()
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckHP));
        if (this.rootObject == null)
        {
            return;
        }
        this.rootObject.SetActive(false);
        this.image.SetActive(false);
    }

    private void CheckHP()
    {
        if (this.mainView == null)
        {
            return;
        }
        if (!this.mainView.CheckIsLowHealth())
        {
            this.Hide();
        }
    }

    private GameObject rootObject;

    private GameObject image;

    private UI_MainView mainView;
}
