using System;
using System.Collections.Generic;
using Chat;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using magic;
using Models;
using msg;
using Pet;
using Team;
using UnityEngine;

internal class MainUIController : ControllerBase
{
    public UI_MainView mainView
    {
        get
        {
            return UIManager.GetUIObject<UI_MainView>();
        }
    }

    public void LoadMainView(Action Loadover)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_MainView>("UI_Main", delegate ()
        {
            if (!this.isInitLuaPanel)
            {
                this.isInitLuaPanel = true;
                LuaPanelBase luaPanelBase = new LuaPanelBase(this.mainView.uiPanelRoot.gameObject);
                luaPanelBase.Awake(false);
                UIManager.AddLuaUIPanel(luaPanelBase);
            }
            if (this.mainView != null)
            {
                this.mainView.SwitchBottomUI(false);
                this.refreshTaskListNum = 0;
                Scheduler.Instance.AddTimer(0.7f, true, new Scheduler.OnScheduler(this.RefreshTaskList));
            }
            if (Loadover != null)
            {
                Loadover();
            }
            if (this.mapNameAction != null)
            {
                Scheduler.Instance.AddTimer(0.7f, false, delegate
                {
                    this.mapNameAction();
                });
            }
            if (this.storyNameAction != null)
            {
                Scheduler.Instance.AddTimer(0.7f, false, delegate
                {
                    this.storyNameAction();
                });
            }
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.TryCheckPackageFull", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            });
            CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
            if (manager != null)
            {
                CopyManager copyManager = manager;
                copyManager.OnCopyChangeEvt = (Action)Delegate.Combine(copyManager.OnCopyChangeEvt, new Action(this.CheckOpenLastAdventrue));
            }
            if (this.isGoFightUI)
            {
                LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.ShowFightUI", new object[]
                {
                    Util.GetLuaTable("MainUICtrl")
                });
                GameSystemSettings.SetMainPlayerInBattleState(true);
            }
            //GameObject gameObject = GameObject.Find("UIRoot").FindChild("GMButton");
            //gameObject.GetComponent<GMToolManager>().Initilize();
            this.MainUIEnableDeque();
            Action action = this.onMainShow;
            if (action != null)
            {
                action();
                this.onMainShow = null;
            }
        }, UIManager.ParentType.Main, false);
    }

    public void RefreshTeamInfo()
    {
        if (this.mainView != null)
        {
            this.mainView.CheckViewTask();
            this.mainView.refreshTeamInfo();
        }
    }

    public void RefreshHeadIcon()
    {
        if (this.mainView != null)
        {
            this.mainView.setMainHeadIcon();
            this.mainView.SetMainHeadIcon2();
        }
    }

    public void CopyGuideSpecialHandling(bool benter)
    {
        if (this.mainView != null)
        {
            this.mainView.CopyGuideSpecialHandling(benter);
        }
    }

    public void SetPlayerHeadIcon()
    {
    }

    public void RefreshTaskInfo()
    {
        if (this.mainView != null)
        {
            this.mainView.CheckViewTask();
            this.mainView.switchToTask(null);
        }
    }

    public void RefreshMainPlayerJob(uint level, uint job)
    {
        if (this.mainView != null)
        {
            this.mainView.ResetMainPlayerCareer(level, job);
        }
        if (MainPlayer.Self != null)
        {
            AutoAttack component = MainPlayer.Self.GetComponent<AutoAttack>();
            if (component != null)
            {
                component.SetAutoAttackConfig();
            }
        }
    }

    public void SetBattleEffectVisibility(bool visibility)
    {
        if (this.mainView != null)
        {
        }
    }

    public void VisiteNPC(uint npcid)
    {
        if (this.mainView != null)
        {
            this.mainView.ShorcutVisitNpcByFindPathNew(npcid);
        }
    }

    public void RefreshMainPlayerMp()
    {
        if (this.mainView != null)
        {
            this.mainView.RefreshMainPlayerMp();
            this.mainView.UpdateSkillButtonColor();
        }
    }

    public void NpcWarpMove(MSG_RetNpcWarpMove_SC moveData)
    {
        EntitiesID entryident = default(EntitiesID);
        entryident.Id = moveData.tempid;
        entryident.Etype = CharactorType.NPC;
        Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(entryident) as Npc;
        if (npc != null && npc.CharState == CharactorState.CreatComplete)
        {
            npc.setPetPostionAndDir(new Vector2(moveData.movedata.pos.fx, moveData.movedata.pos.fy), moveData.movedata.dir);
        }
        else
        {
            FFDebug.LogWarning(this, "  pet  is  null   " + moveData.tempid);
        }
    }

    public void RefreshPetInfo()
    {
        if (this.mainView != null)
        {
            this.mainView.RefreshPetInfo();
        }
    }

    public void ResetMainPlayerHp(float thp, float tmaxhp)
    {
        if (this.mainView != null)
        {
            this.mainView.ResetMainPlayerHp(thp, tmaxhp);
        }
    }

    public void LevelUpEffect(ulong id, CharactorType type)
    {
        CharactorBase charactorByID = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(id, type);
        if (charactorByID == null)
        {
            return;
        }
        FFEffectControl component = charactorByID.GetComponent<FFEffectControl>();
        if (component == null)
        {
            FFDebug.LogWarning(this, "FFEffectControl is null");
            return;
        }
        component.AddEffectGroupOnce("levelupeffect");
    }

    public void RefreshEXP(MSG_UpdateExpLevel_SC data)
    {
        this.mExpData = data;
    }

    public uint GetCurLv()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>().GetCurLv();
        }
        if (this.mExpData == null)
        {
            return 0U;
        }
        HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
        if (controller.IsMainHeroOnline())
        {
            return this.mExpData.mainhero_lv;
        }
        return this.mExpData.curlevel;
    }

    public ulong GetCurExp()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>().GetCurExp();
        }
        if (this.mExpData == null)
        {
            return 0UL;
        }
        HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
        if (controller.IsMainHeroOnline())
        {
            return this.mExpData.mainhero_exp;
        }
        return (ulong)this.mExpData.curexp;
    }

    public uint GetSecondLv()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            return 0U;
        }
        if (this.mExpData == null)
        {
            return 0U;
        }
        HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
        if (controller.IsMainHeroOnline())
        {
            return this.mExpData.curlevel;
        }
        return this.mExpData.mainhero_lv;
    }

    public ulong GetSecondExp()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            return 0UL;
        }
        if (this.mExpData == null)
        {
            return 0UL;
        }
        HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
        if (controller.IsMainHeroOnline())
        {
            return (ulong)this.mExpData.curexp;
        }
        return this.mExpData.mainhero_exp;
    }

    public void RefreshPlayerEXP(uint level, uint exp)
    {
        if (this.CheckLevelUp(level))
        {
            this.CheckSkillNewIconShow();
            this.LevelUpSpeciallEffect();
        }
        if (this.mainView != null)
        {
            this.mainView.RefreshExpValue(level, exp);
        }
    }

    public void RefreshHeroEXP(uint level, uint exp)
    {
        if (this.mainView != null)
        {
            this.mainView.RefreshHeroExpValue(level, exp);
        }
    }

    public bool CheckLevelUp(uint level)
    {
        return MainPlayer.Self != null && MainPlayer.Self.OtherPlayerData != null && level > MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.level;
    }

    public void LevelUpSpeciallEffect()
    {
    }

    public void EnterCompetition()
    {
        if (this.mainView != null)
        {
            this.mainView.SetChatMaskActive(false);
        }
    }

    public void ExitCompetition()
    {
        if (this.mainView != null)
        {
            this.mainView.SetChatMaskActive(true);
        }
    }

    public void SetCurOpenChatType(string str)
    {
        this._strOpenChatType = str;
    }

    public void AddChatItem(ChatData data)
    {
    }

    public void AddRightSysLog(ChatData data)
    {
        if (this.mainView != null)
        {
            DateTime now = DateTime.Now;
            this.mainView.ShowSysLog(now.ToString("HH:mm:ss") + " " + data.content);
        }
    }

    public void AddChatItem_Tips(ChatData data)
    {
    }

    public void SetChatViewAndHidden()
    {
        if (this.mainView != null)
        {
            this.mainView.SetChatHidden(this.hiddenChat);
            this.hiddenChat = !this.hiddenChat;
        }
    }

    public void AddMessage(MessageType msgType, int count, Action CallBack)
    {
        if (this.mainView != null)
        {
            this.mainView.AddMessageIcon(msgType, count, CallBack);
        }
    }

    public void ReadMessage(MessageType msgType)
    {
        if (this.mainView != null)
        {
            this.mainView.ReadMessage(msgType);
        }
    }

    public void EnablePetQTE(MSG_NotifyPetQTESkill_SC msgInfo)
    {
        if (this.mainView != null)
        {
            this.mainView.EnableBtnPetQte(msgInfo.leftlasttime, msgInfo.totallasttime, msgInfo.distancetomaster, msgInfo.bosstempid, msgInfo.distanceratio);
        }
    }

    public void DisablePetQTE()
    {
        if (this.mainView != null)
        {
            this.mainView.DisAbleBtnPetQte();
        }
    }

    public void SetMapNameAction(string name, string name_en, string icon)
    {
        this.mapNameAction = delegate ()
        {
            if (this.mainView != null)
            {
                LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.AddMapName", new object[]
                {
                    Util.GetLuaTable("MainUICtrl"),
                    name,
                    name_en,
                    icon
                });
            }
            this.mapNameAction = null;
        };
    }

    public void SetStoryNameAction(string id)
    {
        this.storyNameAction = delegate ()
        {
            if (this.mainView != null)
            {
                LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.AddStoryName", new object[]
                {
                    id
                });
            }
            this.storyNameAction = null;
        };
    }

    public void ChangeMainPanelMenu(uint menuId, bool menuState)
    {
        if (this.mainView != null)
        {
            this.mainView.ChangeMainPanelMenu(menuId, menuState);
        }
    }

    public void ShowTargetInfo(EntitiesID eid, bool isNotInRange = false, Memember memember = null)
    {
        if (this.mainView == null)
        {
            return;
        }
        EntitiesID oldEntry = default(EntitiesID);
        EntitiesID newEntry = default(EntitiesID);
        if (this.mainView.CurOwner != null)
        {
            oldEntry.Id = this.mainView.CurOwner.EID.Id;
            oldEntry.Etype = this.mainView.CurOwner.EID.Etype;
        }
        newEntry.Id = eid.Id;
        newEntry.Etype = eid.Etype;
        if (oldEntry.Id != newEntry.Id)
        {
            if (eid.Etype == CharactorType.NPC)
            {
                this.mainView.ShowTargetNPCInfo(eid.Id);
            }
            else if (isNotInRange)
            {
                this.mainView.ShowTargetPlayerInfoNotInRange(eid.Id, memember);
            }
            else
            {
                this.mainView.ShowTargetPlayerInfo(eid.Id);
            }
        }
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqEntrySelectState(oldEntry, newEntry, false);
    }

    public void CloseTargetInfo()
    {
        if (this.mainView == null)
        {
            return;
        }
        this.mainView.CloseTargetInfo();
    }

    public bool GetNormalTargetActive()
    {
        return !(this.mainView == null) && this.mainView.GetNormalTargetActive();
    }

    public void UpdateTargetRelation(RelationType type, bool isPlayer = false)
    {
        if (this.mainView == null)
        {
            return;
        }
        this.mainView.UpdateSpriteOrColor(type, isPlayer);
    }

    public void UpdateTargetHP(ulong entryid, uint curHP, uint maxHP)
    {
        if (this.mainView == null)
        {
            return;
        }
        this.mainView.UpdateTargetHP(entryid, curHP, maxHP);
    }

    public void UpdateBaseInfo(ulong id, string name, uint level)
    {
        if (this.mainView == null)
        {
            return;
        }
        this.mainView.UpdateBaseInfo(id, name, level);
    }

    public void UpdateTargetBuffIcon(ulong entryid, CharactorType type, List<StateItem> targetList)
    {
        if (this.mainView == null)
        {
            return;
        }
        this.mainView.UpdateTargetBuffIcon(entryid, type, targetList);
    }

    public void UpdateFightValue()
    {
        if (this.mainView == null)
        {
            return;
        }
        this.mainView.RefreshFightValue();
    }

    public void AddSelfBuffIcon(EntitiesID eid, LuaTable bufferConfig, BufferServerDate data)
    {
        if (this.mainView == null)
        {
            return;
        }
        this.mainView.UpdateSelfBuffIcon(eid, bufferConfig, data);
    }

    public void RemoveSelfBuffIcon(EntitiesID eid, LuaTable bufferConfig, BufferServerDate data)
    {
        if (this.mainView == null)
        {
            return;
        }
        this.mainView.RemoveSelfBuffIcon(eid, bufferConfig, data);
    }

    public void OpenAcitvityGuide()
    {
        ActivityController controller = ControllerManager.Instance.GetController<ActivityController>();
        if (controller != null)
        {
            UI_ActivityGuide uiobject = UIManager.GetUIObject<UI_ActivityGuide>();
            if (uiobject == null)
            {
                controller.ShowActivityGuide();
            }
            else
            {
                controller.CloseActivityGuide();
            }
        }
    }

    public void UpdateTeamMemeberBuffIcon(cs_MapUserData MapUserData)
    {
        if (this.mainView != null)
        {
            this.mainView.UpdateTeamMemeberBuffIcon(MapUserData);
        }
    }

    public void CheckOpenCompetition()
    {
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (manager == null)
        {
            return;
        }
        if (manager.InWuDaoHuiCopy)
        {
            ControllerManager.Instance.GetController<PVPCompetitionController>().ReqRankPKCurStage_CS();
        }
        else if (manager.InDeathPKCopy)
        {
            LuaScriptMgr.Instance.CallLuaFunction("CompetitionCtrl.MSG_ReqPvPFightCurStage_CS", new object[]
            {
                Util.GetLuaTable("CompetitionCtrl")
            });
        }
    }

    public void CheckOpenLastAdventrue()
    {
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (manager.InCopy)
        {
            return;
        }
        LuaScriptMgr.Instance.CallLuaFunction("AdventureCtrl.OnOpenLastAdventure", new object[]
        {
            Util.GetLuaTable("AdventureCtrl")
        });
    }

    public void OpenStartMatchBox(float startMatchTime, PvpMatchType pvpMatchType)
    {
        this.mainView.OpenStartMatchBox(startMatchTime, pvpMatchType);
    }

    public void CloseMatchBox()
    {
        this.mainView.CloseMatchBox();
    }

    private void RefreshTaskList()
    {
        this.refreshTaskListNum++;
        if (this.refreshTaskListNum > 5)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.RefreshTaskList));
        }
        if (this.mainView != null)
        {
            this.mainView.setTaskAndTeamSwtich();
        }
    }

    public void ActiveTask(bool active)
    {
        this.mainView.ActiveTask(active);
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
            return "mainui";
        }
    }

    public void MainUIEnableDeque()
    {
        while (this.ActionWhenMainUIEnable.Count > 0)
        {
            Action action = this.ActionWhenMainUIEnable.Dequeue();
            if (action != null)
            {
                action();
            }
        }
    }

    public void AddActionWhenMainViewNotEnable(Action act)
    {
        if (act != null)
        {
            this.ActionWhenMainUIEnable.Enqueue(act);
        }
    }

    public LuaTable GetLvConfig(uint level)
    {
        return LuaConfigManager.GetConfigTable("levelconfig", (ulong)level);
    }

    public bool TryGetLevelAllExp(uint level, out uint exp)
    {
        exp = 0U;
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>().TryGetLevelExp(level, out exp);
        }
        LuaTable lvConfig = this.GetLvConfig(level);
        if (lvConfig == null)
        {
            return false;
        }
        exp = lvConfig.GetField_Uint("levelupexp");
        if (exp < 0U)
        {
            exp = 0U;
        }
        return true;
    }

    public uint GetCurLevelAllExp(uint level)
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>().GetCurLevelExp(level);
        }
        LuaTable lvConfig = this.GetLvConfig(level);
        if (lvConfig == null)
        {
            if (level > 0U)
            {
            }
            return 0U;
        }
        uint num = lvConfig.GetField_Uint("levelupexp");
        if (num < 0U)
        {
            num = 0U;
        }
        return num;
    }

    public string GetNameByCharactorBase(CharactorBase charactorBase)
    {
        if (charactorBase is OtherPlayer)
        {
            return (charactorBase as OtherPlayer).OtherPlayerData.MapUserData.name;
        }
        if (charactorBase is Npc)
        {
            return (charactorBase as Npc).NpcData.MapNpcData.name;
        }
        return string.Empty;
    }

    public void AddBuffLog(BufferServerDate serverData, CharactorBase owner)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("charstate", (ulong)serverData.flag);
        if (configTable == null)
        {
            return;
        }
        if (configTable.GetField_Uint("battleshow") == 0U)
        {
            return;
        }
        if (serverData.flag == UserState.USTATE_NOSTATE)
        {
            return;
        }
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        CharactorBase charactorFromCharid = manager.GetCharactorFromCharid(serverData.giver);
        if (charactorFromCharid != null && serverData.skillid != 0UL)
        {
            uint baseOrHeroId = charactorFromCharid.BaseData.GetBaseOrHeroId();
            uint num = 0U;
            if (!(charactorFromCharid is Npc))
            {
                LuaTable cacheField_Table = LuaConfigManager.GetConfig("heros").GetCacheField_Table(baseOrHeroId);
                if (cacheField_Table != null)
                {
                    num = cacheField_Table.GetField_Uint("normalskill");
                }
            }
            else
            {
                LuaTable cacheField_Table2 = LuaConfigManager.GetConfig("npc_data").GetCacheField_Table(baseOrHeroId);
                if (cacheField_Table2 != null)
                {
                    num = cacheField_Table2.GetField_Uint("skilluuid");
                }
            }
            if ((ulong)num == serverData.skillid)
            {
                return;
            }
            LuaTable luaTable = ManagerCenter.Instance.GetManager<SkillManager>().Gett_skill_lv_config(serverData.skillid);
            if (luaTable != null && luaTable.GetField_Uint("canbe_passive") != 0U)
            {
                return;
            }
        }
        string text = string.Empty;
        if (owner is OtherPlayer)
        {
            if (owner is MainPlayer)
            {
                text = "你受到了";
            }
            else if (charactorFromCharid is MainPlayer)
            {
                string name = (owner as OtherPlayer).OtherPlayerData.MapUserData.name;
                text = "对 " + name + " 造成";
            }
        }
        else
        {
            if (!(owner is Npc) || !(charactorFromCharid is MainPlayer))
            {
                return;
            }
            string name2 = (owner as Npc).NpcData.MapNpcData.name;
            text = "对 " + name2 + " 造成";
        }
        text = text + " " + configTable.GetField_String("name") + " 效果";
        this.ShowBattleLog(text, owner is MainPlayer);
    }

    private string GetDebuffName(List<ATTACKRESULT> attcode)
    {
        string result = string.Empty;
        if (attcode.Contains(ATTACKRESULT.ATTACKRESULT_HOLD))
        {
            result = "招架";
        }
        else if (attcode.Contains(ATTACKRESULT.ATTACKRESULT_BLOCK))
        {
            result = "格挡";
        }
        else
        {
            result = "偏斜";
        }
        return result;
    }

    public void SetSelectToShowTarget(ulong id)
    {
        if (this.mainView != null)
        {
            this.mainView.ShowTargetPlayerInfo(id);
        }
    }

    public void AddSkillLog(MSG_Ret_MagicAttack_SC mdata, PKResult pkr)
    {
        if (pkr.changehp >= 0)
        {
            return;
        }
        string text = string.Empty;
        LuaTable luaTable = ManagerCenter.Instance.GetManager<SkillManager>().Gett_skill_lv_config(mdata.skillstage);
        if (luaTable != null)
        {
            text = " " + luaTable.GetField_String("skillname") + " ";
        }
        string text2 = string.Empty;
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        CharactorBase charactorByID = manager.GetCharactorByID(mdata.att);
        CharactorBase charactorByID2 = manager.GetCharactorByID(pkr.def);
        if (charactorByID is MainPlayer)
        {
            string nameByCharactorBase = this.GetNameByCharactorBase(charactorByID2);
            text2 = "你使用了 " + text + " ";
            if (pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_MISS))
            {
                text2 = text2 + "被 " + nameByCharactorBase + " 闪避";
            }
            else
            {
                if (pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_HOLD) || pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_BLOCK) || pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_DEFLECT))
                {
                    string text3 = text2;
                    text2 = string.Concat(new string[]
                    {
                        text3,
                        "被 ",
                        nameByCharactorBase,
                        " ",
                        this.GetDebuffName(pkr.attcode),
                        ","
                    });
                }
                else
                {
                    text2 = text2 + "对 " + nameByCharactorBase + " 造成";
                }
                if (pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_BANG))
                {
                    text2 += " 暴击 ";
                }
                text2 = text2 + "伤害" + Mathf.Abs(pkr.changehp);
            }
        }
        else
        {
            if (!(charactorByID2 is MainPlayer))
            {
                return;
            }
            if (charactorByID != null)
            {
                string nameByCharactorBase2 = this.GetNameByCharactorBase(charactorByID);
                if (!string.IsNullOrEmpty(nameByCharactorBase2))
                {
                    text2 = nameByCharactorBase2 + " 的";
                }
            }
            if (pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_MISS))
            {
                text2 = text2 + text + "被你闪避了";
            }
            else
            {
                if (pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_HOLD) || pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_BLOCK) || pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_DEFLECT))
                {
                    string text3 = text2;
                    text2 = string.Concat(new string[]
                    {
                        text3,
                        text,
                        "被你 ",
                        this.GetDebuffName(pkr.attcode),
                        ",造成"
                    });
                }
                else
                {
                    text2 = text2 + text + "对你造成";
                }
                if (pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_BANG))
                {
                    text2 += " 暴击 ";
                }
                text2 = text2 + "伤害" + Mathf.Abs(pkr.changehp);
            }
        }
        this.ShowBattleLog(text2, charactorByID2 is MainPlayer);
    }

    public void AddHpChangeLog(MSG_Ret_HpMpPop_SC data)
    {
        if (data.hp_change == 0)
        {
            return;
        }
        string text = string.Empty;
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        CharactorBase charactorByID = manager.GetCharactorByID(data.target);
        CharactorBase charactorBase = null;
        if (data.att != null)
        {
            charactorBase = manager.GetCharactorByID(data.att);
        }
        if (charactorByID is MainPlayer)
        {
            if (data.skillstage > 0UL)
            {
                string text2 = string.Empty;
                LuaTable luaTable = ManagerCenter.Instance.GetManager<SkillManager>().Gett_skill_lv_config(data.skillstage);
                if (luaTable != null)
                {
                    text2 = " " + luaTable.GetField_String("skillname") + " ";
                }
                text = string.Concat(new object[]
                {
                    text2,
                    "为你恢复",
                    data.hp_change,
                    "生命"
                });
            }
            else
            {
                uint userStateFromHash = (uint)CommonTools.GetUserStateFromHash((ulong)((uint)data.state_id));
                LuaTable configTable = LuaConfigManager.GetConfigTable("charstate", (ulong)userStateFromHash);
                if (configTable == null)
                {
                    return;
                }
                if (configTable.GetField_Uint("battleshow") == 0U)
                {
                    return;
                }
                string text3 = string.Empty;
                text3 = " " + configTable.GetField_String("name") + " ";
                text = text3;
                if (data.hp_change > 0)
                {
                    string text4 = text;
                    text = string.Concat(new object[]
                    {
                        text4,
                        "为你恢复",
                        data.hp_change,
                        "生命"
                    });
                }
                else
                {
                    text = text + "对你造成伤害" + Mathf.Abs(data.hp_change);
                }
            }
        }
        else
        {
            if (charactorBase == null || !(charactorBase is MainPlayer))
            {
                return;
            }
            string nameByCharactorBase = this.GetNameByCharactorBase(charactorByID);
            if (data.hp_change > 0)
            {
                text = string.Concat(new object[]
                {
                    "使 ",
                    nameByCharactorBase,
                    " 恢复",
                    data.hp_change
                });
            }
            else
            {
                text = string.Concat(new object[]
                {
                    "对 ",
                    nameByCharactorBase,
                    " 造成伤害",
                    Mathf.Abs(data.hp_change)
                });
            }
        }
        this.ShowBattleLog(text, charactorByID is MainPlayer);
    }

    public void ShowBattleLog(string log, bool targetIsSelf = true)
    {
        if (this.mainView != null)
        {
            log = DateTime.Now.ToString("HH:mm:ss") + " " + log;
            this.mainView.ShowBattleLog(log, (!targetIsSelf) ? Color.white : new Color(0.7647059f, 0.7647059f, 0.7647059f));
        }
    }

    public uint GetIDfor3DIconPosData(uint baseid, uint showid)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)baseid);
        if (configTable != null && string.IsNullOrEmpty(configTable.GetCacheField_String("modelposfor3dicon")))
        {
            configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)showid);
            if (configTable != null && !string.IsNullOrEmpty(configTable.GetCacheField_String("modelposfor3dicon")))
            {
                return showid;
            }
        }
        return baseid;
    }

    public void CheckSkillNewIconShowByMoney(uint moneyNum)
    {
        if (moneyNum > this.oldMoney)
        {
            this.CheckSkillNewIconShow();
        }
        this.oldMoney = moneyNum;
    }

    public void CheckSkillNewIconShow()
    {
        bool flag = false;
        if (this.tryCatchErrored)
        {
            if (this.mainView != null)
            {
                this.mainView.SetSkillBtnShowNew(flag);
            }
            return;
        }
        try
        {
            UnLockSkillsController controller = ControllerManager.Instance.GetController<UnLockSkillsController>();
            ulong thisid = ulong.Parse(MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid);
            int heroIdByThisid = ControllerManager.Instance.GetController<HeroHandbookController>().GetHeroIdByThisid(thisid);
            LuaTable configTable = LuaConfigManager.GetConfigTable("skillshow", (ulong)((long)heroIdByThisid));
            if (configTable != null)
            {
                string field_String = configTable.GetField_String("skill");
                string field_String2 = configTable.GetField_String("breakskill");
                for (int i = 0; i < controller.skillMapList.Count; i++)
                {
                    uint skillId = controller.skillMapList[i].m_skillId;
                    uint skillLv = controller.skillMapList[i].m_skillLv;
                    uint lvupSkillId = controller.GetLvupSkillId(skillId, skillLv);
                    bool flag2 = lvupSkillId > 0U;
                    if (flag2)
                    {
                        flag2 &= controller.IsEnoughGold(skillId, skillLv);
                    }
                    if (flag2)
                    {
                        flag2 &= controller.IsUnLockLevel(lvupSkillId, skillLv + 1U);
                    }
                    if (flag2)
                    {
                        List<LvCostItem> costItemByType = controller.GetCostItemByType(UnLockSkillsController.ItemType.Normal, skillId, skillLv);
                        for (int j = 0; j < costItemByType.Count; j++)
                        {
                            flag2 &= controller.IsEnoughItem(costItemByType[i].m_id, costItemByType[i].m_num, skillId, skillLv);
                        }
                    }
                    flag = flag2;
                    if (flag)
                    {
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (!this.tryCatchErrored)
            {
                FFDebug.LogError(this, ex.ToString());
                this.tryCatchErrored = true;
            }
        }
        finally
        {
            if (this.mainView != null)
            {
                this.mainView.SetSkillBtnShowNew(flag);
            }
        }
    }

    public bool isInitLuaPanel;

    private string _strOpenChatType = string.Empty;

    private Action mapNameAction;

    private Action storyNameAction;

    public int mFriendApplyNum;

    public bool isGoFightUI;

    public Action onMainShow;

    public MSG_UpdateExpLevel_SC mExpData;

    private bool hiddenChat;

    private float configDistance = 5f;

    private int refreshTaskListNum;

    public Queue<Action> ActionWhenMainUIEnable = new Queue<Action>();

    private bool tryCatchErrored;

    private uint oldMoney;
}
