using System;
using LuaInterface;
using UnityEngine;

public class MainPlayerSkillNormal : MainPlayerSkillBase
{
    public MainPlayerSkillNormal(MainPlayerSkillHolder Holder, uint skillid, uint level, uint cdlength) : base(Holder, skillid, level, cdlength)
    {
    }

    public override bool CheckCanEnter()
    {
        if (!base.CheckCanEnter())
        {
            return false;
        }
        if (base.CurrState == MainPlayerSkillBase.state.CD)
        {
            return this.IsStorageType && base.IStorageTimes > 0U;
        }
        return base.CurrState == MainPlayerSkillBase.state.Standby || base.CurrState == MainPlayerSkillBase.state.Release;
    }

    public override void OnSendSkillEvent()
    {
        if (base.CurrState != MainPlayerSkillBase.state.Standby && base.IStorageTimes == 0U)
        {
            if (base.CurrState == MainPlayerSkillBase.state.Release)
            {
                this.SkillHolder.CacheNextSkill(this.Skillid);
            }
            return;
        }
        if (!this.OnSkill)
        {
            base.OnSendSkillEvent();
            base.SendSkill();
            this.OnSkill = true;
            if (this.IsStorageType && base.IStorageTimes >= 1U)
            {
                if (base.IStorageTimes == base.IMaxStorageTimes && this.mSorageType == SkillSorageType.CDWhenRelase)
                {
                    base.LastUpdateTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
                }
                base.IStorageTimes -= 1U;
                base.CheckStorageSkillIsCD();
            }
        }
    }

    public override void OnServerConfirm()
    {
        base.OnServerConfirm();
        if (this.Stage_configList.Count <= 0)
        {
            FFDebug.LogError(this, string.Format("Skill [{0}] OnServerConfirm stages not exist", this.Skillid));
            return;
        }
        this.currentStageIndex = 0;
        this.SelfDisplaySkillStage(this.Stage_configList[this.currentStageIndex]);
    }

    private void DisplayNextStage()
    {
        this.currentStageIndex++;
        if (this.Stage_configList.Count <= this.currentStageIndex)
        {
            FFDebug.LogError(this, string.Format("Skill [{0}] OnServerConfirm stages not exist", this.Stage_configList.Count));
            return;
        }
        this.SelfDisplaySkillStage(this.Stage_configList[this.currentStageIndex]);
    }

