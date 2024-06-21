using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextTip : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        this.lbl_content = root.Find("Text").GetComponent<Text>();
        root.gameObject.AddComponent<UIFocus>();
        base.OnInit(root);
    }

    public void Initilize(string content)
    {
        this.lbl_content.text = content;
    }

    public void SetTarget(GameObject obj)
    {
        this.mEventData = obj;
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    public Text lbl_content;

    public GameObject mEventData;
}
