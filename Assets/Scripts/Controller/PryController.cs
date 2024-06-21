using System;
using System.Collections.Generic;
using Framework.Managers;
using guild;
using Models;
using msg;

public class PryController : ControllerBase
{
    public UI_Pry uiPry
    {
        get
        {
            return UIManager.GetUIObject<UI_Pry>();
        }
    }

    public override void Awake()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.Luafun_ReqBackToMyGuild));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.Luafun_ShowPry));
        this.pryNetWork = new PryNetWork();
        this.pryNetWork.Initialize();
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "pry_controller";
        }
    }

    public void ShowPryUI(bool bylua = false)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Pry>("UI_Activity_Poke", delegate ()
        {
            this.pryNetWork.ReqTargetPryEnemyGuild();
        }, UIManager.ParentType.CommonUI, bylua);
    }

    public void ClosePryUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Activity_Poke");
    }

    public bool IfHasTargetGuild()
    {
        return this.currentPryGuild != null && this.currentPryGuild.guildid != 0UL;
    }

    public void RefreshVisableNpc(MSG_Ret_VisibleNpcList_SC msgb)
    {
        this.visibleSpecialNpc.Clear();
        for (int i = 0; i < msgb.npc.Count; i++)
        {
            this.visibleSpecialNpc.Add(msgb.npc[i]);
        }
    }

    public bool IsSpecialNpc(uint id)
    {
        for (int i = 0; i < this.specealNpcBaseID.Length; i++)
        {
            if (this.specealNpcBaseID[i] == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsInVisableSpecialNpc(uint id)
    {
        return this.IsSpecialNpc(id) && !this.visibleSpecialNpc.Contains(id);
    }

    public void Luafun_ShowPry(List<VarType> paras)
    {
        if (paras.Count != 0)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        this.ShowPryUI(true);
    }

    public void Luafun_ReqBackToMyGuild(List<VarType> paras)
    {
        if (paras.Count != 0)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelSystem, CommonUtil.GetText(dynamic_textid.IDs.sociaty_transmission), MsgBoxController.MsgOptionConfirm, MsgBoxController.MsgOptionCancel, UIManager.ParentType.CommonUI, delegate ()
        {
            this.pryNetWork.ReqBackToMyGuild();
        }, null, null);
    }

    public PryNetWork pryNetWork;

    public CiTanEnemyGuildItem currentPryGuild;

    public uint[] specealNpcBaseID = new uint[]
    {
        5011U,
        5012U,
        5013U,
        5014U,
        5015U,
        5016U,
        5017U,
        5018U,
        5019U,
        5020U
    };

    public List<uint> visibleSpecialNpc = new List<uint>();
}
