using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class MainPlayerSkillCombo : MainPlayerSkillBase
{
    public MainPlayerSkillCombo(MainPlayerSkillHolder Holder, uint skillid, uint level, uint cdlength) : base(Holder, skillid, level, cdlength)
    {
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    private uint ComboCount
    {
        get
        {
            return this.combocount;
        }
        set
        {
            this.combocount = value;
        }
    }

    private FFBehaviourControl FFBC
    {
        get
        {
            return MainPlayer.Self.GetComponent<FFBehaviourControl>();
        }
    }

    private bool moveBreakComb
    {
        get
        {
            return base.SkillConfig.GetField_Bool("MoveBreakComb");
        }
    }

    private ulong ComboInterval
    {
        get
        {
            if (this.combinterval == 0UL)
            {
                LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("comboLimit").GetCacheField_Table(this.Skillid.ToString());
                if (cacheField_Table == null)
                {
                    this.combinterval = (ulong)this.defaultComboInterval;
                }
                else
                {
                    this.combinterval = (ulong)cacheField_Table.GetCacheField_Uint("value");
                }
            }
            return this.combinterval;
        }
    }

    private bool CanCancelPreFist
    {
        get
        {
            return this.currSkillClip != null && this.currSkillClip.AnimConfig.CanCancelPreFist && this.currSkillClip.mState < SkillClip.State.Attack;
        }
    }

    private bool CanCancelCloseFist
    {
        get
        {
            return this.currSkillClip.AnimConfig.MoveCancelCloseFist && this.currSkillClip.mState > SkillClip.State.Attack;
        }
    }

    public override void Break(CSkillBreakType type)
    {
        this.OnDisplay = false;
        this.OnConfirm = false;
        if (type == CSkillBreakType.ServerInterrupt || type == CSkillBreakType.InBuff)
        {
            this.CombBreak(MainPlayerSkillCombo.ComboBreakType.OtherBreak);
        }
        if (type == CSkillBreakType.Move)
        {
            if (this.moveBreakComb)
            {
                this.CombBreak(MainPlayerSkillCombo.ComboBreakType.OtherBreak);
            }
            else if (this.CanCancelPreFist)
            {
                this.CombBreak(MainPlayerSkillCombo.ComboBreakType.OtherBreak);
            }
        }
        base.Break(type);
    }

    public override bool CheckCanEnter()
    {
        return base.CheckCanEnter() && (base.CurrState == MainPlayerSkillBase.state.Standby || base.CurrState == MainPlayerSkillBase.state.Release);
    }

    private uint MaxCombCount
    {
        get
        {
            return (uint)this.Stage_configList.Count;
        }
    }

    public override void OnSkillEnter(MainPlayerSkillBase skillbase)
    {
        base.OnSkillEnter(skillbase);
        if (this.startcombwait)
        {
            bool field_Bool = base.SkillConfig.GetField_Bool("SkillBreakComb");
            if (field_Bool)
            {
                this.CombBreak(MainPlayerSkillCombo.ComboBreakType.OtherBreak);
            }
        }
    }

    public override void OnSendSkillEvent()
    {
        base.OnSendSkillEvent();
        if (this.ComboCount == 0U)
        {
            this.orginalSelfPos = MainPlayer.Self.CurrentPosition2D;
            if (MainPlayer.Self.MainPlayeData.AttributeData.mp < base.SkillConfig.GetField_Uint("magiccost"))
            {
                base.ShowNoEnoughMpTip();
                return;
            }
            if (this.OnDisplay)
            {
                return;
            }
            this.isCombing = true;
            this.OnConfirm = true;
            this.ComboCount = 1U;
            base.SendSkill();
            if (this.ComboCount < this.MaxCombCount)
            {
                this.StartCombInputAccess();
            }
            if (this.ComboCount == this.MaxCombCount)
            {
                this.CombBreak(MainPlayerSkillCombo.ComboBreakType.InMaxCombo);
            }
            base.ActivatePublicCD();
        }
        else
        {
            if (!this.startcombwait)
            {
                return;
            }
            if (this.ComboCount >= this.MaxCombCount)
            {
                this.orginalSelfPos = Vector2.zero;
                return;
            }
            if (!this.CheckNextStageMpEnough())
            {
                base.ShowNoEnoughMpTip();
                return;
            }
            this.CombSuccess();
            this.IsComboNext = true;
            if (!(this.FFBC.CurrState is FFBehaviourState_Skill))
            {
                MainPlayer.Self.StopMoveImmediate(delegate
                {
                    this.FFBC.ChangeState(ClassPool.GetObject<FFBehaviourState_Skill>());
                    this.ComboNext();
                });
            }
        }
        if (ControllerManager.Instance.GetController<MainUIController>().mainView != null)
        {
            ControllerManager.Instance.GetController<MainUIController>().mainView.InitSkillTexture(true);
        }
        base.InitSightType((int)this.ComboCount);
    }

    private bool CheckNextStageMpEnough()
    {
        if ((long)this.Stage_configList.Count <= (long)((ulong)this.ComboCount))
        {
            return false;
        }
        int num = (int)(this.ComboCount - 1U);
        if (num < this.Stage_configList.Count)
        {
            LuaTable luaTable = this.Stage_configList[num];
            return luaTable != null && MainPlayer.Self.MainPlayeData.AttributeData.mp >= luaTable.GetField_Uint("magiccost");
        }
        return false;
    }

    public override void OnServerConfirm()
    {
        base.OnServerConfirm();
        int num = 1;
        while ((long)num <= (long)((ulong)this.ComboCount))
        {
            this.SelfDisplaySkillStage(this.Stage_configList[num - 1]);
            num++;
        }
        this.OnConfirm = false;
    }

    private bool OnDisplay
    {
        get
        {
            return this.ondisplay;
        }
        set
        {
            this.ondisplay = value;
        }
    }

    private void ComboNext()
    {
        this.ComboCount += 1U;
        if (!this.OnConfirm)
        {
            int index = (int)(this.ComboCount - 1U);
            this.SelfDisplaySkillStage(this.Stage_configList[index]);
        }
        base.ActivatePublicCD();
        if (this.ComboCount < this.MaxCombCount)
        {
            this.StartCombInputAccess();
        }
        if (this.ComboCount == this.MaxCombCount)
        {
            this.CombBreak(MainPlayerSkillCombo.ComboBreakType.OtherBreak);
        }
    }

    private void SelfDisplaySkillStage(LuaTable SkillStage)
    {
        this.IsComboNext = false;
        ulong skillstageid = SkillStage.GetField_ULong("id");
        FFBehaviourState_Skill skillState = MainPlayer.Self.GetComponent<FFBehaviourControl>().CurrStatebyType<FFBehaviourState_Skill>();
        if (skillState == null)
        {
            FFDebug.LogWarning(this, "Player Not InSkillState when skill :" + skillstageid);
            return;
        }
        bool flag = false;
        Vector2 Pos = base.GetTargetPos(SkillStage, "MoveDis", out flag);
        SkillClipServerDate skillClipServerDate = new SkillClipServerDate();
        skillClipServerDate.MoveToTarget = this.Skillmgr.SkillNeedMove(skillstageid);
        skillClipServerDate.TargetPos = Pos;
        skillClipServerDate.TargetEffectPos = base.GetTargetPos(SkillStage, "Moveeffect", out flag);
        CharactorBase targetObj = base.GetTargetObj(SkillStage);
        if (targetObj != null)
        {
            skillClipServerDate.targetID = targetObj.EID;
        }
        SkillClip ffskillAnimClip = this.Skillmgr.GetFFSkillAnimClip(MainPlayer.Self, skillstageid, skillClipServerDate);
        this.currSkillClip = ffskillAnimClip;
        if (ffskillAnimClip == null)
        {
            FFDebug.LogWarning(this, "cant find SkillClip !!! skillstageid = " + skillstageid);
            return;
        }
        skillState.AddSkillClip(ffskillAnimClip);
        SkillClip skillClip2 = ffskillAnimClip;
        skillClip2.OnStateChange = (Action<SkillClip>)Delegate.Combine(skillClip2.OnStateChange, new Action<SkillClip>(delegate (SkillClip skillClip)
        {
            if (skillClip.mState == SkillClip.State.BeforePose)
            {
                this.IsComboNext = false;
                if (skillClip.AnimData.AttackTimeF > 0U)
                {
                    this.SendSkillStage(skillstageid, 1U, Pos, default(EntitiesID));
                }
                else
                {
                    this.SendSkillStage(skillstageid, 3U, Pos, default(EntitiesID));
                }
                this.OnDisplay = true;
            }
            else if (skillClip.mState == SkillClip.State.Attack)
            {
                if (skillClip.AnimData.AttackTimeF > 0U)
                {
                    this.SendSkillStage(skillstageid, 2U, Pos, default(EntitiesID));
                }
            }
            else if (skillClip.mState == SkillClip.State.CloseFist)
            {
                if (SkillStage.GetField_Bool("CanCancelCloseFist") && this.NextSkillAction != null)
                {
                    if (this.SkillHolder.NextSkill == this.Skillid && skillstageid != this.GetStagePartId(this.ComboCount))
                    {
                        return;
                    }
                    this.ComboCount = 0U;
                    this.OnDisplay = false;
                    skillState.ClearSkillClipQueue();
                    this.CallNextSkill();
                }
                else if (this.IsComboNext)
                {
                    this.ComboNext();
                }
            }
            else if (skillClip.mState == SkillClip.State.Over)
            {
                if (skillstageid % 10UL == (ulong)this.MaxCombCount)
                {
                    this.CurrState = MainPlayerSkillBase.state.ReleaseOver;
                }
                if (this.NextSkillAction != null)
                {
                    if (this.SkillHolder.NextSkill == this.Skillid && skillstageid != this.GetStagePartId(this.ComboCount))
                    {
                        return;
                    }
                    this.ComboCount = 0U;
                    this.OnDisplay = false;
                    skillState.ClearSkillClipQueue();
                    this.CallNextSkill();
                }
                else if (this.IsComboNext)
                {
                    this.ComboNext();
                }
                this.OnDisplay = false;
            }
        }));
    }

    public override void CancelCacheSkill()
    {
        this.IsComboNext = false;
        base.CancelCacheSkill();
    }

    public override void CallNextSkill()
    {
        if (this.NextSkillAction != null)
        {
            this.CombBreak(MainPlayerSkillCombo.ComboBreakType.OtherBreak);
            this.ComboCount = 0U;
            this.OnDisplay = false;
            this.OnConfirm = false;
            base.CurrState = MainPlayerSkillBase.state.CD;
            this.NextSkillAction();
            this.NextSkillAction = null;
        }
    }

    public override bool CanBreakMe(uint NextSkill)
    {
        return true;
    }

    private void StartCombInputAccess()
    {
        this.startcombwait = true;
        this.LastComboClickInput = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        if (MainPlayerSkillCombo.OnCombStart != null)
        {
            int num = (int)(this.ComboCount - 1U);
            if ((long)num < (long)((ulong)this.MaxCombCount))
            {
                MainPlayerSkillCombo.OnCombStart(this.Stage_configList[num].GetField_Uint("skillid"));
            }
        }
    }

    private bool CheckInCD()
    {
        ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        float num = base.GetCDLeft(currServerTime);
        return num > 0f;
    }

    private void CombBreak(MainPlayerSkillCombo.ComboBreakType type)
    {
        this.isCombing = false;
        if (!this.CheckInCD())
        {
            base.ActivateCD();
        }
        this.orginalSelfPos = Vector2.zero;
        this.startcombwait = false;
        this.ComboCount = 0U;
        this.OnConfirm = false;
        if (type == MainPlayerSkillCombo.ComboBreakType.InMaxCombo)
        {
            this.OnDisplay = false;
            base.CurrState = MainPlayerSkillBase.state.ReleaseOver;
        }
        else if (type == MainPlayerSkillCombo.ComboBreakType.TimeOverBreak)
        {
            this.OnDisplay = false;
            base.CurrState = MainPlayerSkillBase.state.CD;
        }
        if (MainPlayerSkillCombo.OnCombBreak != null)
        {
            MainPlayerSkillCombo.OnCombBreak(type);
        }
        if (ControllerManager.Instance.GetController<MainUIController>().mainView != null)
        {
            ControllerManager.Instance.GetController<MainUIController>().mainView.InitSkillTexture(true);
        }
        base.InitSightType(0);
    }

    private void CombSuccess()
    {
        if (MainPlayerSkillCombo.OnCombBreak != null)
        {
            MainPlayerSkillCombo.OnCombBreak(MainPlayerSkillCombo.ComboBreakType.CombSuccess);
        }
    }

    public static void FreezeTime(bool bguide)
    {
        MainPlayerSkillCombo.bGuide = bguide;
    }

    private void Update()
    {
        if (!this.startcombwait)
        {
            return;
        }
        if (MainPlayerSkillCombo.bGuide)
        {
            this.LastComboClickInput = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        }
        ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        if (currServerTime - this.LastComboClickInput > this.ComboInterval)
        {
            this.CombBreak(MainPlayerSkillCombo.ComboBreakType.TimeOverBreak);
        }
        if (MainPlayerSkillCombo.OnCombUpdate != null)
        {
            MainPlayerSkillCombo.OnCombUpdate((this.ComboInterval - (currServerTime - this.LastComboClickInput)) / 1000f, this.ComboInterval / 1000f);
        }
    }

    public override string GetCurSkillIconName()
    {
        if (this.ComboCount == 0U)
        {
            return this.IconNames[0];
        }
        if (this.IconNames.Length > 1)
        {
            return this.IconNames[1];
        }
        return this.IconNames[0];
    }

    private bool startcombwait;

    private uint combocount;

    public static Action<MainPlayerSkillCombo.ComboBreakType> OnCombBreak;

    public static Action<uint> OnCombStart;

    public static Action<float, float> OnCombUpdate;

    private SkillClip currSkillClip;

    private uint defaultComboInterval = 3000U;

    private ulong combinterval;

    private ulong LastComboClickInput;

    private bool isCombing;

    public bool OnConfirm;

    private bool ondisplay;

    private bool IsComboNext;

    private static bool bGuide;

    public enum ComboBreakType
    {
        CombSuccess,
        TimeOverBreak,
        OtherBreak,
        InMaxCombo
    }
}
