using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using map;
using UnityEngine;

public class MainPlayerSkillBase
{
    public MainPlayerSkillBase(MainPlayerSkillHolder Holder, uint skillid, uint level, uint cdlength)
    {
        this.SkillHolder = Holder;
        this.Skillmgr = this.SkillHolder.Skillmgr;
        this.Skillid = skillid;
        this.NormalSkill = (Holder.GetNormalSkillID() == skillid);
        this.Level = level;
        this._cdlength = cdlength;
        this.Wholeskillid = (ulong)this.Skillid * 10000UL + (ulong)(this.Level * 10U) + 1UL;
        if (this.SkillHolder.Skill_configMap.ContainsKey(this.Wholeskillid))
        {
            this.skillconfig = this.SkillHolder.Skill_configMap[this.Wholeskillid];
        }
        else
        {
            this.skillconfig = null;
        }
        ulong num = this.Wholeskillid;
        while (Holder.Stage_configMap.ContainsKey(num))
        {
            this.Stage_configList.Add(Holder.Stage_configMap[num]);
            num += 1UL;
        }
        this.InitSightType(0);
        this.PreloadEffect();
        this.InitStorageType();
        this.IconNames = this.SkillConfig.GetField_String("skillicon").Split(new char[]
        {
            ','
        }, StringSplitOptions.RemoveEmptyEntries);
        this.breakAutoAttack = this.SkillConfig.GetCacheField_Bool("canbe_breakattack");
        this.canBePassive = (this.SkillConfig.GetCacheField_Int("canbe_passive") == 0);
        this.needTarget = this.SkillConfig.GetField_Bool("NeedTarget");
    }

    public bool NormalSkill
    {
        get
        {
            return this.normalSkill;
        }
        set
        {
            this.normalSkill = value;
        }
    }

    public uint IStorageTimes
    {
        get
        {
            return this._iStorageTimes;
        }
        set
        {
            this._iStorageTimes = value;
            if (this.mSorageType == SkillSorageType.CDAfterRelase && this._iStorageTimes == 0U)
            {
                this._iStorageTimes = this.IMaxStorageTimes;
                this.ActivateCD();
            }
        }
    }

    public uint IMaxStorageTimes
    {
        get
        {
            return this.ServerData.IMaxStorageTimes;
        }
    }

    public virtual string GetCurSkillIconName()
    {
        if (this.IconNames.Length == 0)
        {
            FFDebug.LogError(this, "Wholeskillid:" + this.Wholeskillid + " IconNames.Length == 0");
            return string.Empty;
        }
        return this.IconNames[0];
    }

    public virtual bool GetCanBePassive()
    {
        return this.canBePassive;
    }

    public bool BreakAutoAttack()
    {
        return this.breakAutoAttack;
    }

    public SkillServerData ServerData
    {
        get
        {
            if (!this.Skillmgr.SkillServerDataMap.ContainsKey(this.Skillid))
            {
                this.Skillmgr.SkillServerDataMap[this.Skillid] = new SkillServerData();
                this.Skillmgr.SkillServerDataMap[this.Skillid].Skillid = this.Skillid;
            }
            return this.Skillmgr.SkillServerDataMap[this.Skillid];
        }
    }

    public PublicCDData PCdData
    {
        get
        {
            if (this.spcdc == null)
            {
                this.spcdc = MainPlayer.Self.GetComponent<SkillPublicCDControl>();
            }
            if (this.spcdc != null)
            {
                return this.spcdc.GetPublicCDDataByGroup(this.SkillConfig.GetCacheField_Uint("publicCD"));
            }
            return null;
        }
    }

    public MainPlayerTargetSelectMgr TargetSelectMgr
    {
        get
        {
            if (this.m_targetSelectMgr == null)
            {
                this.m_targetSelectMgr = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            }
            return this.m_targetSelectMgr;
        }
    }

    public LuaTable SkillConfig
    {
        get
        {
            return this.skillconfig;
        }
    }

    public LuaTable LastStage
    {
        get
        {
            return this.Stage_configList[this.Stage_configList.Count - 1];
        }
    }

    public LuaTable FirstStage
    {
        get
        {
            return this.Stage_configList[0];
        }
    }

    public float AttackRange
    {
        get
        {
            if (this.attackrange >= 0f)
            {
                return this.attackrange;
            }
            if (this.SkillConfig != null)
            {
                string field_String = this.SkillConfig.GetField_String("SkillRange");
                string[] array = field_String.Split(new char[]
                {
                    ':'
                });
                if (array.Length > 1)
                {
                    this.attackrange = float.Parse(array[1]);
                }
            }
            return this.attackrange;
        }
    }

