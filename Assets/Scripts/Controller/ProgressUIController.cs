using System;
using Framework.Managers;
using Models;

public class ProgressUIController : ControllerBase
{
    public UI_ProgressBar uiProgressBar
    {
        get
        {
            return UIManager.GetUIObject<UI_ProgressBar>();
        }
    }

    public string StrIcon
    {
        get
        {
            return this._strIcon;
        }
        set
        {
            this._strIcon = value;
        }
    }

    public string StrInfo
    {
        get
        {
            return this._strInfo;
        }
        set
        {
            this._strInfo = value;
        }
    }

    public void ShowProgressBar(float durationtime, ProgressUIController.ProgressBarType _type, Action callback)
    {
        this.ShowProgressBar(durationtime, SliderDirection.LeftToRight, _type, callback);
    }

    public void ShowProgressBar(float durationtime, SliderDirection sliderDir, Action callback)
    {
        this.ShowProgressBar(durationtime, sliderDir, ProgressUIController.ProgressBarType.Normal, callback);
    }

    public void ShowProgressBar(float durationtime, Action callback)
    {
        this.ShowProgressBar(durationtime, SliderDirection.LeftToRight, ProgressUIController.ProgressBarType.Normal, callback);
    }

    public void ShowProgressBar(float durationtime, SliderDirection sliderDir, ProgressUIController.ProgressBarType _type, Action callback)
    {
        this.state = ProgressUIController.ProgressBarShowState.Loading;
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_ProgressBar>("UI_ProgressBar", delegate ()
        {
            this.mType = _type;
            if (this.state == ProgressUIController.ProgressBarShowState.Deleted)
            {
                this.BreakProgressBar();
                return;
            }
            this.state = ProgressUIController.ProgressBarShowState.Complete;
            if (this.uiProgressBar != null)
            {
                this.uiProgressBar.InitThis(durationtime, callback, sliderDir);
                if (this.mType == ProgressUIController.ProgressBarType.Skill)
                {
                    this.uiProgressBar.RefreshInfo(this.StrInfo, this.StrIcon, true);
                }
                else
                {
                    this.uiProgressBar.RefreshInfo(this.StrInfo, this.StrIcon, false);
                }
            }
            this.StrIcon = string.Empty;
            this.StrInfo = string.Empty;
        }, UIManager.ParentType.CommonUI, false);
    }

    public void BreakProgressBar()
    {
        if (this.uiProgressBar != null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_ProgressBar");
        }
        this.state = ProgressUIController.ProgressBarShowState.Deleted;
    }

    public override void Awake()
    {
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "pregressui_controller";
        }
    }

    private string _strIcon = string.Empty;

    private string _strInfo = string.Empty;

    private ProgressUIController.ProgressBarShowState state = ProgressUIController.ProgressBarShowState.Deleted;

    private ProgressUIController.ProgressBarType mType = ProgressUIController.ProgressBarType.Normal;

    public enum ProgressBarShowState
    {
        Loading,
        Complete,
        Deleted
    }

    public enum ProgressBarType
    {
        Normal = 1,
        Skill
    }
}
