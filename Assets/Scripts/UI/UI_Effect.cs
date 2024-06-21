using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class UI_Effect : UIPanelBase
{
    private UIEffectController controller
    {
        get
        {
            return ControllerManager.Instance.GetController<UIEffectController>();
        }
    }

    public override void OnDispose()
    {
        base.Dispose();
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.CheckIsInPlayState));
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.cacheNames = new List<string>();
        this.curEffectName = new List<string>();
        this.rootObject = root.gameObject;
        this.uiEffectPanel = this.rootObject.transform.Find("Offset/Panel_effect");
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.CheckIsInPlayState));
    }

    public void ShowEffectByName(string effectName)
    {
        if (!this.curEffectName.Contains(effectName))
        {
            this.curEffectName.Add(effectName);
        }
        this.rootObject.transform.SetAsLastSibling();
        for (int i = 0; i < this.uiEffectPanel.childCount; i++)
        {
            Transform child = this.uiEffectPanel.GetChild(i);
            bool flag = this.curEffectName.Contains(child.name) && child.name.Equals(effectName);
            if (flag)
            {
                child.gameObject.SetActive(flag);
                TweenFram component = child.GetComponent<TweenFram>();
                TweenPosition component2 = child.GetComponent<TweenPosition>();
                TweenAlpha component3 = child.GetComponent<TweenAlpha>();
                Vector2 zero = Vector2.zero;
                child.GetComponent<RectTransform>().anchoredPosition = zero;
                component.ResetAndPlayForward();
                component2.ResetAndPlayForward();
                component3.ResetAndPlayForward();
            }
        }
    }

    private void CheckIsInPlayState()
    {
        if (this.curEffectName.Count == 0)
        {
            return;
        }
        for (int i = 0; i < this.uiEffectPanel.childCount; i++)
        {
            Transform child = this.uiEffectPanel.GetChild(i);
            bool flag = this.curEffectName.Contains(child.name);
            if (flag)
            {
                TweenFram component = child.GetComponent<TweenFram>();
                if (component != null && !component.enabled && this.curEffectName.Contains(child.name))
                {
                    this.curEffectName.Remove(child.name);
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void CloseEffect()
    {
        for (int i = 0; i < this.uiEffectPanel.childCount; i++)
        {
            Transform child = this.uiEffectPanel.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }

    private GameObject rootObject;

    private Transform uiEffectPanel;

    private List<string> curEffectName;

    private List<string> cacheNames;
}
