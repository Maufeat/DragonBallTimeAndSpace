using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Models;
using UnityEngine;

public class UINpcDlgController : ControllerBase
{
    public override void Awake()
    {
        this.Init();
    }

    public override string ControllerName
    {
        get
        {
            return "npcdlg_controller";
        }
    }

    public void Init()
    {
        this.InitDramaGroupConfig();
    }

    public void ShowNpcTalk(Action<Transform> callback)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_NpcTalk>("UI_NPCtalk", delegate ()
        {
            if (UIManager.GetUIObject<UI_NpcTalk>() != null)
            {
                callback(UIManager.GetUIObject<UI_NpcTalk>().talkRoot);
            }
            else
            {
                callback(null);
            }
        }, UIManager.ParentType.CommonUI, true);
    }

    private void InitDramaGroupConfig()
    {
        this.DramaGroups.Clear();
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("dialogueconfig");
        for (int i = 0; i < configTableList.Count; i++)
        {
            if (configTableList[i].GetField_Uint("group") != 0U)
            {
                uint field_Uint = configTableList[i].GetField_Uint("group");
                if (!this.DramaGroups.ContainsKey(field_Uint))
                {
                    this.DramaGroups.Add(field_Uint, new List<LuaTable>());
                }
                this.DramaGroups[field_Uint].Add(configTableList[i]);
            }
        }
        DramaDataComparer comparer = new DramaDataComparer();
        this.DramaGroups.BetterForeach(delegate (KeyValuePair<uint, List<LuaTable>> pair)
        {
            pair.Value.Sort(comparer);
        });
    }

    public BetterDictionary<uint, List<LuaTable>> DramaGroups = new BetterDictionary<uint, List<LuaTable>>();
}
