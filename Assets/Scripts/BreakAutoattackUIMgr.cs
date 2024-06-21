using System;
using System.Collections.Generic;
using Framework.Managers;

public class BreakAutoattackUIMgr : IFFComponent
{
    public void Init()
    {
        this.BreakAutoAttackUINames.Clear();
        this.currentOpeningBreakAutoAttackUI.Clear();
        this.RegisterBreakAutoAttackUIName();
    }

    private void RegisterBreakAutoAttackUIName()
    {
        this.BreakAutoAttackUINames.Add("UI_NPCtalk");
        this.BreakAutoAttackUINames.Add("UI_Guild");
        this.BreakAutoAttackUINames.Add("UI_BuildGuild");
        this.BreakAutoAttackUINames.Add("UI_GuildList");
        this.BreakAutoAttackUINames.Add("UI_TaskDialog");
        this.BreakAutoAttackUINames.Add("UI_Activity_Poke");
        this.BreakAutoAttackUINames.Add("UI_Instance");
        this.BreakAutoAttackUINames.Add("UI_ChooseCamp");
    }

    public void ProcessOpenedUI(string uinama)
    {
        if (string.Compare(uinama, "UI_NPCtalk") == 0)
        {
            return;
        }
        UIPanelBase uiobject = UIManager.GetUIObject(uinama);
        LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel(uinama);
        if (uiobject)
        {
            this.ProcessOpenedUI(uinama, uiobject.byLua);
        }
        if (luaUIPanel != null)
        {
            this.ProcessOpenedUI(uinama, luaUIPanel.byNpcdlg);
        }
    }

    public void ProcessOpenedUI(string uinama, bool byLua)
    {
        if (this.BreakAutoAttackUINames.Contains(uinama))
        {
            if (byLua)
            {
                if (!this.currentOpeningBreakAutoAttackUI.Contains(uinama))
                {
                    this.currentOpeningBreakAutoAttackUI.Add(uinama);
                }
            }
            else if (this.currentOpeningBreakAutoAttackUI.Contains(uinama))
            {
                this.currentOpeningBreakAutoAttackUI.Remove(uinama);
            }
        }
        if (this.currentOpeningBreakAutoAttackUI.Count > 0)
        {
            this.RefreshBreakState();
        }
    }

    public void ProcessDeletedUI(string uinama)
    {
        if (this.currentOpeningBreakAutoAttackUI.Contains(uinama))
        {
            if (this.currentOpeningBreakAutoAttackUI.Contains(uinama))
            {
                this.currentOpeningBreakAutoAttackUI.Remove(uinama);
            }
            if (this.currentOpeningBreakAutoAttackUI.Count == 0 && MainPlayer.Self != null)
            {
                if (MainPlayer.Self.GetComponent<AutoAttack>() != null && MainPlayer.Self.GetComponent<AutoAttack>().AutoAttackOn)
                {
                    MainPlayer.Self.GetComponent<AutoAttack>().ThinkDelay(1f);
                }
                if (MainPlayer.Self.GetComponent<AttactFollowTeamLeader>() != null && MainPlayer.Self.GetComponent<AttactFollowTeamLeader>().AutoAttackOn)
                {
                    MainPlayer.Self.GetComponent<AttactFollowTeamLeader>().ThinkDelay(1f);
                }
            }
        }
    }

    public bool IsBreakAutoAttack()
    {
        bool result = false;
        if (this.currentOpeningBreakAutoAttackUI.Count > 0)
        {
            result = true;
        }
        return result;
    }

    private void RefreshBreakState()
    {
        if (MainPlayer.Self != null)
        {
            if (MainPlayer.Self.GetComponent<AutoAttack>() != null && MainPlayer.Self.GetComponent<AutoAttack>().AutoAttackOn)
            {
                MainPlayer.Self.GetComponent<AutoAttack>().ThinkImmediately();
            }
            if (MainPlayer.Self.GetComponent<AttactFollowTeamLeader>() != null && MainPlayer.Self.GetComponent<AttactFollowTeamLeader>().AutoAttackOn)
            {
                MainPlayer.Self.GetComponent<AttactFollowTeamLeader>().ThinkImmediately();
            }
        }
    }

    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Init();
        UIManager manager = ManagerCenter.Instance.GetManager<UIManager>();
        if (manager != null)
        {
            manager.eventShowUI += this.ProcessOpenedUI;
            manager.eventDeleteUI += this.ProcessDeletedUI;
        }
    }

    public void CompUpdate()
    {
    }

    public void CompDispose()
    {
        UIManager manager = ManagerCenter.Instance.GetManager<UIManager>();
        if (manager != null)
        {
            manager.eventShowUI -= this.ProcessOpenedUI;
            manager.eventDeleteUI -= this.ProcessDeletedUI;
        }
    }

    public void ResetComp()
    {
    }

    private List<string> BreakAutoAttackUINames = new List<string>();

    private List<string> currentOpeningBreakAutoAttackUI = new List<string>();
}
