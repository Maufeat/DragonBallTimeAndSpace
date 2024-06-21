using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class UIEffectController : ControllerBase
{
    private UIEffectNetWork network
    {
        get
        {
            if (this.network_ == null)
            {
                this.network_ = new UIEffectNetWork();
            }
            return this.network_;
        }
    }

    private UI_Effect ui_effect
    {
        get
        {
            if (this.ui_effect_ == null)
            {
                this.ui_effect_ = UIManager.GetUIObject<UI_Effect>();
            }
            if (this.ui_effect_ == null)
            {
                UIManager.Instance.ShowUI<UI_Effect>("UI_Effect", null, UIManager.ParentType.Tips, false);
            }
            return this.ui_effect_;
        }
    }

    public override void Awake()
    {
        this.network.RegisterMsg();
    }

    public override void OnUpdate()
    {
        if (this.effectNeedToShowNames.Count > 0 && !UI_Loading.isLoading && this.ui_effect != null && Time.time > this.lastShowTime + 1.15f)
        {
            string effectName = this.effectNeedToShowNames.Dequeue();
            this.ui_effect.ShowEffectByName(effectName);
            this.lastShowTime = Time.time;
        }
    }

    public void ShowTaskEffect(uint questid, uint state)
    {
        string uieffectName = string.Empty;
        switch (state)
        {
            case 100U:
                uieffectName = "ui_mbdc";
                break;
            case 101U:
            case 102U:
                uieffectName = "ui_wcrw";
                break;
            default:
                if (state == 1U)
                {
                    uieffectName = "ui_lqrw";
                }
                break;
        }
        this.ShowEfffectByName(uieffectName);
    }

    public void ShowEfffectByName(string uieffectName)
    {
        if (!this.effectNeedToShowNames.Contains(uieffectName) && this.effectNeedToShowNames.Count < 4)
        {
            this.effectNeedToShowNames.Enqueue(uieffectName);
        }
    }

    public override string ControllerName
    {
        get
        {
            return "UIEffect_Controller";
        }
    }

    private UIEffectNetWork network_;

    private Queue<string> effectNeedToShowNames = new Queue<string>();

    private float lastShowTime;

    private int showCounter;

    private UI_Effect ui_effect_;
}
