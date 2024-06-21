using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextTip : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(this.mContent))
        {
            UIManager.Instance.ShowUI<UI_TextTip>("UI_TextTip", delegate ()
            {
                if (this == null || this.gameObject == null)
                {
                    return;
                }
                RectTransform rectTransform = this.transform as RectTransform;
                UI_TextTip uiobject = UIManager.GetUIObject<UI_TextTip>();
                uiobject.Initilize(this.mContent);
                uiobject.SetTarget(eventData.pointerEnter);
                uiobject.uiPanelRoot.SetParent(this.transform);
                uiobject.uiPanelRoot.localPosition = new Vector3((0.5f - rectTransform.pivot.x) * rectTransform.sizeDelta.x, rectTransform.sizeDelta.y * (1f - rectTransform.pivot.y) * 1.2f, 0f);
                uiobject.uiPanelRoot.SetParent(UIManager.Instance.GetUIParent(UIManager.ParentType.Tips));
                ManagerCenter.Instance.GetManager<EscManager>().SetCurTipObj(this.gameObject);
            }, UIManager.ParentType.Tips, false);
        }
    }

    private void OnDestroy()
    {
        UI_TextTip uiobject = UIManager.GetUIObject<UI_TextTip>();
        if (uiobject != null && uiobject.lbl_content.text == this.mContent)
        {
            UIManager.Instance.DeleteUI<UI_TextTip>();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.OnDestroy();
    }

    private string mContent
    {
        get
        {
            if (this.cb == null)
            {
                return this._content;
            }
            return this.cb();
        }
        set
        {
            this._content = value;
        }
    }

    public void SetText(string content)
    {
        this.mContent = content;
    }

    public void SetTextUI(string content)
    {
        this.mContent = content;
        UI_TextTip uiobject = UIManager.GetUIObject<UI_TextTip>();
        if (uiobject != null)
        {
            uiobject.Initilize(this.mContent);
        }
    }

    public void SetTextCb(TextTipContentCb _cb)
    {
        this.cb = _cb;
    }

    private string _content;

    private TextTipContentCb cb;
}

public delegate string TextTipContentCb();
