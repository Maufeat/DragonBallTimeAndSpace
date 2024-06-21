using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using LuaInterface;
using magic;
using map;
using msg;
using UnityEngine;

public class SkillManager : IManager
{
    private EntitiesManager mEntitiesManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    public void Init()
    {
        this.NetWork.Initialize();
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("skill_data");
        for (int i = 0; i < configTableList.Count; i++)
        {
            LuaTable luaTable = configTableList[i];
            this.Skill_configMap[luaTable.GetField_ULong("id")] = luaTable;
        }
        List<LuaTable> configTableList2 = LuaConfigManager.GetConfigTableList("skill_stage");
        for (int j = 0; j < configTableList2.Count; j++)
        {
            LuaTable luaTable2 = configTableList2[j];
            this.Stage_configMap[luaTable2.GetField_ULong("id")] = luaTable2;
        }
    }

    public void RelaseDrinkBloodSkill(CharactorBase NPCOwner)
    {
        uint occupation = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.occupation;
        uint cacheField_Uint = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("drinkBloodSkill").GetCacheField_Table(occupation.ToString()).GetCacheField_Uint("value");
        Vector2 serverPosByWorldPos = GraphUtils.GetServerPosByWorldPos(MainPlayer.Self.ModelObj.transform.position, true);
        Vector2 serverPosByWorldPos2 = GraphUtils.GetServerPosByWorldPos(NPCOwner.ModelObj.transform.position, true);
        Vector2 vector;
        if (GraphUtils.FindPosForRush(serverPosByWorldPos, serverPosByWorldPos2, out vector, (TileFlag)11, (TileFlag)11))
        {
            this.DrinkBloodTarget = NPCOwner;
            this.DrinkSkillID = cacheField_Uint;
            this.NetWork.ReqRelaseDrinkBloodSkill(NPCOwner.EID.Id, cacheField_Uint);
        }
        else
        {
            TipsWindow.ShowWindow(TipsType.DRINLBLOODFAIL, null);
        }
    }

    public void RefreahChantSkillLimit(List<uint> skillids)
    {
        this.ChantSkillUnlimit = skillids;
    }

    public bool CheckChantSkillLimit(uint skillid)
    {
        if (this.ChantSkillUnlimit == null)
        {
            return true;
        }
        for (int i = 0; i < this.ChantSkillUnlimit.Count; i++)
        {
            if (skillid == this.ChantSkillUnlimit[i])
            {
                return false;
            }
        }
        return true;
    }

    public void SendDrinkSkill()
    {
        if (this.DrinkSkillID != 0U && this.DrinkBloodTarget != null)
        {
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().manualSelect.SetTarget(this.DrinkBloodTarget, false, true);
            MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(this.DrinkSkillID);
            this.DrinkBloodTarget = null;
            this.DrinkSkillID = 0U;
        }
    }

    public void DrinkBloodRelase(CharactorBase NPCOwner)
    {
        uint occupation = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.occupation;
        float num = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("drinkBloodDistance").GetCacheField_Table(occupation.ToString()).GetCacheField_Uint("value");
        Vector2 serverPosByWorldPos = GraphUtils.GetServerPosByWorldPos(MainPlayer.Self.ModelObj.transform.position, true);
        Vector2 serverPosByWorldPos2 = GraphUtils.GetServerPosByWorldPos(NPCOwner.ModelObj.transform.position, true);
        if (Vector2.Distance(serverPosByWorldPos, serverPosByWorldPos2) < num)
        {
            this.RelaseDrinkBloodSkill(NPCOwner);
        }
        else
        {
            MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(NPCOwner, PathFindFollowTarget.FollowType.FollowToDrinkBlood, new Action<CharactorBase, bool>(this.OnReachTarget));
        }
    }

    private void OnReachTarget(CharactorBase cbase, bool isreach)
    {
        this.RelaseDrinkBloodSkill(cbase);
    }

    public void ReqRelasePetQTESkill(ulong bossID, uint dir, Vector2 pos)
    {
        this.NetWork.ReqRelasePetQTESkill(bossID, dir, pos);
    }

    public bool SkillNeedMove(ulong _stageid)
    {
        LuaTable luaTable = this.Gett_skill_stage_config(_stageid);
        if (luaTable == null)
        {
            FFDebug.Log(this, FFLogType.Skill, "stage config can't find stageID,stageID = " + _stageid.ToString());
            return false;
        }
        int[] skillMoveParam = SkillManager.GetSkillMoveParam(luaTable.GetField_String("MoveDis"));
        return skillMoveParam[0] != 0 || skillMoveParam[1] != 0 || skillMoveParam[2] != 0;
    }

