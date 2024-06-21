using System;
using System.Collections.Generic;
using career;
using Framework.Managers;
using LuaInterface;
using magic;

public class MainPlayerSkillHolder : IFFComponent, ISkillHolder
{
    private MainPlayerSkillHolder()
    {
    }

    public static MainPlayerSkillHolder Instance
    {
        get
        {
            if (MainPlayerSkillHolder._instance == null)
            {
                MainPlayerSkillHolder._instance = new MainPlayerSkillHolder();
            }
            return MainPlayerSkillHolder._instance;
        }
    }

    public CompnentState State { get; set; }

    public FFBehaviourControl FFBC
    {
        get
        {
            return this.Owmner.GetComponent<FFBehaviourControl>();
        }
    }

    public SkillManager Skillmgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<SkillManager>();
        }
    }

    public Dictionary<ulong, LuaTable> Stage_configMap
    {
        get
        {
            return this.Skillmgr.Stage_configMap;
        }
    }

    public Dictionary<ulong, LuaTable> Skill_configMap
    {
        get
        {
            return this.Skillmgr.Skill_configMap;
        }
    }

    public MainPlayerSkillBase GetCurAvailableSkill()
    {
        if (this.MainPlayerSkillList.Count == 0)
        {
            return null;
        }
        for (uint num = 2U; num < 6U; num += 1U)
        {
            if (this.mainPlayerEquipSkill.ContainsKey(num))
            {
                if (this.MainPlayerSkillList.ContainsKey(this.mainPlayerEquipSkill[num]))
                {
                    MainPlayerSkillBase mainPlayerSkillBase = this.MainPlayerSkillList[this.mainPlayerEquipSkill[num]];
                    if (mainPlayerSkillBase.CurrState == MainPlayerSkillBase.state.Standby && mainPlayerSkillBase.CheckCanEnter())
                    {
                        return mainPlayerSkillBase;
                    }
                }
            }
        }
        return this.MainPlayerSkillList[this.mainPlayerEquipSkill[1U]];
    }

    public LuaTable Getskill_lv_config(uint Skillid, uint Level)
    {
        ulong skillStageID = this.GetSkillStageID(Skillid, Level);
        if (!this.Skill_configMap.ContainsKey(skillStageID))
        {
            return null;
        }
        return this.Skill_configMap[skillStageID];
    }

    public ulong GetSkillStageID(uint skillid, uint level)
    {
        return ulong.Parse(skillid.ToString()) * 10000UL + (ulong)(level * 10U) + 1UL;
    }

    public ulong GetSkillStageID(uint skillid)
    {
        return this.GetSkillStageID(skillid, 1U);
    }

    public ulong GetSkillID(ulong skillstageid)
    {
        return skillstageid / 10000UL;
    }

    public void LoadMainPlayerSkill(bool blogin = true)
    {
        SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
        if (controller != null)
        {
            this.mainPlayerEquipSkill.Clear();
            controller.InitSkillSlotMap();
            GameMainManager manager = ManagerCenter.Instance.GetManager<GameMainManager>();
            List<careerunlockItem> dicEquipSkill = controller.dicEquipSkill;
            for (int i = 0; i < dicEquipSkill.Count; i++)
            {
                careerunlockItem careerunlockItem = dicEquipSkill[i];
                if (careerunlockItem != null)
                {
                    LuaTable luaTable = MainPlayerSkillHolder.Instance.Getskill_lv_config(careerunlockItem.skill.skillid, careerunlockItem.skill.level);
                    if (luaTable != null && luaTable.GetField_Uint("canbe_passive") == 0U)
                    {
                        if (controller.SkillSlotMap.ContainsKey(careerunlockItem.skill.skillid))
                        {
                            uint key = controller.SkillSlotMap[careerunlockItem.skill.skillid];
                            if (this.mainPlayerEquipSkill.ContainsKey(key))
                            {
                                if (this.mainPlayerEquipSkill[key] != careerunlockItem.skill.skillid)
                                {
                                    this.mainPlayerEquipSkill[key] = careerunlockItem.skill.skillid;
                                }
                            }
                            else
                            {
                                this.mainPlayerEquipSkill[key] = careerunlockItem.skill.skillid;
                            }
                        }
                        this.InstallPlayerSkill(careerunlockItem.skill.skillid, careerunlockItem.skill.level, careerunlockItem.skill.skillcd);
                        manager.PreLoadEffectBySkillId((ulong)careerunlockItem.skill.skillid, careerunlockItem.skill.level);
                    }
                }
            }
            List<uint> list = new List<uint>();
            uint _key;
            foreach (uint key2 in this.MainPlayerSkillList.Keys)
            {
                _key = key2;
                if (dicEquipSkill.Find((careerunlockItem _skill) => _skill.skill.skillid == _key) == null)
                {
                    list.Add(_key);
                }
            }
            list.ForEach(delegate (uint _keyS)
            {
                if (this.MainPlayerSkillList.ContainsKey(_keyS))
                {
                    this.MainPlayerSkillList.Remove(_keyS);
                }
            });
            list.Clear();
            if (blogin)
            {
                ServerStorageManager.Instance.GetData(controller.skillslotkey, 0U);
            }
            controller.RefreshEvolutionSkillSlotInfo(null);
        }
    }

    public void InstallPlayerSkill(uint Skillid, uint Level, uint skillcd)
    {
        LuaTable luaTable = this.Getskill_lv_config(Skillid, Level);
        if (luaTable == null)
        {
            FFDebug.LogError(this, string.Format("InstallPlayerSkill: ID: {0}  skilllevel: {1}  ", Skillid, Level));
            return;
        }
        FFDebug.Log(this, FFLogType.Skill, string.Format("InstallPlayerSkill: ID: {0}  skillname: {1}  releasetype: {2} dtime: {3} chanttime: {4} skillstatus: {5} maxoverlaytimes: {6}   skillcd:{7}", new object[]
        {
            luaTable.GetField_Uint("id"),
            luaTable.GetField_String("skillname"),
            luaTable.GetField_Uint("releasetype"),
            luaTable.GetField_Uint("dtime"),
            luaTable.GetField_Uint("chanttime"),
            luaTable.GetField_String("skillstatus"),
            luaTable.GetField_Uint("maxoverlaytimes"),
            skillcd
        }));
        if (this.MainPlayerSkillList.ContainsKey(Skillid))
        {
            this.MainPlayerSkillList[Skillid].ResetCD(skillcd);
            return;
        }
        if (luaTable.GetField_Uint("releasetype") == 0U || luaTable.GetField_Uint("releasetype") == 7U)
        {
            MainPlayerSkillBase value = new MainPlayerSkillNormal(this, Skillid, Level, skillcd);
            this.MainPlayerSkillList[Skillid] = value;
        }
        else if (luaTable.GetField_Uint("releasetype") == 1U)
        {
            MainPlayerSkillBase value2 = new MainPlayerNormalAttacklCombo(this, Skillid, Level, skillcd);
            this.MainPlayerSkillList[Skillid] = value2;
        }
        else if (luaTable.GetField_Uint("releasetype") == 2U)
        {
            uint cdlength = skillcd - luaTable.GetField_Uint("chanttime");
            MainPlayerSkillBase value3 = new MainPlayerSkillChant(this, Skillid, Level, cdlength);
            this.MainPlayerSkillList[Skillid] = value3;
        }
        else if (luaTable.GetField_Uint("releasetype") == 3U)
        {
            MainPlayerSkillBase value4 = new MainPlaySkillHasState(this, Skillid, Level, skillcd);
            this.MainPlayerSkillList[Skillid] = value4;
        }
        else if (luaTable.GetField_Uint("releasetype") == 4U)
        {
            MainPlayerSkillBase value5 = new MainPlayerSkillCombo(this, Skillid, Level, skillcd);
            this.MainPlayerSkillList[Skillid] = value5;
        }
        else if (luaTable.GetField_Uint("releasetype") == 5U)
        {
            MainPlayerSkillBase value6 = new MainPlaySkillZELASCombo(this, Skillid, Level, skillcd);
            this.MainPlayerSkillList[Skillid] = value6;
        }
        else if (luaTable.GetField_Uint("releasetype") == 6U)
        {
            MainPlayerSkillBase value7 = new MainPlaySkillSpellChannel(this, Skillid, Level, skillcd);
            this.MainPlayerSkillList[Skillid] = value7;
        }
        else
        {
            FFDebug.LogError(this, string.Format("can't  handle skill releasetype: {0}", luaTable.GetField_Uint("releasetype")));
        }
    }

    public void InitCombSkillConfig()
    {
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("skill_data");
    }

    public MainPlayerSkillBase GetMainPlayerSkill(uint skillid)
    {
        MainPlayerSkillBase skillbase = null;
        this.MainPlayerSkillList.BetterForeach(delegate (int index, KeyValuePair<uint, MainPlayerSkillBase> skill)
        {
            if (skill.Value.Skillid == skillid)
            {
                skillbase = skill.Value;
                return false;
            }
            return true;
        });
        return skillbase;
    }

    public void SendSkill(uint Skillid, CharactorBase target = null)
    {
        this.Skillmgr.SendSkill(Skillid, target);
        this.StartDisplaySkill(Skillid);
    }

    public void setInGuide(bool guide)
    {
        this.inGuide = guide;
    }

    public bool ClickSkillEvent(uint Skill)
    {
        if (this.inGuide)
        {
            return false;
        }
        PlayerBufferControl component = this.Owmner.ComponentMgr.GetComponent<PlayerBufferControl>();
        if (component != null)
        {
            if (component.HasBeControlled(BufferState.ControlType.NoSkill))
            {
                return false;
            }
            int cacheField_Int = this.MainPlayerSkillList[Skill].SkillConfig.GetCacheField_Int("canrelieve");
            if (component.HasBeControlled(BufferState.ControlType.NoLieve) && cacheField_Int == 0)
            {
                return false;
            }
        }
        if (this.FFBC.LockState)
        {
            return false;
        }
        if (this.CurrPlayerSkill == null)
        {
            if (this.MainPlayerSkillList[Skill].CheckCanEnter())
            {
                this.CurrPlayerSkill = this.MainPlayerSkillList[Skill];
                this.CurrPlayerSkill.OnSendSkillEvent();
            }
        }
        else if (this.CurrPlayerSkill.Skillid == Skill)
        {
            if (this.MainPlayerSkillList[Skill].CheckCanEnter())
            {
                this.CurrPlayerSkill.OnSendSkillEvent();
            }
        }
        else
        {
            this.CacheNextSkill(Skill);
        }
        return true;
    }

    public void CacheNextSkill(uint Skill)
    {
        this.CancelCacheSkill();
        if (this.CurrPlayerSkill == null)
        {
            this.ClickSkillEvent(Skill);
        }
        else if (this.CurrPlayerSkill.CanBreakMe(Skill))
        {
            this.OnChantStage = true;
            this.NextSkill = Skill;
            this.CurrPlayerSkill.CacheSkill(delegate
            {
                this.CurrPlayerSkill = null;
                this.ClickSkillEvent(this.NextSkill);
                this.NextSkill = 0U;
                this.OnChantStage = false;
            });
        }
    }

    public void CancelCacheSkill()
    {
        if (this.CurrPlayerSkill != null)
        {
            this.CurrPlayerSkill.CancelCacheSkill();
        }
        this.NextSkill = 0U;
    }

    public void OnBreak(CSkillBreakType type)
    {
        if (this.CurrPlayerSkill != null)
        {
            this.CurrPlayerSkill.Break(type);
            this.CurrPlayerSkill = null;
        }
        else
        {
            if (this.progressUIController == null)
            {
                this.progressUIController = ControllerManager.Instance.GetController<ProgressUIController>();
            }
            this.progressUIController.BreakProgressBar();
        }
    }

    public void OnBreak(CSkillBreakType type, uint Skillid)
    {
        if (this.MainPlayerSkillList.ContainsKey(Skillid))
        {
            this.MainPlayerSkillList[Skillid].Break(type);
            if (this.CurrPlayerSkill == this.MainPlayerSkillList[Skillid])
            {
                this.CurrPlayerSkill = null;
            }
        }
        else
        {
            if (this.progressUIController == null)
            {
                this.progressUIController = ControllerManager.Instance.GetController<ProgressUIController>();
            }
            this.progressUIController.BreakProgressBar();
        }
    }

    public void RegiestBreakMsg(Action msg)
    {
        this.UnRegiestBreakMsg(msg);
        this.OnBreakMsg = (Action)Delegate.Combine(this.OnBreakMsg, msg);
    }

    public void UnRegiestBreakMsg(Action msg)
    {
        this.OnBreakMsg = (Action)Delegate.Remove(this.OnBreakMsg, msg);
    }

    public void ResetSkillState()
    {
        this.MainPlayerSkillList.BetterForeach(delegate (KeyValuePair<uint, MainPlayerSkillBase> pair)
        {
            pair.Value.CurrState = MainPlayerSkillBase.state.CD;
        });
    }

    private void SkillGetConfirm(uint Skill)
    {
        if (this.FFBC.CurrState is FFBehaviourState_Skill)
        {
            FFBehaviourState_Skill ffbehaviourState_Skill = this.FFBC.CurrState as FFBehaviourState_Skill;
            foreach (SkillClip skillClip in ffbehaviourState_Skill.SkillClipQueue)
            {
                if ((uint)(skillClip.SkillStageId / 10000UL) == Skill)
                {
                    return;
                }
            }
        }
        if (this.CurrPlayerSkill != null && Skill == this.CurrPlayerSkill.Skillid)
        {
            this.CurrPlayerSkill.OnServerConfirm();
            this.MainPlayerSkillList.BetterForeach(delegate (KeyValuePair<uint, MainPlayerSkillBase> pair)
            {
                pair.Value.OnSkillEnter(this.CurrPlayerSkill);
            });
        }
        else
        {
            FFDebug.LogError(this, string.Format("SkillGetConfirm  ERROR: ", Skill));
        }
    }

    public CharactorBase GetNormalAttackTarget()
    {
        MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        if (component == null)
        {
            return null;
        }
        component.normalattackSelect.ReqTarget();
        bool flag = component.normalattackSelect.CheckLegal(component.TargetCharactor, false);
        if (flag)
        {
            return component.TargetCharactor;
        }
        return null;
    }

    public void ClickSkillAttack(uint skillid, bool bclick = false)
    {
        this.skillAttackAutoAttack.StartAttack(skillid, bclick);
    }

    public void OnChangeAttack()
    {
        this.ChangeNormalAttack();
    }

    public void ChangeNormalAttack()
    {
        MainPlayerSkillBase mainPlayerSkill = this.GetMainPlayerSkill(this.GetNormalSkillID());
        mainPlayerSkill.CurrState = MainPlayerSkillBase.state.Standby;
        this.skillAttackAutoAttack.ChangeNormalAttack();
    }

    public void ClickNormalAttack(uint skillid, bool bclick = false)
    {
        this.ClickSkillAttack(skillid, bclick);
    }

    private void StartDisplaySkill(uint skillid)
    {
        if (this.FFBC == null)
        {
            return;
        }
        if (this.FFBC.CurrState is FFBehaviourState_Skill)
        {
            FFBehaviourState_Skill ffbehaviourState_Skill = this.FFBC.CurrStatebyType<FFBehaviourState_Skill>();
            this.SkillGetConfirm(skillid);
        }
        else
        {
            this.Owmner.StopMoveImmediate(delegate
            {
                this.FFBC.ChangeState(ClassPool.GetObject<FFBehaviourState_Skill>());
                this.SkillGetConfirm(skillid);
            });
        }
    }

    public void UpdateSkillState(List<SkillData> serverDataList)
    {
        for (int i = 0; i < serverDataList.Count; i++)
        {
            SkillData skillData = serverDataList[i];
            if (this.MainPlayerSkillList.ContainsKey(skillData.skillid))
            {
                this.MainPlayerSkillList[skillData.skillid].UpdateStoreTimes(skillData.overlaytimes);
                this.MainPlayerSkillList[skillData.skillid].UpdateSkillState();
            }
        }
    }

    public uint GetAttackSkillIDbystor(uint stor)
    {
        if (this.mainPlayerEquipSkill.Count == 0)
        {
            return 0U;
        }
        uint normalSkillID = this.GetNormalSkillID();
        uint num = 0U;
        foreach (uint num2 in this.mainPlayerEquipSkill.Values)
        {
            if (normalSkillID == num2)
            {
                return normalSkillID;
            }
            num = ((num2 % 100U <= num % 100U) ? num2 : num);
        }
        return num;
    }

    public LuaTable[] GetALLMainPlayerSkillStageArray()
    {
        this.Stage_configTmpList.Clear();
        SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
        if (controller != null)
        {
            Profession occupation = (Profession)MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.occupation;
            List<careerunlockItem> dicEquipSkill = controller.dicEquipSkill;
            for (int i = 0; i < dicEquipSkill.Count; i++)
            {
                uint num = 1U;
                careerunlockItem careerunlockItem = dicEquipSkill[i];
                ulong skillStageID = this.GetSkillStageID(careerunlockItem.skill.skillid, num);
                ulong num2 = skillStageID;
                while (this.Stage_configMap.ContainsKey(num2))
                {
                    while (this.Stage_configMap.ContainsKey(num2))
                    {
                        this.Stage_configTmpList.Add(this.Stage_configMap[num2]);
                        num2 += 1UL;
                    }
                    if (this.Skill_configMap.ContainsKey(skillStageID))
                    {
                        uint cacheField_Uint = this.Skill_configMap[skillStageID].GetCacheField_Uint("nextskillid");
                        if (cacheField_Uint > 0U)
                        {
                            skillStageID = this.GetSkillStageID(cacheField_Uint, num += 1U);
                            num2 = skillStageID;
                        }
                    }
                }
            }
        }
        return this.Stage_configTmpList.ToArray();
    }

    public CharactorBase GetTargetFromEntitiesID(EntitiesID EID)
    {
        return ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(EID);
    }

    public bool IsSightingState()
    {
        return (this.Owmner != null && this.FFBC != null && this.FFBC.CurrState is FFBehaviourState_Skill && (this.OnChantStage || (this.CurrPlayerSkill != null && this.CurrPlayerSkill.mSightState != MainPlayerSkillBase.SightState.None))) || UIBagManager.Instance.IsInSightingStateByObjectSkill();
    }

    public void StartDisplaySkill(MSG_Ret_StartMagicAttack_SC Data)
    {
    }

    public void DisplaySkillStage(MSG_Ret_SyncSkillStage_SC Data)
    {
        if (this.FFBC == null)
        {
            return;
        }
        if (this.FFBC.CurrStatebyType<FFBehaviourState_Skill>() == null)
        {
            return;
        }
        if (this.Skillmgr.IsChantStage(Data.skillstage) && this.CurrPlayerSkill is MainPlayerSkillChant)
        {
            MainPlayerSkillChant mainPlayerSkillChant = this.CurrPlayerSkill as MainPlayerSkillChant;
            mainPlayerSkillChant.StartChant(Data.skillstage);
        }
        if (this.Skillmgr.IsSpellChannelStage(Data.skillstage) && this.CurrPlayerSkill is MainPlaySkillSpellChannel)
        {
            MainPlaySkillSpellChannel mainPlaySkillSpellChannel = this.CurrPlayerSkill as MainPlaySkillSpellChannel;
            mainPlaySkillSpellChannel.StartDisplaySkill(Data.skillstage);
        }
    }

    public void HandleBreakSkill(MSG_Ret_InterruptSkill_SC Data)
    {
        if (this.FFBC == null)
        {
            return;
        }
        if (this.FFBC.CurrState is FFBehaviourState_Skill && (ulong)(this.FFBC.CurrStatebyType<FFBehaviourState_Skill>().CurrSkillClip.AnimConfig.SkillStateId / 1000U) == Data.skillstage / 1000UL)
        {
            this.FFBC.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
        }
        this.OnBreak(CSkillBreakType.ServerInterrupt, (uint)Data.skillstage / 1000U);
    }

    public bool IsNormalSkill(uint skillid)
    {
        LuaTable heroConfig = MainPlayer.Self.OtherPlayerData.GetHeroConfig();
        if (heroConfig != null)
        {
            uint field_Uint = heroConfig.GetField_Uint("normalskill");
            if (skillid.Equals(field_Uint))
            {
                return true;
            }
        }
        return false;
    }

    public uint GetNormalSkillID()
    {
        LuaTable heroConfig = MainPlayer.Self.OtherPlayerData.GetHeroConfig();
        if (heroConfig != null)
        {
            return heroConfig.GetField_Uint("normalskill");
        }
        return 0U;
    }

    public uint GetSkillIndex(uint _skillid)
    {
        return _skillid % 100U;
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owmner = Mgr.Owner;
        this.InitCombSkillConfig();
        this.LoadMainPlayerSkill(true);
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.InitSkillTexture(true);
            mainView.UpdateSkillStorage();
        }
        this.normalAttackAutoAttack = new NormalAttackAutoAttack();
        this.normalAttackAutoAttack.Init();
        this.skillAttackAutoAttack = new SkillAttackAutoAttack();
        this.skillAttackAutoAttack.Init();
    }

    public void CompUpdate()
    {
        if (this.CurrPlayerSkill != null)
        {
            this.CurrPlayerSkill.CheckTargetRange();
            if (this.CurrPlayerSkill.CurrState == MainPlayerSkillBase.state.ReleaseOver)
            {
                this.CurrPlayerSkill.CurrState = MainPlayerSkillBase.state.CD;
                this.CurrPlayerSkill = null;
            }
        }
        ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        this.MainPlayerSkillTmpList = this.MainPlayerSkillList.KeyValuePairs;
        for (int i = 0; i < this.MainPlayerSkillTmpList.Count; i++)
        {
            MainPlayerSkillBase value = this.MainPlayerSkillTmpList[i].Value;
            value.CheckCD(SingletonForMono<GameTime>.Instance.GetCurrServerTime());
            if (value.CurrState == MainPlayerSkillBase.state.ReleaseOver)
            {
                value.CurrState = MainPlayerSkillBase.state.CD;
            }
        }
    }

    public void CompDispose()
    {
        this.MainPlayerSkillList.Clear();
        this.mainPlayerEquipSkill.Clear();
        this.normalAttackAutoAttack.Dispose();
        this.skillAttackAutoAttack.Dispose();
    }

    public void ResetComp()
    {
    }

    private static MainPlayerSkillHolder _instance;

    private CharactorBase Owmner;

    public BetterDictionary<uint, MainPlayerSkillBase> MainPlayerSkillList = new BetterDictionary<uint, MainPlayerSkillBase>();

    public BetterDictionary<uint, uint> mainPlayerEquipSkill = new BetterDictionary<uint, uint>();

    public NormalAttackAutoAttack normalAttackAutoAttack;

    public SkillAttackAutoAttack skillAttackAutoAttack;

    private List<uint> listSkillold = new List<uint>();

    public bool OnChantStage;

    public MainPlayerSkillBase CurrPlayerSkill;

    private bool inGuide;

    public uint NextSkill;

    private ProgressUIController progressUIController;

    public Action OnBreakMsg;

    private List<LuaTable> Stage_configTmpList = new List<LuaTable>();

    private List<KeyValuePair<uint, MainPlayerSkillBase>> MainPlayerSkillTmpList;

    public class CombSkillConfogComparer : IComparer<LuaTable>
    {
        public int Compare(LuaTable item1, LuaTable item2)
        {
            return item1.GetField_Uint("id").CompareTo(item2.GetField_Uint("id"));
        }
    }
}
