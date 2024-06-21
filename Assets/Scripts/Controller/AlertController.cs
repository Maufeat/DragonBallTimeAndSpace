using System;
using System.Collections.Generic;
using Framework.Managers;
using Models;

public class AlertController : ControllerBase
{
    private UI_Alert ui_alert
    {
        get
        {
            return UIManager.GetUIObject<UI_Alert>();
        }
    }

    public override void Awake()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "alert_controller";
        }
    }

    public void ShowAlert(int flag)
    {
        this.flags[flag] = true;
        if (this.ui_alert != null)
        {
            this.ui_alert.Show();
        }
        else
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Alert>("UI_Alert", delegate ()
            {
                if (UIManager.GetUIObject<UI_Alert>() != null)
                {
                    UIManager.GetUIObject<UI_Alert>().Show();
                }
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    public void CloseAlert(int flag)
    {
        this.flags[flag] = false;
        if (this.flags.ContainsValue(true))
        {
            return;
        }
        if (this.ui_alert != null)
        {
            this.ui_alert.Hide();
        }
    }

    private Dictionary<int, bool> flags = new Dictionary<int, bool>();
}
