using System;
using System.Collections.Generic;
using Framework.Managers;
using Models;

public class CampController : ControllerBase
{
    public UI_JoinCamp UIJoinCamp
    {
        get
        {
            return UIManager.GetUIObject<UI_JoinCamp>();
        }
    }

    public override void Awake()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.Luafun_ShowCampInfo));
        this.campNetWork = new CampNetWork();
        this.campNetWork.Initialize();
    }

    public override string ControllerName
    {
        get
        {
            return "camp";
        }
    }

    public void ShowJoinCamp(bool bylua = false)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_JoinCamp>("UI_ChooseCamp", null, UIManager.ParentType.CommonUI, bylua);
    }

    public void CloseJoinCamp()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_ChooseCamp");
    }

    public void ReqJoinCamp(uint campid)
    {
        this.campNetWork.ReqJoinCountry(campid);
    }

    public void Luafun_ShowCampInfo(List<VarType> paras)
    {
        if (paras.Count != 0)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        this.ShowJoinCamp(true);
    }

    public CampNetWork campNetWork;
}
