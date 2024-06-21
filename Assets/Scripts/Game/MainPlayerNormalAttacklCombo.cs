using System;
using LuaInterface;
using UnityEngine;

public class MainPlayerNormalAttacklCombo : MainPlayerSkillBase
{
    public MainPlayerNormalAttacklCombo(MainPlayerSkillHolder Holder, uint skillid, uint level, uint cdlength) : base(Holder, skillid, level, cdlength)
    {
    }

    public override void Break(CSkillBreakType type)
    {
        this.ComboCount = 0U;
        this.OnDisplay = false;
        this.OnConfirm = false;
        base.Break(type);
    }

    public override bool CheckCanEnter()
    {
        return base.CheckCanEnter() && (base.CurrState == MainPlayerSkillBase.state.Standby || base.CurrState == MainPlayerSkillBase.state.Release);
    }

    public bool IsMaxCombo()
    {
        return (ulong)this.ComboCount >= (ulong)((long)this.Stage_configList.Count);
    }

    public override void OnSendSkillEvent()
    {
        base.OnSendSkillEvent();
        if (this.ComboCount == 0U)
        {
            if (this.OnDisplay)
            {
                return;
            }
            this.OnConfirm = true;
            this.LastComboClickInput = SingletonForMono<GameTime>.Instance.GetNowMsecond();
            this.ComboCount = 1U;
            base.SendSkill();
        }
        else
        {
            if (this.FinishCurrStageDisplay)
            {
                return;
            }
            if ((ulong)this.ComboCount >= (ulong)((long)this.Stage_configList.Count))
            {
                this.SkillHolder.CacheNextSkill(this.Skillid);
                return;
            }
            this.IsComboNext = true;
        }
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

    private void ComboNext()
    {
        this.ComboCount += 1U;
        if (!this.OnConfirm)
        {
            int index = (int)(this.ComboCount - 1U);
            this.SelfDisplaySkillStage(this.Stage_configList[index]);
        }
    }

    private void SelfDisplaySkillStage(LuaTable SkillStage)
    {
        this.IsComboNext = false;
        this.FinishCurrStageDisplay = false;
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
        if (ffskillAnimClip == null)
        {
            return;
        }
        skillState.AddSkillClip(ffskillAnimClip);
        SkillClip skillClip2 = ffskillAnimClip;
        skillClip2.OnStateChange = (Action<SkillClip>)Delegate.Combine(skillClip2.OnStateChange, new Action<SkillClip>(delegate (SkillClip skillClip)
        {
            if (skillClip.mState == SkillClip.State.BeforePose)
            {
                this.IsComboNext = false;
                this.FinishCurrStageDisplay = false;
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
                this.FinishCurrStageDisplay = true;
                if (SkillStage.GetField_Bool("CanCancelCloseFist"))
                {
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
                    else if (MyPlayerPrefs.GetInt("AutoAttackClosed") == 0 && MainPlayerSkillHolder.Instance.skillAttackAutoAttack.AutoAttackState)
                    {
                        this.OnSendSkillEvent();
                        if (this.IsComboNext)
                        {
                            this.ComboNext();
                        }
                    }
                }
                else if (this.IsComboNext)
                {
                    this.ComboNext();
                }
            }
            else if (skillClip.mState == SkillClip.State.Over)
            {
                if (skillstageid == this.GetStagePartId(this.ComboCount))
                {
                    this.CurrState = MainPlayerSkillBase.state.ReleaseOver;
                    this.ComboCount = 0U;
                }
                if (this.NextSkillAction != null && this.SkillHolder.NextSkill != this.Skillid)
                {
                    this.ComboCount = 0U;
                    this.OnDisplay = false;
                    skillState.ClearSkillClipQueue();
                    this.CallNextSkill();
                }
                this.OnDisplay = false;
            }
        }));
    }

    public override void CancelCacheSkill()
    {
        base.CancelCacheSkill();
    }

    public override void CallNextSkill()
    {
        if (this.NextSkillAction != null)
        {
            base.CurrState = MainPlayerSkillBase.state.CD;
            this.NextSkillAction();
            this.NextSkillAction = null;
        }
    }

    public override bool CanBreakMe(uint NextSkill)
    {
        return true;
    }

    private uint ComboCount;

    private ulong ComboInterval = 1000UL;

    private ulong LastComboClickInput;

    public bool OnConfirm;

    private bool OnDisplay;

    private bool FinishCurrStageDisplay;

    private bool IsComboNext;
}