    public ulong GetStagePartId(uint index)
    {
        Debug.LogError("Wholeskillid: " + (this.Wholeskillid - 1UL + (ulong)index));
        return this.Wholeskillid - 1UL + (ulong)index;
    }

    public TileFlag GetStageRushBlock(ulong Stageid)
    {
        if (this.SkillHolder.Stage_configMap.ContainsKey(Stageid) && this.Skillmgr.SkillNeedMove(Stageid) && this.SkillHolder.Skill_configMap.ContainsKey(Stageid))
        {
            LuaTable luaTable = this.SkillHolder.Skill_configMap[Stageid];
            if (luaTable.GetField_Uint("usetype") == 9U)
            {
                return (TileFlag)10;
            }
        }
        return (TileFlag)11;
    }

    public LuaTable GetStage(uint index)
    {
        return this.Stage_configList[(int)(index - 1U)];
    }

    public ulong LastSkillTime
    {
        get
        {
            return this.ServerData.Lastusetime;
        }
        set
        {
            this.ServerData.Lastusetime = value;
        }
    }

    public ulong LastUpdateTime
    {
        get
        {
            return this.ServerData.Lastupdatetime;
        }
        set
        {
            this.ServerData.Lastupdatetime = value;
        }
    }

    public virtual void OnSendSkillEvent()
    {
    }

    public void ActivateCD()
    {
        this.LastSkillTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
    }

    public void ActivatePublicCD()
    {
        if (this.PCdData != null)
        {
            this.PCdData.ActivateCD(this.SkillConfig.GetField_Uint("skillid"));
        }
    }

    public void CheckStorageSkillIsCD()
    {
        if (this.IsStorageType && this.mSorageType == SkillSorageType.CDAfterRelase && this.IStorageTimes == this.IMaxStorageTimes)
        {
            ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
            if (currServerTime < this.CDLength + this.LastSkillTime)
            {
                this.CurrState = MainPlayerSkillBase.state.CD;
            }
        }
    }

    public virtual void OnServerConfirm()
    {
        this.CurrState = MainPlayerSkillBase.state.Release;
        if (this.SkillConfig.GetField_Uint("releasetype") != 4U && this.SkillConfig.GetField_Uint("releasetype") != 5U)
        {
            this.ActivateCD();
            this.ActivatePublicCD();
        }
    }

    public virtual void OnSkillEnter(MainPlayerSkillBase skillbase)
    {
    }

    public virtual bool CanBreakMe(uint NextSkill)
    {
        return this.LastStage.GetField_Bool("CanCancelCloseFist");
    }

    public virtual void CallNextSkill()
    {
        if (this.NextSkillAction != null)
        {
            if (this.LastStage.GetField_Bool("CanCancelCloseFist"))
            {
                this.CurrState = MainPlayerSkillBase.state.CD;
                this.NextSkillAction();
            }
            this.NextSkillAction = null;
        }
    }

    public bool HasNextSkillAction
    {
        get
        {
            return this.NextSkillAction != null;
        }
    }

    public virtual void CancelCacheSkill()
    {
        this.NextSkillAction = null;
    }

    public virtual void CacheSkill(Action callback)
    {
        this.NextSkillAction = callback;
    }

    public void ShowNoEnoughMpTip()
    {
        if (MainPlayer.Self != null && MainPlayer.Self.BaseData != null)
        {
            LuaTable heroConfig = MainPlayer.Self.OtherPlayerData.GetHeroConfig();
            if (heroConfig != null)
            {
                LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("MpNotEnoughTip");
                string cacheField_String = heroConfig.GetCacheField_String("addmpcfg");
                uint cacheField_Uint = cacheField_Table.GetCacheField_Table(cacheField_String).GetCacheField_Uint("tip");
                TipsWindow.ShowWindow(CommonUtil.GetText(cacheField_Uint));
            }
        }
    }

    public virtual bool CheckCanEnter()
    {
        if (!this.CheckOutCondition())
        {
            return false;
        }
        if (!this.CheckMpEnough())
        {
            this.ShowNoEnoughMpTip();
            return false;
        }
        if (!this.CheckTarget())
        {
            TipsWindow.ShowWindow(TipsType.NO_TARGET, null);
            return false;
        }
        if (this.IsStorageType && this.IStorageTimes < 1U)
        {
            return false;
        }
        if (ManagerCenter.Instance.GetManager<GameScene>().isAbattoirScene)
        {
            AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
            return controller.CheckCanEnter(this.Skillid);
        }
        return true;
    }

