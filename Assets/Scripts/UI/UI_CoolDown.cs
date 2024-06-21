using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoolDown : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        this.lbl_time = root.Find("Text").GetComponent<Text>();
        base.OnInit(root);
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    public void CoolDown(int second)
    {
        this.time = second;
        this.lbl_time.text = this.time.ToString();
        this.time--;
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DoCoolDown));
        Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.DoCoolDown));
    }

    private void DoCoolDown()
    {
        if (this.lbl_time == null)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DoCoolDown));
            return;
        }
        this.lbl_time.text = this.time.ToString();
        this.time--;
        if (this.time < 0)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DoCoolDown));
            UIManager.Instance.DeleteUI<UI_CoolDown>();
        }
    }

    private Text lbl_time;

    private int time;
}
