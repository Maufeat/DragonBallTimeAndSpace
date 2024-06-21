using System;
using Framework.Managers;
using msg;
using UnityEngine;

public class TextTipTimer : MonoBehaviour
{
    private void Init(Vector2 offset)
    {
        if (!string.IsNullOrEmpty(this.mContent))
        {
            UIManager.Instance.ShowUI<UI_TextTip>("UI_TextTip", delegate ()
            {
                UI_TextTip uiobject = UIManager.GetUIObject<UI_TextTip>();
                uiobject.Initilize(this.mContent);
                uiobject.uiPanelRoot.SetParent(this.transform);
                uiobject.uiPanelRoot.localPosition = new Vector3(0f, (this.transform as RectTransform).sizeDelta.y, 0f) + new Vector3(offset.x, 0f, offset.y);
                uiobject.uiPanelRoot.SetParent(UIManager.Instance.GetUIParent(UIManager.ParentType.Tips));
                this.isStart = true;
                ManagerCenter.Instance.GetManager<EscManager>().SetCurTipObj(this.gameObject);
            }, UIManager.ParentType.Tips, false);
        }
    }

    private void Update()
    {
        if (this.isStart)
        {
            if (ControllerManager.Instance.GetController<PVPMatchController>().pvpState != StageType.Login)
            {
                this.OnDestroy();
            }
            if (Time.realtimeSinceStartup - this.startTime > this.continueTime)
            {
                this.OnDestroy();
            }
            UI_TextTip uiobject = UIManager.GetUIObject<UI_TextTip>();
            if (uiobject != null)
            {
                uiobject.Initilize(GlobalRegister.GetTimeInMinutes((uint)(Time.realtimeSinceStartup - this.startTime)));
            }
        }
    }

    private void OnDestroy()
    {
        UI_TextTip uiobject = UIManager.GetUIObject<UI_TextTip>();
        if (uiobject != null)
        {
            UIManager.Instance.DeleteUI<UI_TextTip>();
        }
        this.isStart = false;
    }

    public void SetCountDownText(string content, float time, float offsetX = 0f, float offsetY = 0f)
    {
        this.mContent = content;
        this.startTime = Time.realtimeSinceStartup;
        this.continueTime = time;
        this.Init(new Vector2(offsetX, offsetY));
    }

    private float continueTime;

    private float startTime;

    private bool isStart;

    private string mContent;
}
