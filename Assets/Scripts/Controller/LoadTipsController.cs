using Framework.Managers;
using Models;
using System;

public class LoadTipsController : ControllerBase
{
    private UI_LoadTips loadTips
    {
        get
        {
            return UIManager.GetUIObject<UI_LoadTips>();
        }
    }

    public void ShowReconnectTips(string text)
    {
        if ((UIPanelBase)this.loadTips != (UIPanelBase)null)
            this.loadTips.ShowThis(text);
        else
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_LoadTips>("UI_LoadTips", (Action)(() =>
            {
                if (!((UIPanelBase)UIManager.GetUIObject<UI_LoadTips>() != (UIPanelBase)null))
                    return;
                UIManager.GetUIObject<UI_LoadTips>().ShowThis(text);
            }), UIManager.ParentType.Loading, false);
    }

    public void CloseReconnectTips()
    {
        if (!((UIPanelBase)this.loadTips != (UIPanelBase)null))
            return;
        this.loadTips.CloseThis();
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
            return "loadtips";
        }
    }
}