    private bool CheckOutCondition()
    {
        return !MainPlayer.Self.OnRestTime;
    }

    public virtual void UpdateSkillState()
    {
    }

    public virtual void UpdateStoreTimes(uint times)
    {
    }

    public MainPlayerSkillBase.state CurrState
    {
        get
        {
            return this._currState;
        }
        set
        {
            if (this._currState == value)
            {
                return;
            }
            if (value == MainPlayerSkillBase.state.CD)
            {
                ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
                if (currServerTime > this.CDLength + this.LastSkillTime)
                {
                    if (this.PCdData != null)
                    {
                        if (this.PCdData.ActivateCDSkill == this.SkillConfig.GetField_Uint("skillid"))
                        {
                            this._currState = MainPlayerSkillBase.state.Standby;
                        }
                        else if (!this.PCdData.CheckPublicCD())
                        {
                            this._currState = MainPlayerSkillBase.state.Standby;
                        }
                        else
                        {
                            this._currState = value;
                        }
                    }
                    else
                    {
                        this._currState = MainPlayerSkillBase.state.Standby;
                    }
                }
                else
                {
                    this._currState = value;
                }
            }
            else
            {
                this._currState = value;
            }
            FFDebug.Log(this, FFLogType.Skill, string.Format("MainPlayerSkill: {0} ChangeState->{1}", this.Skillid, this._currState));
        }
    }

    public bool CheckMpEnough()
    {
        return MainPlayerSkillBase.NoCD || (this.SkillConfig.GetField_Uint("releasetype") == 4U && this.SkillConfig.GetField_Uint("releasetype") == 5U) || MainPlayer.Self.MainPlayeData.AttributeData.mp >= this.SkillConfig.GetField_Uint("magiccost");
    }

    public CharactorBase GetTarget(bool searchifnull, bool forcesearch = false)
    {
        if (this.TargetSelectMgr == null)
        {
            return null;
        }
        this.TargetSelectMgr.skillattackSelect.SetSelectType(this.SkillConfig.GetField_Uint("SearchType"));
        if (searchifnull)
        {
            bool ignoredeath = this.SkillConfig.GetField_Uint("usetype") == 11U;
            if (this.TargetSelectMgr.skillattackSelect.CheckLegal(this.TargetSelectMgr.TargetCharactor, ignoredeath))
            {
                return this.TargetSelectMgr.TargetCharactor;
            }
            if (!(this.TargetSelectMgr.TargetCharactor is MainPlayer) && this.TargetSelectMgr.skillattackSelect.CheckLegal(MainPlayer.Self, false))
            {
                return MainPlayer.Self;
            }
            if (this.SkillConfig.GetField_Uint("CastBeyondType") != 2U && this.SkillConfig.GetField_Uint("CastBeyondType") != 1U && (!this.TargetSelectMgr.skillattackSelect.CheckLegalCharactor(this.TargetSelectMgr.TargetCharactor, true) || forcesearch))
            {
                this.TargetSelectMgr.skillattackSelect.ReqTarget();
                if (this.TargetSelectMgr.skillattackSelect.CheckLegal(this.TargetSelectMgr.TargetCharactor, false))
                {
                    return this.TargetSelectMgr.TargetCharactor;
                }
            }
        }
        else if (this.TargetSelectMgr.skillattackSelect.CheckLegal(this.TargetSelectMgr.TargetCharactor, this.SkillConfig.GetField_Uint("usetype") == 11U))
        {
            return this.TargetSelectMgr.TargetCharactor;
        }
        return null;
    }

    public bool CheckTarget()
    {
        return !this.SkillConfig.GetField_Bool("NeedTarget") || this.GetTarget(true, false) != null;
    }

    public bool CheckLegal(CharactorBase cb)
    {
        if (this.TargetSelectMgr != null)
        {
            this.TargetSelectMgr.skillattackSelect.SetSelectType(this.SkillConfig.GetField_Uint("SearchType"));
            return this.TargetSelectMgr.skillattackSelect.CheckLegal(this.TargetSelectMgr.TargetCharactor, false);
        }
        return false;
    }

