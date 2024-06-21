using System;
using Models;

public class CoolDownController : ControllerBase
{
    public UI_CoolDown mUICoolDown
    {
        get
        {
            return UIManager.GetUIObject<UI_CoolDown>();
        }
    }

    public override void Awake()
    {
        this.Init();
    }

    public void Init()
    {
    }

    public void OpenUI(int second)
    {
        if (UIManager.GetUIObject<UI_CoolDown>() != null)
        {
            UIManager.Instance.DeleteUI<UI_CoolDown>();
        }
        else
        {
            UIManager.Instance.ShowUI<UI_CoolDown>("UI_CoolDown", delegate ()
            {
                this.mUICoolDown.CoolDown(second);
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    public void CloseUI()
    {
        if (UIManager.GetUIObject<UI_CoolDown>() != null)
        {
            UIManager.Instance.DeleteUI<UI_CoolDown>();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override string ControllerName
    {
        get
        {
            return "cooldown_controller";
        }
    }
}
