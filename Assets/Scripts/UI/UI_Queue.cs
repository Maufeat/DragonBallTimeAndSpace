using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Queue : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        this.txtNumber = root.Find("Offset_Example/Panel_queue/Head/txt_number").GetComponent<Text>();
        this.txtTime = root.Find("Offset_Example/Panel_queue/Head/txt_time").GetComponent<Text>();
        Button component = root.Find("Offset_Example/Panel_queue/Head/CloseButton").GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.btn_close_onclick));
    }

    public void SetupUI(int number, int second)
    {
        this.txtNumber.text = number + "↓";
        this.txtTime.text = this.GetTimeFormaterStr(second) + "↓";
    }

    private string GetTimeFormaterStr(int second)
    {
        TimeSpan timeSpan = new TimeSpan(0, 0, second);
        return string.Concat(new string[]
        {
            timeSpan.Hours.ToString("00"),
            ":",
            timeSpan.Minutes.ToString("00"),
            ":",
            timeSpan.Minutes.ToString("00")
        });
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    public override void AfterInit()
    {
        base.AfterInit();
    }

    private void btn_close_onclick()
    {
        UIManager.Instance.DeleteUI<UI_Queue>();
        ControllerManager.Instance.GetController<QueueController>().CloseQueue();
    }

    private Text txtNumber;

    private Text txtTime;
}