    public virtual void CheckTargetRange()
    {
    }

    public void CheckCD(ulong Now)
    {
        if ((this.CurrState == MainPlayerSkillBase.state.CD || this.CurrState == MainPlayerSkillBase.state.Standby) && Now > this.CDLength + this.LastSkillTime)
        {
            if (this.PCdData != null)
            {
                if (this.PCdData.ActivateCDSkill == this.SkillConfig.GetCacheField_Uint("skillid"))
                {
                    this.CurrState = MainPlayerSkillBase.state.Standby;
                }
                else if (!this.PCdData.CheckPublicCD())
                {
                    this.CurrState = MainPlayerSkillBase.state.Standby;
                }
                else
                {
                    this.CurrState = MainPlayerSkillBase.state.CD;
                }
            }
            else
            {
                this.CurrState = MainPlayerSkillBase.state.Standby;
            }
        }
    }

    public ulong CDLength
    {
        get
        {
            if (MainPlayerSkillBase.NoCD)
            {
                return 0UL;
            }
            return (ulong)this._cdlength;
        }
    }

    public uint GetCDLeft(ulong Now)
    {
        if (this.CDLength == 0UL)
        {
            return 0U;
        }
        if (Now - this.LastSkillTime > this.CDLength)
        {
            return 0U;
        }
        return (uint)(this.CDLength - (Now - this.LastSkillTime));
    }

    public void ResetCD(uint cdlength)
    {
        if (this._cdlength / 2U == cdlength)
        {
            ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
            if (currServerTime - this.LastSkillTime < this.CDLength)
            {
                this.LastSkillTime = (currServerTime - this.LastSkillTime) / 2UL + this.LastSkillTime;
            }
        }
        this._cdlength = cdlength;
    }

    public uint GetStorageCDLeft(ulong Now)
    {
        uint result = 0U;
        if (this.CDLength == 0UL)
        {
            return result;
        }
        if (Now - this.LastUpdateTime > this.CDLength)
        {
            return result;
        }
        return (uint)(this.CDLength - (Now - this.LastUpdateTime));
    }

    public virtual void Break(CSkillBreakType type)
    {
        this.NextSkillAction = null;
        this.SetEffectOver();
        this.SkillHolder.skillAttackAutoAttack.OnBreak();
        this._currState = MainPlayerSkillBase.state.CD;
        if (this.SkillHolder.OnBreakMsg != null)
        {
            this.SkillHolder.OnBreakMsg();
        }
    }

