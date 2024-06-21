using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_TaskTrackCheck : UIPanelBase
{
    public override void OnDispose()
    {
        base.Dispose();
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.rootObject = root.gameObject;
        this.btnTest = this.rootObject.transform.Find("PanelTask/Button");
        this.btnClose = this.rootObject.transform.Find("PanelTask/Button_close");
        this.inputTest = this.rootObject.transform.Find("PanelTask/InputField");
        this.taskItem = this.rootObject.transform.Find("PanelTask/TaskList/TaskScrollView/TaskList/TaskItem_concurrent/Panel_content/txt_content");
        Button component = this.btnTest.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.Test));
        Button component2 = this.btnClose.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(new UnityAction(this.Close));
    }

    private void Test()
    {
        InputField component = this.inputTest.GetComponent<InputField>();
        if (!string.IsNullOrEmpty(component.text))
        {
            WayFindItem wayFindItem = new WayFindItem(component.text);
            wayFindItem.InitUI(this.taskItem.gameObject, 50020U, 1, string.Empty, 0U, 10U);
        }
    }

    private void Close()
    {
        UIManager.Instance.DeleteUI<UI_TaskTrackCheck>();
    }

    private GameObject rootObject;

    private Transform btnTest;

    private Transform btnClose;

    private Transform inputTest;

    private Transform taskItem;
}
