using System;
using Framework.Managers;
using msg;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextTipUpdate : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        PvpMatchType pvpMatchType = this.mPvpMatchType;
        if (pvpMatchType != PvpMatchType.Wudaohui)
        {
            if (pvpMatchType == PvpMatchType.Abattoir)
            {
                AbattoirMatchState getState = ControllerManager.Instance.GetController<AbattoirMatchController>().getState;
                AbattoirMatchState abattoirMatchState = getState;
                if (abattoirMatchState != AbattoirMatchState.Matching)
                {
                    return;
                }
            }
        }
        else if (ControllerManager.Instance.GetController<PVPMatchController>().pvpState != StageType.Match)
        {
            return;
        }
        if (!string.IsNullOrEmpty(this.mContent))
        {
            UIManager.Instance.ShowUI<UI_TextTip>("UI_TextTip", delegate ()
            {
                UI_TextTip uiobject = UIManager.GetUIObject<UI_TextTip>();
                string timeInMinutes = GlobalRegister.GetTimeInMinutes((uint)(Time.realtimeSinceStartup - this.mStartTime + Time.deltaTime));
                uiobject.Initilize(string.Format(this.mContent, timeInMinutes));
                uiobject.uiPanelRoot.SetParent(base.transform);
                uiobject.uiPanelRoot.localPosition = new Vector3(0f, (base.transform as RectTransform).sizeDelta.y, 0f);
                uiobject.uiPanelRoot.SetParent(UIManager.Instance.GetUIParent(UIManager.ParentType.Tips));
                ManagerCenter.Instance.GetManager<EscManager>().SetCurTipObj(base.gameObject);
            }, UIManager.ParentType.Tips, false);
            this.isUpdateTime = true;
        }
    }

    private void OnDestroy()
    {
        UI_TextTip uiobject = UIManager.GetUIObject<UI_TextTip>();
        if (uiobject != null)
        {
            UIManager.Instance.DeleteUI<UI_TextTip>();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.isUpdateTime = false;
        this.OnDestroy();
    }

    private void Update()
    {
        if (this.isUpdateTime)
        {
            UI_TextTip uiobject = UIManager.GetUIObject<UI_TextTip>();
            if (uiobject != null)
            {
                string timeInMinutes = GlobalRegister.GetTimeInMinutes((uint)(Time.realtimeSinceStartup - this.mStartTime + Time.deltaTime));
                uiobject.Initilize(string.Format(this.mContent, timeInMinutes));
            }
        }
    }

    public void SetTipCountDownText(PvpMatchType pvpMatchType, float startTime, float offsetX = 0f, float offsetY = 0f)
    {
        uint num = 190001U;
        if (pvpMatchType != PvpMatchType.Wudaohui)
        {
            if (pvpMatchType == PvpMatchType.Abattoir)
            {
                num = 190002U;
            }
        }
        this.mPvpMatchType = pvpMatchType;
        TextModelController controller = ControllerManager.Instance.GetController<TextModelController>();
        this.mContent = controller.GetContentByIDWithoutColorText(num.ToString());
        this.mStartTime = startTime;
    }

    private string mContent = string.Empty;

    private float mStartTime;

    private PvpMatchType mPvpMatchType;

    private bool isUpdateTime;
}