    public void SetEffectOver()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        FFEffectControl component = MainPlayer.Self.GetComponent<FFEffectControl>();
        if (component == null)
        {
            return;
        }
        string[] array = new string[0];
        for (int i = 0; i < this.Stage_configList.Count; i++)
        {
            LuaTable luaTable = this.Stage_configList[i];
            FFActionClip[] ffactionClipArr = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClipArr(MainPlayer.Self.animatorControllerName, luaTable.GetField_Uint("ActionID"));
            for (int j = 0; j < ffactionClipArr.Length; j++)
            {
                array = ffactionClipArr[j].GetEffectsByGroupID(FFActionClip.EffectType.Type_Skill, luaTable.GetCacheField_Uint("EffectId"));
                for (int k = 0; k < array.Length; k++)
                {
                    component.SetEffectOver(array[k]);
                }
                array = ffactionClipArr[j].GetEffectsByGroupID(FFActionClip.EffectType.Type_Fly, 1U);
                for (int l = 0; l < array.Length; l++)
                {
                    component.SetEffectOver(array[l]);
                }
            }
        }
    }

    public void PreloadEffect()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < this.Stage_configList.Count; i++)
        {
            LuaTable luaTable = this.Stage_configList[i];
            FFActionClip[] ffactionClipArr = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClipArr(MainPlayer.Self.animatorControllerName, luaTable.GetField_Uint("ActionID"));
            for (int j = 0; j < ffactionClipArr.Length; j++)
            {
                this.PreloadSkillActionClipEffect(ffactionClipArr[j], list);
            }
        }
        for (int k = 0; k < list.Count; k++)
        {
            ManagerCenter.Instance.GetManager<FFEffectManager>().LoadEffobj(list[k], delegate
            {
            });
        }
    }

    private void PreloadSkillActionClipEffect(FFActionClip clip, List<string> EffectNameList)
    {
        if (clip == null)
        {
            return;
        }
        string[] effectsByGroupID = clip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Skill, 1U);
        for (int i = 0; i < effectsByGroupID.Length; i++)
        {
            EffectClip clip2 = ManagerCenter.Instance.GetManager<FFEffectManager>().GetClip(effectsByGroupID[i]);
            if (!(clip2 == null))
            {
                if (!EffectNameList.Contains(clip2.EffectName))
                {
                    EffectNameList.Add(clip2.EffectName);
                }
            }
        }
        string[] effectsByGroupID2 = clip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Hit, 1U);
        for (int j = 0; j < effectsByGroupID2.Length; j++)
        {
            EffectClip clip3 = ManagerCenter.Instance.GetManager<FFEffectManager>().GetClip(effectsByGroupID2[j]);
            if (!(clip3 == null))
            {
                if (!EffectNameList.Contains(clip3.EffectName))
                {
                    EffectNameList.Add(clip3.EffectName);
                }
            }
        }
    }

    private bool IsOnPeaceState()
    {
        if (MainPlayer.Self != null)
        {
            PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
            if (component != null)
            {
                return component.ContainsState(UserState.USTATE_PEACE);
            }
        }
        return false;
    }

    public void SendSkill()
    {
        if (this.IsOnPeaceState())
        {
            TipsWindow.ShowWindow("can't send skill on peace state.");
            return;
        }
        if (this.BreakAutoAttack() && MainPlayer.Self != null)
        {
            MainPlayer.Self.SwitchAutoAttack(false);
        }
        CharactorBase target = null;
        if (this.SkillConfig.GetField_Bool("NeedTarget"))
        {
            target = this.GetTarget(true, false);
        }
        this.SkillHolder.SendSkill(this.Skillid, target);
    }

    public void SendSkillStage(ulong SkillStageid, uint type, Vector3 Pos, [Optional] EntitiesID EID)
    {
        LuaTable luaTable = ManagerCenter.Instance.GetManager<SkillManager>().Gett_skill_stage_config(SkillStageid);
        if (luaTable.GetField_Bool("NeedFaceTarget") && type != 2U)
        {
            this.FaceTarget(false);
        }
        if (type != 2U)
        {
            this.CheckPlaySkillPortrait(luaTable);
        }
        CharactorBase target = null;
        if (EID == default(EntitiesID))
        {
            if (this.TargetSelectMgr.TargetCharactor != null)
            {
                if (this.TargetSelectMgr.TargetCharactor != null && this.SkillConfig.GetField_Bool("NeedTarget"))
                {
                    target = this.GetTarget(true, false);
                }
            }
            else if (this.SkillConfig.GetField_Bool("NeedTarget") && this.TargetSelectMgr.skillattackSelect.CheckLegal(MainPlayer.Self, false))
            {
                target = MainPlayer.Self;
            }
        }
        else
        {
            target = this.SkillHolder.GetTargetFromEntitiesID(EID);
        }
        this.Skillmgr.SendSkillStage(SkillStageid, type, Pos, MainPlayer.Self.ServerDir, target);
    }

    public void CheckPlaySkillPortrait(LuaTable config)
    {
        if (config == null)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.GetField_String("SkillPortrait")))
        {
            return;
        }
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            try
            {
                string[] array = config.GetField_String("SkillPortrait").Split(new char[]
                {
                    ':'
                });
                uint length = uint.Parse(array[1]);
                mainView.mskillShowImage.ShowSkillImage(array[0], length);
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, ex.ToString() + "stageid:" + config.GetField_ULong("id"));
            }
        }
    }

    public Vector2 GetTargetPos(LuaTable SkillStage, string configname, out bool HasFail)
    {
        HasFail = false;
        Vector2 vector = Vector2.zero;
        int[] skillMoveParam = this.GetSkillMoveParam(SkillStage.GetField_String(configname));
        if (SkillManager.HasSelectPos(SkillStage))
        {
            SkillSelectRangeEffect component = MainPlayer.Self.GetComponent<SkillSelectRangeEffect>();
            if (component != null)
            {
                FFDebug.Log(this, FFLogType.Skill, string.Format("SkillStage.NeedFaceTarget: ", SkillStage.GetField_Bool("NeedFaceTarget")));
                if (SkillStage.GetField_Bool("NeedFaceTarget"))
                {
                    this.FaceTarget(true);
                }
                vector = GraphUtils.GetServerPosByWorldPos(component.SelectPos, false);
            }
            else
            {
                FFDebug.LogError(this, "SkillBTN  SkillSelectRangeEffect  NULL");
            }
        }
        else if ((skillMoveParam[0] == 0 || skillMoveParam[0] == 4) && skillMoveParam[2] > 0)
        {
            if (SkillStage.GetField_Bool("NeedFaceTarget"))
            {
                this.FaceTarget(true);
            }
            float num = (float)skillMoveParam[2];
            if (skillMoveParam[0] == 4)
            {
                float num2 = this.ReSetSelectPosByRectanglesearch(vector, num, this.Sightradius);
                if (num.FloatEqual(num2))
                {
                    HasFail = true;
                }
                num = num2;
            }
            vector = MainPlayer.Self.GetPosBySelf((float)skillMoveParam[1], num);
        }
        else if (skillMoveParam[0] == 1)
        {
            CharactorBase target = this.GetTarget(false, false);
            if (SkillStage.GetField_Bool("NeedFaceTarget"))
            {
                this.FaceTarget(true);
            }
            if (target != null)
            {
                if (target.CurrMoveData != null)
                {
                    float[] skillRangeParam = this.GetSkillRangeParam(this.SkillConfig.GetField_String("SkillRange"));
                    float num3 = Vector2.Distance(target.CurrServerPos, MainPlayer.Self.CurrServerPos);
                    float num4 = 0f;
                    if (target is Npc)
                    {
                        num4 = LuaConfigManager.GetConfigTable("npc_data", (ulong)(target as Npc).NpcData.MapNpcData.baseid).GetField_Uint("volume");
                        num3 -= num4;
                    }
                    if (num3 >= 0f)
                    {
                        vector = MainPlayer.Self.GetPosByTarget(target.CurrServerPos, (float)skillMoveParam[2] - num4);
                    }
                }
                else
                {
                    FFDebug.LogError(this, string.Format("Target.CurrMoveData null", new object[0]));
                }
            }
        }
        else if (skillMoveParam[0] == 3)
        {
            CharactorBase target2 = this.GetTarget(false, false);
            if (SkillStage.GetField_Bool("NeedFaceTarget"))
            {
                this.FaceTarget(true);
            }
            if (target2 != null)
            {
                float[] skillRangeParam2 = this.GetSkillRangeParam(this.SkillConfig.GetField_String("SkillRange"));
                if (target2.CurrMoveData != null)
                {
                    float num5 = Vector2.Distance(target2.CurrServerPos, MainPlayer.Self.CurrServerPos);
                    float num6 = 0f;
                    if (target2 is Npc)
                    {
                        num6 = LuaConfigManager.GetConfigTable("npc_data", (ulong)(target2 as Npc).NpcData.MapNpcData.baseid).GetField_Uint("volume");
                        num5 -= num6;
                    }
                    if (num5 > skillRangeParam2[0] && num5 <= skillRangeParam2[1] + 1.42f)
                    {
                        vector = MainPlayer.Self.GetPosByTarget(target2.CurrServerPos, (float)skillMoveParam[2] - num6);
                    }
                }
                else
                {
                    FFDebug.LogError(this, string.Format("Target.CurrMoveData null", new object[0]));
                }
            }
            else
            {
                vector = MainPlayer.Self.GetPosBySelf(0f, (float)skillMoveParam[1]);
            }
        }
        else if (skillMoveParam[0] == 5)
        {
            vector = this.LastPos;
        }
        else if (skillMoveParam[0] == 6)
        {
            vector = this.orginalSelfPos;
            this.orginalSelfPos = Vector2.zero;
        }
        if (vector != Vector2.zero)
        {
            GraphUtils.FindPosForRush(MainPlayer.Self.CurrServerPos, vector, out vector, this.GetStageRushBlock(SkillStage.GetField_ULong("id")), (TileFlag)11);
        }
        this.LastPos = vector;
        return vector;
    }

    public CharactorBase GetTargetObj(LuaTable SkillStage)
    {
        CharactorBase charactorBase = null;
        int[] skillMoveParam = this.GetSkillMoveParam(SkillStage.GetField_String("MoveDis"));
        if ((skillMoveParam[0] != 0 && skillMoveParam[0] != 4) || skillMoveParam[2] <= 0)
        {
            if (skillMoveParam[0] == 1)
            {
                CharactorBase target = this.GetTarget(false, false);
                if (SkillStage.GetField_Bool("NeedFaceTarget"))
                {
                    this.FaceTarget(true);
                }
                if (target != null)
                {
                    if (target.CurrMoveData != null)
                    {
                        float[] skillRangeParam = this.GetSkillRangeParam(this.SkillConfig.GetField_String("SkillRange"));
                        float num = Vector2.Distance(target.CurrServerPos, MainPlayer.Self.CurrServerPos);
                        if (target is Npc)
                        {
                            float num2 = LuaConfigManager.GetConfigTable("npc_data", (ulong)(target as Npc).NpcData.MapNpcData.baseid).GetField_Uint("volume");
                            num -= num2;
                        }
                        if (num >= 0f)
                        {
                            charactorBase = target;
                        }
                    }
                    else
                    {
                        FFDebug.LogError(this, string.Format("Target.CurrMoveData null", new object[0]));
                    }
                }
            }
            else if (skillMoveParam[0] == 3)
            {
                CharactorBase target2 = this.GetTarget(false, false);
                if (SkillStage.GetField_Bool("NeedFaceTarget"))
                {
                    this.FaceTarget(true);
                }
                if (target2 != null)
                {
                    float[] skillRangeParam2 = this.GetSkillRangeParam(this.SkillConfig.GetField_String("SkillRange"));
                    if (target2.CurrMoveData != null)
                    {
                        float num3 = Vector2.Distance(target2.CurrServerPos, MainPlayer.Self.CurrServerPos);
                        if (target2 is Npc)
                        {
                            float num4 = LuaConfigManager.GetConfigTable("npc_data", (ulong)(target2 as Npc).NpcData.MapNpcData.baseid).GetField_Uint("volume");
                            num3 -= num4;
                        }
                        if (num3 > skillRangeParam2[0] && num3 <= skillRangeParam2[1] + 1.42f)
                        {
                            charactorBase = target2;
                        }
                    }
                }
            }
        }
        return (charactorBase != null) ? charactorBase : null;
    }

    private float ReSetSelectPosByRectanglesearch(Vector2 pos, float Length, float width)
    {
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        EntitiesManager.SeachResult[] array = manager.RectangleSeachBaseMainPlayer(Length, width);
        EntitiesManager.SeachResult seachResult = null;
        this.TargetSelectMgr.skillattackSelect.SetSelectType(this.SkillConfig.GetField_Uint("SearchType"));
        int i = 0;
        while (i < array.Length)
        {
            EntitiesManager.SeachResult seachResult2 = array[i];
            if (seachResult2.Char.EID.Etype != CharactorType.NPC)
            {
                goto IL_9B;
            }
            Npc npc = seachResult2.Char as Npc;
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
            if (!configTable.GetField_Bool("not_beselect"))
            {
                goto IL_9B;
            }
        IL_DB:
            i++;
            continue;
        IL_9B:
            if (!this.TargetSelectMgr.skillattackSelect.CheckLegal(seachResult2.Char, false))
            {
                goto IL_DB;
            }
            if (seachResult == null)
            {
                seachResult = seachResult2;
                goto IL_DB;
            }
            if (seachResult2.length < seachResult.length)
            {
                seachResult = seachResult2;
                goto IL_DB;
            }
            goto IL_DB;
        }
        if (seachResult != null)
        {
            return seachResult.length;
        }
        return Length;
    }

    private int[] GetSkillMoveParam(string ParamStr)
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

    public float[] GetSkillRangeParam(string ParamStr)
    {
        string[] array = ParamStr.Split(new char[]
        {
            ':'
        });
        return new float[]
        {
            float.Parse(array[0]),
            float.Parse(array[1])
        };
    }

    public void FaceTarget(bool immediately)
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        Vector3 vector = Vector3.zero;
        if (this.mSightType == MainPlayerSkillBase.SightType.Circle || this.mSightType == MainPlayerSkillBase.SightType.Sector || this.mSightType == MainPlayerSkillBase.SightType.Rectangle)
        {
            SkillSelectRangeEffect component = MainPlayer.Self.GetComponent<SkillSelectRangeEffect>();
            if (component != null)
            {
                vector = component.SelectPos;
            }
            FFDebug.Log(this, FFLogType.Skill, string.Format("FaceTarget targetpos: {0}", vector));
        }
        else
        {
            CharactorBase target = this.GetTarget(false, false);
            if (target == null)
            {
                return;
            }
            vector = new Vector3(target.ModelObj.transform.position.x, 0f, target.ModelObj.transform.position.z);
        }
        MainPlayer.Self.SetPlayerLookAt(vector, immediately);
    }

    public void ForceChangeState(MainPlayerSkillBase.state _newState)
    {
        if (this._currState != _newState)
        {
            this._currState = _newState;
        }
    }

    protected void InitSightType(int index)
    {
        try
        {
            if (index < this.Stage_configList.Count)
            {
                string[] array = this.Stage_configList[index].GetField_String("SightType").Split(new char[]
                {
                    ':'
                });
                float num = 0f;
                if (!float.TryParse(array[0], out num))
                {
                    FFDebug.LogError(this, "MainPlayerSillBase InitSightType SightType Error, skillid = " + this.Skillid.ToString() + ", Param[0] = " + array[0]);
                }
                else
                {
                    this.mSightType = (MainPlayerSkillBase.SightType)num;
                    FFDebug.Log(this, FFLogType.Skill, string.Format("Skill {0} mSightType: {1}", this.Skillid, this.mSightType));
                    if (this.mSightType == MainPlayerSkillBase.SightType.Sector)
                    {
                        this.Sightangle = float.Parse(array[1]);
                        this.Sightradius = float.Parse(array[2]) / 3f;
                        this.Sighttex1name = array[3];
                        this.Sighttex2name = array[4];
                        this.Sighttex3name = array[5];
                    }
                    else if (this.mSightType == MainPlayerSkillBase.SightType.Circle)
                    {
                        this.Sightradius = float.Parse(array[1]) / 3f;
                        this.Sightsize = float.Parse(array[2]) / 3f;
                        this.Sighttex1name = array[3];
                        this.Sighttex2name = array[4];
                    }
                    else if (this.mSightType == MainPlayerSkillBase.SightType.Rectangle)
                    {
                        this.Sightradius = float.Parse(array[1]) / 3f;
                        this.Sightsize = float.Parse(array[2]) / 3f;
                        this.Sighttex1name = array[3];
                    }
                }
            }
        }
        catch (Exception arg)
        {
            FFDebug.LogError(this, string.Format("InitSightType Error: {0}  {1} ", arg, this.Skillid));
        }
    }

    private void InitStorageType()
    {
        if (this.SkillConfig == null)
        {
            this.IsStorageType = false;
            return;
        }
        if (this.SkillConfig.GetField_Bool("multiskill"))
        {
            this.mSorageType = SkillSorageType.CDAfterRelase;
        }
        else
        {
            this.mSorageType = SkillSorageType.CDWhenRelase;
        }
        if (this.SkillConfig.GetField_Uint("dtime") > 0U && this.SkillConfig.GetField_Uint("maxoverlaytimes") > 0U && !this.SkillConfig.GetField_Bool("multiskill"))
        {
            this.IsStorageType = true;
            this.IStorageTimes = this.ServerData.IStorageTimes;
        }
    }

    public static bool NoCD;

    public uint Skillid;

    public uint Level;

    private bool normalSkill;

    public bool skilllock;

    public SkillSorageType mSorageType;

    public bool IsStorageType;

    private uint _iStorageTimes;

    protected MainPlayerSkillHolder SkillHolder;

    protected SkillManager Skillmgr;

    public ulong Wholeskillid;

    private LuaTable skillconfig;

    protected string[] IconNames;

    protected bool breakAutoAttack;

    protected bool canBePassive;

    protected bool needTarget;

    private SkillPublicCDControl spcdc;

    private MainPlayerTargetSelectMgr m_targetSelectMgr;

    public List<LuaTable> Stage_configList = new List<LuaTable>();

    private float attackrange = -1f;

    protected Action NextSkillAction;

    private MainPlayerSkillBase.state _currState = MainPlayerSkillBase.state.Standby;

    private uint _cdlength;

    private Vector2 LastPos = Vector2.zero;

    protected Vector2 orginalSelfPos = Vector2.zero;

    public MainPlayerSkillBase.SightState mSightState;

    public MainPlayerSkillBase.SightType mSightType;

    public float Sightangle;

    public float Sightradius;

    public float Sightsize;

    public string Sighttex1name;

    public string Sighttex2name;

    public string Sighttex3name;

    public enum state
    {
        CD,
        Release,
        ReleaseOver,
        Standby
    }

    public enum SightState
    {
        None,
        Begin,
        End
    }

    public enum SightType
    {
        Click,
        RotateCamera,
        Sector,
        Circle,
        Rectangle
    }
}