    private bool SelfDisplaySkillStage(LuaTable SkillStage)
    {
        bool goonskill = true;
        ulong SkillStageid = SkillStage.GetField_ULong("id");
        FFBehaviourState_Skill ffbehaviourState_Skill = MainPlayer.Self.GetComponent<FFBehaviourControl>().CurrStatebyType<FFBehaviourState_Skill>();
        if (ffbehaviourState_Skill == null)
        {
            FFDebug.LogWarning(this, "Player Not InSkillState when skill :" + SkillStageid);
            return false;
        }
        bool flag = false;
        Vector2 Pos = base.GetTargetPos(SkillStage, "MoveDis", out flag);
        if (SkillStage.GetField_Uint("Checkfail") == 1U && flag)
        {
            goonskill = false;
        }
        SkillClipServerDate skillClipServerDate = new SkillClipServerDate();
        skillClipServerDate.MoveToTarget = this.Skillmgr.SkillNeedMove(SkillStageid);
        if (MainPlayer.Self.OnFastMove)
        {
            skillClipServerDate.TargetPos = Vector2.zero;
        }
        else
        {
            skillClipServerDate.TargetPos = Pos;
        }
        skillClipServerDate.TargetEffectPos = base.GetTargetPos(SkillStage, "Moveeffect", out flag);
        CharactorBase targetObj = base.GetTargetObj(SkillStage);
        if (targetObj != null)
        {
            skillClipServerDate.targetID = targetObj.EID;
        }
        SkillClip ffskillAnimClip = this.Skillmgr.GetFFSkillAnimClip(MainPlayer.Self, SkillStageid, skillClipServerDate);
        if (ffskillAnimClip == null)
        {
            return false;
        }
        ffbehaviourState_Skill.AddSkillClip(ffskillAnimClip);
        SkillClip skillClip3 = ffskillAnimClip;
        skillClip3.OnStateChange = (Action<SkillClip>)Delegate.Combine(skillClip3.OnStateChange, new Action<SkillClip>(delegate (SkillClip skillClip)
        {
            if (skillClip.mState == SkillClip.State.BeforePose)
            {
                this.SendSkillStage(SkillStageid, 1U, Pos, default(EntitiesID));
                if (skillClip.AnimData.AttackTimeF == 0U && skillClip.AnimData.EndMoveF > 0U)
                {
                    Scheduler.Instance.AddTimer(skillClip.AnimData.EndMoveF / 30f, false, delegate
                    {
                        this.SendSkillStage(SkillStageid, 2U, Pos, default(EntitiesID));
                    });
                }
            }
            else if (skillClip.mState == SkillClip.State.Attack)
            {
                if (skillClip.AnimData.AttackTimeF > 0U)
                {
                    this.SendSkillStage(SkillStageid, 2U, Pos, default(EntitiesID));
                }
            }
            else if (skillClip.mState == SkillClip.State.CloseFist)
            {
                if (skillClip.AnimData.AttackTimeF == 0U && skillClip.AnimData.EndMoveF == 0U)
                {
                    this.SendSkillStage(SkillStageid, 2U, Pos, default(EntitiesID));
                }
                if (this.currentStageIndex < this.Stage_configList.Count - 1 && goonskill)
                {
                    this.DisplayNextStage();
                }
            }
        }));
        if (SkillStageid == base.LastStage.GetField_ULong("id"))
        {
            SkillClip skillClip2 = ffskillAnimClip;
            skillClip2.OnStateChange = (Action<SkillClip>)Delegate.Combine(skillClip2.OnStateChange, new Action<SkillClip>(delegate (SkillClip skillClip)
            {
                if (skillClip.mState == SkillClip.State.Over)
                {
                    if (this.IsStorageType)
                    {
                        if (this.mSorageType == SkillSorageType.CDWhenRelase)
                        {
                            if (this.IStorageTimes > 0U)
                            {
                                this.CurrState = MainPlayerSkillBase.state.Standby;
                            }
                            else
                            {
                                this.CurrState = MainPlayerSkillBase.state.ReleaseOver;
                            }
                        }
                        else if (this.IStorageTimes == this.IMaxStorageTimes)
                        {
                            this.CurrState = MainPlayerSkillBase.state.ReleaseOver;
                        }
                        else if (!this.OnSkill)
                        {
                            this.CurrState = MainPlayerSkillBase.state.Standby;
                        }
                    }
                    else
                    {
                        this.CurrState = MainPlayerSkillBase.state.ReleaseOver;
                    }
                    this.OnSkill = false;
                }
                else if (skillClip.mState == SkillClip.State.CloseFist)
                {
                    this.OnSkill = false;
                    if (this.NextSkillAction != null)
                    {
                        if (this.IsStorageType && this.mSorageType == SkillSorageType.CDAfterRelase && this.IStorageTimes == this.IMaxStorageTimes)
                        {
                            this.CurrState = MainPlayerSkillBase.state.ReleaseOver;
                        }
                        this.CallNextSkill();
                    }
                }
            }));
        }
        return goonskill;
    }

    public override void UpdateStoreTimes(uint times)
    {
        if (this.IsStorageType && times <= base.IMaxStorageTimes)
        {
            base.IStorageTimes = times;
            if (this.mSorageType == SkillSorageType.CDWhenRelase)
            {
                base.LastUpdateTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
                if (times > 0U)
                {
                    base.CurrState = MainPlayerSkillBase.state.Standby;
                    base.LastSkillTime = base.LastUpdateTime - base.CDLength;
                }
                else
                {
                    base.CurrState = MainPlayerSkillBase.state.ReleaseOver;
                }
            }
        }
    }

    public override void Break(CSkillBreakType type)
    {
        this.OnSkill = false;
        if (this.IsStorageType)
        {
            if (this.mSorageType == SkillSorageType.CDWhenRelase)
            {
                if (base.IStorageTimes > 0U)
                {
                    base.CurrState = MainPlayerSkillBase.state.Standby;
                }
                else
                {
                    base.CurrState = MainPlayerSkillBase.state.CD;
                }
            }
            else if (base.IStorageTimes == base.IMaxStorageTimes)
            {
                base.CurrState = MainPlayerSkillBase.state.CD;
            }
            else
            {
                base.CurrState = MainPlayerSkillBase.state.Standby;
            }
            this.NextSkillAction = null;
        }
        base.Break(type);
    }

    public override void CallNextSkill()
    {
        if (this.NextSkillAction != null)
        {
            if (base.LastStage.GetField_Bool("CanCancelCloseFist"))
            {
                if (this.IsStorageType)
                {
                    if (this.mSorageType == SkillSorageType.CDWhenRelase)
                    {
                        if (base.IStorageTimes > 0U)
                        {
                            base.CurrState = MainPlayerSkillBase.state.Standby;
                        }
                        else
                        {
                            base.CurrState = MainPlayerSkillBase.state.CD;
                        }
                    }
                    else if (base.IStorageTimes == base.IMaxStorageTimes)
                    {
                        base.CurrState = MainPlayerSkillBase.state.CD;
                    }
                    else
                    {
                        base.CurrState = MainPlayerSkillBase.state.Standby;
                    }
                }
                this.NextSkillAction();
            }
            this.NextSkillAction = null;
        }
    }

    private bool OnSkill;

    private int currentStageIndex = 1;
}