    private static int[] GetSkillMoveParam(string ParamStr)
    {
        string[] array = ParamStr.Split(new char[]
        {
            ':'
        });
        return new int[]
        {
            int.Parse(array[0]),
            int.Parse(array[1]),
            int.Parse(array[2])
        };
    }

    public static bool HasSelectPos(LuaTable config)
    {
        return config.GetField_String("SightType") != "0" && config.GetField_String("SightType") != "1" && config.GetField_String("SightType") != string.Empty;
    }

    public LuaTable Gett_skill_lv_config(ulong key)
    {
        if (!this.Skill_configMap.ContainsKey(key))
        {
            FFDebug.LogWarning(this, string.Format("t_skill_lv_config cant find key : {0}", key));
            return null;
        }
        return this.Skill_configMap[key];
    }

    public LuaTable Gett_skill_stage_config(ulong key)
    {
        if (!this.Stage_configMap.ContainsKey(key))
        {
            return null;
        }
        return this.Stage_configMap[key];
    }

    public bool IsChantStage(ulong stateid)
    {
        LuaTable luaTable = this.Getlv_config(stateid);
        return luaTable != null && luaTable.GetField_Uint("releasetype") == 2U && this.GetStageCount(stateid) == 1UL;
    }

    public bool IsSpellChannelStage(ulong stateid)
    {
        LuaTable luaTable = this.Getlv_config(stateid);
        return luaTable != null && luaTable.GetField_Uint("releasetype") == 6U;
    }

    public bool IsLieveSkill(ulong stateid)
    {
        LuaTable luaTable = this.Getlv_config(stateid);
        return luaTable != null && luaTable.GetField_Uint("canrelieve") == 1U;
    }

    public LuaTable Getlv_config(ulong Skillid, uint Level)
    {
        ulong key = Skillid * 10000UL + (ulong)(Level * 10U) + 1UL;
        if (this.Skill_configMap.ContainsKey(key))
        {
            return this.Skill_configMap[key];
        }
        return null;
    }

    public LuaTable Getlv_config(ulong stageId)
    {
        ulong key = stageId - this.GetStageCount(stageId) + 1UL;
        if (this.Skill_configMap.ContainsKey(key))
        {
            return this.Skill_configMap[key];
        }
        return null;
    }

    public uint GetSkillLevel(uint Wholeskillid)
    {
        uint num = Wholeskillid % 1000U;
        return num / 10U;
    }

    public ulong GetStageCount(ulong Wholeskillid)
    {
        return Wholeskillid % 10UL;
    }

    public void SendSkill(uint Skillid, CharactorBase Target = null)
    {
        EntryIDType entryIDType = null;
        if (Target != null)
        {
            FFDebug.Log(this, FFLogType.Skill, string.Format("SendSkill Target :{0}", Target.EID.ToString()));
            entryIDType = Target.EID.ToEntryIDType();
            if (entryIDType.id == 0UL)
            {
                entryIDType = null;
            }
        }
        this.NetWork.SendStartSkill(Skillid, entryIDType);
    }

    public void SendSkillStage(ulong SkillStageid, uint type, Vector3 Pos, uint Dir, CharactorBase Target)
    {
        EntryIDType entryIDType = null;
        if (Target != null)
        {
            entryIDType = Target.EID.ToEntryIDType();
            if (entryIDType.id == 0UL)
            {
                entryIDType = null;
            }
        }
        this.NetWork.SendSkillStage(SkillStageid, type, Pos, Dir, entryIDType);
    }

    public void TurnOffSkill(uint Skillid)
    {
        FFDebug.Log(this, FFLogType.Skill, string.Format("TurnOffSkill :{0}", Skillid));
        this.NetWork.TurnOffSkill(Skillid);
    }

    public void StartDisplaySkill(MSG_Ret_StartMagicAttack_SC mdata)
    {
        CharactorBase charactorByID = this.mEntitiesManager.GetCharactorByID(mdata.att);
        if (charactorByID != null)
        {
            ISkillHolder component = charactorByID.GetComponent<ISkillHolder>();
            if (component != null)
            {
                component.StartDisplaySkill(mdata);
            }
            else
            {
                FFDebug.Log(this, FFLogType.Skill, string.Format("{0} has no ISkillHolder", charactorByID.EID.ToString()));
            }
        }
        else
        {
            FFDebug.LogWarning(this, string.Format("ERROR StartDisplaySkill Can not Find Charactor : {0} ", mdata.att));
        }
    }

