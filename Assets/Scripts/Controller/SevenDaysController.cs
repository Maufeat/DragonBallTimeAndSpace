using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Models;
using quiz;

public class SevenDaysController : ControllerBase
{
    public UI_SevenDays SevenDays
    {
        get
        {
            return UIManager.GetUIObject<UI_SevenDays>();
        }
    }

    public UI_MainView MainView
    {
        get
        {
            return UIManager.GetUIObject<UI_MainView>();
        }
    }

    public override string ControllerName
    {
        get
        {
            return "sevendays_controller";
        }
    }

    public override void Awake()
    {
        this.mNetWork = new SevenDaysNetWork();
        this.mNetWork.Initialize();
        this.InitConfig();
    }

    public override void OnDestroy()
    {
        if (this.mNetWork != null)
        {
            this.mNetWork.Uninitialize();
        }
        base.OnDestroy();
    }

    private void InitConfig()
    {
        this.SevenDaysConfig = LuaConfigManager.GetConfigTableList("seventarget_config");
    }

    public void ShowSevenDaysUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_SevenDays>("UI_7days", delegate ()
        {
            if (this.SevenDays != null)
            {
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public void CloseSevenDaysUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_7days");
    }

    public void Ret_Day7ActivityInfo_SC(MSG_Ret_Day7ActivityInfo_SC msg)
    {
        this.SevenInfo = msg;
        if (this.SevenDays != null)
        {
            this.SevenDays.InitInfo();
        }
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.EnableSevenDays(this.SevenInfo);
        }
    }

    public void Ret_SeekActivityInfo_SC(MSG_Ret_SeekActivityInfo_SC msg)
    {
        this.SeekInfo = msg;
        if (this.MainView != null)
        {
            this.MainView.AchievementView.InitInfo(msg);
        }
    }

    public SevenDaysNetWork mNetWork;

    public List<LuaTable> SevenDaysConfig = new List<LuaTable>();

    public MSG_Ret_Day7ActivityInfo_SC SevenInfo = new MSG_Ret_Day7ActivityInfo_SC();

    public MSG_Ret_SeekActivityInfo_SC SeekInfo = new MSG_Ret_SeekActivityInfo_SC();
}