    public void DisplaySkillStage(MSG_Ret_SyncSkillStage_SC mdata)
    {
        CharactorBase charactorByID = this.mEntitiesManager.GetCharactorByID(mdata.att);
        if (charactorByID != null)
        {
            FFDebug.Log(this, FFLogType.Skill, string.Concat(new object[]
            {
                string.Format("MSG_Ret_SyncSkillStage_SC--->({0},{1})  dir:{2}", mdata.desx, mdata.desy, mdata.userdir),
                " WorldPos: ",
                GraphUtils.GetWorldPosByServerPos(new Vector2(mdata.desx, mdata.desy)),
                "---",
                charactorByID.EID
            }));
            this.ShowSkillAttWarning(charactorByID, mdata);
            ISkillHolder component = charactorByID.GetComponent<ISkillHolder>();
            if (component != null)
            {
                component.DisplaySkillStage(mdata);
            }
            else
            {
                FFDebug.Log(this, FFLogType.Skill, string.Format("{0} has no ISkillHolder", charactorByID.EID.ToString()));
            }
        }
        else
        {
            FFDebug.LogWarning(this, string.Format("ERROR DisplaySkillStage Can not Find Charactor : {0} ", mdata.att));
        }
    }

    private void ShowSkillAttWarning(CharactorBase att, MSG_Ret_SyncSkillStage_SC mdata)
    {
        if (mdata.stagetype == 2U)
        {
            return;
        }
        if (!this.Stage_configMap.ContainsKey(mdata.skillstage))
        {
            return;
        }
        LuaTable cacheField_Table = this.Stage_configMap[mdata.skillstage].GetCacheField_Table("PreWorkdata");
        if (cacheField_Table == null)
        {
            return;
        }
        if (cacheField_Table.GetField_Int("attwarntype") == 0)
        {
            return;
        }
        AttackWarningEffect component = att.GetComponent<AttackWarningEffect>();
        if (component != null)
        {
            Vector2 vector = new Vector2(mdata.desx, mdata.desy);
            Vector3 vector2 = Vector3.zero;
            uint dir = 0U;
            float param = cacheField_Table.GetField_Float("rangep1");
            int field_Int = cacheField_Table.GetField_Int("rangetype");
            if (cacheField_Table.GetField_Int("attwarntype") == 1)
            {
                vector2 = att.CurrServerPos;
                Vector2 vdir = CommonTools.ToServerVector((vector - att.CurrServerPos).normalized);
                dir = CommonTools.GetServerDirByClientDir(vdir);
            }
            else if (cacheField_Table.GetField_Int("attwarntype") == 2)
            {
                vector2 = vector;
                dir = 0U;
            }
            if (vector != Vector2.zero && field_Int == 1)
            {
                param = Vector3.Distance(vector, vector2);
            }
            component.SetWarningEffect(field_Int, cacheField_Table.GetField_Float("lasttime") / 1000f, GraphUtils.GetWorldPosByServerPos(vector2), CommonTools.GetClientDirQuaternionByServerDir((int)dir), param, cacheField_Table.GetField_Float("rangep2"));
        }
    }

    public void GetSkillHit(MSG_Ret_MagicAttack_SC mdata)
    {
        this.CharactorList.Clear();
        for (int i = 0; i < mdata.pklist.Count; i++)
        {
            PKResult pkresult = mdata.pklist[i];
            ControllerManager.Instance.GetController<MainUIController>().AddSkillLog(mdata, pkresult);
            CharactorBase charactorByID = this.mEntitiesManager.GetCharactorByID(pkresult.def);
            if (charactorByID != null)
            {
                if (!this.CharactorList.Contains(charactorByID))
                {
                    this.CharactorList.Add(charactorByID);
                }
            }
            else
            {
                FFDebug.LogWarning(this, string.Format("ERROR GetSkillHit Can not Find Charactor : {0}", pkresult.def.id));
            }
        }
        CharactorBase charactorByID2 = this.mEntitiesManager.GetCharactorByID(mdata.att);
        if (charactorByID2 != null)
        {
            this.disComp.Target = charactorByID2.ModelObj;
            this.CharactorList.Sort(this.disComp);
        }
        for (int j = 0; j < this.CharactorList.Count; j++)
        {
            try
            {
                this.CharactorList[j].HandleHit(mdata);
            }
            catch (Exception arg)
            {
                FFDebug.LogWarning(this, "HandleHit: " + arg);
            }
        }
        CharactorBase charactorByID3 = this.mEntitiesManager.GetCharactorByID(mdata.att);
        if (charactorByID3 != null)
        {
            charactorByID3.HitOther(mdata, this.CharactorList.ToArray());
        }
    }

    public void HandleBreakSkill(MSG_Ret_InterruptSkill_SC mdata)
    {
        CharactorBase charactorByID = this.mEntitiesManager.GetCharactorByID(mdata.att);
        if (charactorByID != null)
        {
            if (charactorByID.GetComponent<ISkillHolder>() != null)
            {
                charactorByID.GetComponent<ISkillHolder>().HandleBreakSkill(mdata);
            }
        }
        else
        {
            FFDebug.LogWarning(this, string.Format("ERROR HandleBreakSkill Can not Find Charactor : {0}", mdata.att));
        }
    }

    public void HandleMpHpChange(MSG_Ret_HpMpPop_SC mdata)
    {
        CharactorBase charactorByID = this.mEntitiesManager.GetCharactorByID(mdata.target);
        if (charactorByID != null)
        {
            charactorByID.RevertHpMp(mdata);
            ControllerManager.Instance.GetController<MainUIController>().AddHpChangeLog(mdata);
        }
    }

    public void RefreshSkillServerData(List<SkillData> skillList)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            SkillData skillData = skillList[i];
            if (!this.SkillServerDataMap.ContainsKey(skillData.skillid))
            {
                this.SkillServerDataMap[skillData.skillid] = new SkillServerData();
            }
            this.SkillServerDataMap[skillData.skillid].SetData(skillData);
        }
        if (MainPlayerSkillHolder.Instance != null)
        {
            MainPlayerSkillHolder.Instance.UpdateSkillState(skillList);
        }
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.InitSkillTexture(true);
            mainView.UpdateSkillStorage();
        }
    }

    public void SetAttackWarningEffect(MSG_Ret_AttWarning_SC mdata)
    {
        CharactorBase charactorByID = this.mEntitiesManager.GetCharactorByID(mdata.attacker);
        if (charactorByID != null)
        {
            AttackWarningEffect component = charactorByID.GetComponent<AttackWarningEffect>();
            if (component != null)
            {
                List<AttWarning> warning = mdata.warning;
                if (warning != null)
                {
                    for (int i = 0; i < warning.Count; i++)
                    {
                        AttWarning attWarning = warning[i];
                        component.SetWarningEffect((int)attWarning.rangetype, attWarning.lasttime / 1000f, CommonTools.ToClientPos(attWarning.pos), CommonTools.GetClientDirQuaternionByServerDir((int)attWarning.dir), attWarning.rangep1, attWarning.rangep2);
                    }
                }
            }
        }
    }

    public SkillClip GetFFSkillAnimClip(CharactorBase Charactor, ulong skillstage, SkillClipServerDate serverDate = null)
    {
        if (Charactor == null)
        {
            FFDebug.LogWarning(this, "Charactor null");
            return null;
        }
        if (string.IsNullOrEmpty(Charactor.animatorControllerName))
        {
            return null;
        }
        LuaTable luaTable = this.Gett_skill_stage_config(skillstage);
        LuaTable luaTable2 = this.Gett_skill_lv_config(skillstage);
        if (luaTable == null)
        {
            return null;
        }
        int selectindex = (!Charactor.IsFly) ? 0 : 1;
        FFActionClip ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(Charactor.animatorControllerName, luaTable.GetField_Uint("ActionID"), selectindex);
        if (ffactionClip == null)
        {
            return null;
        }
        SkillClip @object = ClassPool.GetObject<SkillClip>();
        @object.ServerData = serverDate;
        @object.AnimData = ffactionClip;
        @object.AnimConfig = ClassPool.GetObject<SkillClipConfig>();
        @object.AnimConfig.Init(luaTable);
        @object.LvConfig = ClassPool.GetObject<LVSkillConfig>();
        @object.LvConfig.Init(luaTable2, luaTable2.GetCacheField_Uint("chanttime") / 1000f);
        @object.SkillStageId = skillstage;
        return @object;
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
        this.SkillServerDataMap.Clear();
    }

    public SkillNetWork NetWork = new SkillNetWork();

    private SkillManager.DistanceComparer disComp = new SkillManager.DistanceComparer();

    public Dictionary<ulong, LuaTable> Skill_configMap = new Dictionary<ulong, LuaTable>();

    public Dictionary<ulong, LuaTable> Stage_configMap = new Dictionary<ulong, LuaTable>();

    public BetterDictionary<uint, SkillServerData> SkillServerDataMap = new BetterDictionary<uint, SkillServerData>();

    public List<uint> ChantSkillUnlimit;

    private CharactorBase DrinkBloodTarget;

    private uint DrinkSkillID;

    private List<CharactorBase> CharactorList = new List<CharactorBase>();

    private class DistanceComparer : IComparer<CharactorBase>
    {
        public int Compare(CharactorBase x, CharactorBase y)
        {
            if (null == x.ModelObj)
            {
                return -1;
            }
            if (null == y.ModelObj)
            {
                return 1;
            }
            if (null == this.Target)
            {
                return 0;
            }
            float num = Vector3.Distance(x.ModelObj.transform.position, this.Target.transform.position);
            float num2 = Vector3.Distance(y.ModelObj.transform.position, this.Target.transform.position);
            if (num > num2)
            {
                return 1;
            }
            if (num < num2)
            {
                return -1;
            }
            return 0;
        }

        public GameObject Target;
    }
}
