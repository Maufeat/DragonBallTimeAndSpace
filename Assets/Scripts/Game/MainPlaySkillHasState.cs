using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class MainPlaySkillHasState : MainPlayerSkillBase
{
    public MainPlaySkillHasState(MainPlayerSkillHolder Holder, uint skillid, uint level, uint cdlength) : base(Holder, skillid, level, cdlength)
    {
    }

    public override bool CheckCanEnter()
    {
        return base.ServerData.StateOn || (base.CheckCanEnter() && base.CurrState == MainPlayerSkillBase.state.Standby);
    }

    public override void OnSendSkillEvent()
    {
        if (base.ServerData.StateOn)
        {
            this.TurnOffSkill();
        }
        else
        {
            if (base.CurrState != MainPlayerSkillBase.state.Standby)
            {
                return;
            }
            if (!this.OnSkill)
            {
                base.OnSendSkillEvent();
                base.SendSkill();
                this.OnSkill = true;
            }
        }
    }

    public override void OnServerConfirm()
    {
        base.ServerData.StateOn = true;
        base.CurrState = MainPlayerSkillBase.state.Release;
        FFDebug.Log(this, FFLogType.Default, "MainPlayerSkillNormal OnServerConfirm: " + this.Stage_configList.Count);
        for (int i = 0; i < this.Stage_configList.Count; i++)
        {
            this.SelfDisplaySkillStage(this.Stage_configList[i]);
        }
        if (ControllerManager.Instance.GetController<MainUIController>().mainView != null)
        {
            ControllerManager.Instance.GetController<MainUIController>().mainView.InitSkillTexture(true);
        }
    }

    public override void UpdateSkillState()
    {
        base.ForceChangeState(MainPlayerSkillBase.state.CD);
    }

    private void TurnOffSkill()
    {
        base.CurrState = MainPlayerSkillBase.state.ReleaseOver;
        this.Skillmgr.TurnOffSkill(this.Skillid);
        base.ActivateCD();
        base.ActivatePublicCD();
        base.ServerData.StateOn = false;
        if (ControllerManager.Instance.GetController<MainUIController>().mainView != null)
        {
            ControllerManager.Instance.GetController<MainUIController>().mainView.InitSkillTexture(true);
        }
    }

    private void SelfDisplaySkillStage(LuaTable SkillStage)
    {
        ulong SkillStageid = SkillStage.GetField_ULong("id");
        FFBehaviourState_Skill ffbehaviourState_Skill = MainPlayer.Self.GetComponent<FFBehaviourControl>().CurrStatebyType<FFBehaviourState_Skill>();
        if (ffbehaviourState_Skill == null)
        {
            FFDebug.Log(this, FFLogType.Default, "Player Not InSkillState when skill :" + SkillStageid);
            return;
        }
        bool flag = false;
        Vector2 Pos = base.GetTargetPos(SkillStage, "MoveDis", out flag);
        SkillClipServerDate skillClipServerDate = new SkillClipServerDate();
        skillClipServerDate.MoveToTarget = this.Skillmgr.SkillNeedMove(SkillStageid);
        skillClipServerDate.TargetPos = Pos;
        skillClipServerDate.TargetEffectPos = base.GetTargetPos(SkillStage, "Moveeffect", out flag);
        CharactorBase targetObj = base.GetTargetObj(SkillStage);
        if (targetObj != null)
        {
            skillClipServerDate.targetID = targetObj.EID;
        }
        SkillClip ffskillAnimClip = this.Skillmgr.GetFFSkillAnimClip(MainPlayer.Self, SkillStageid, skillClipServerDate);
        if (ffskillAnimClip == null)
        {
            return;
        }
        ffbehaviourState_Skill.AddSkillClip(ffskillAnimClip);
        SkillClip skillClip3 = ffskillAnimClip;
        skillClip3.OnStateChange = (Action<SkillClip>)Delegate.Combine(skillClip3.OnStateChange, new Action<SkillClip>(delegate (SkillClip skillClip)
        {
            if (skillClip.mState == SkillClip.State.BeforePose)
            {
                if (skillClip.AnimData.AttackTimeF > 0U)
                {
                    this.SendSkillStage(SkillStageid, 1U, Pos, default(EntitiesID));
                }
                else
                {
                    this.SendSkillStage(SkillStageid, 3U, Pos, default(EntitiesID));
                }
            }
            else if (skillClip.mState == SkillClip.State.Attack && skillClip.AnimData.AttackTimeF > 0U)
            {
                this.SendSkillStage(SkillStageid, 2U, Pos, default(EntitiesID));
            }
        }));
        if (SkillStageid == (ulong)base.LastStage.GetField_Uint("id"))
        {
            SkillClip skillClip2 = ffskillAnimClip;
            skillClip2.OnStateChange = (Action<SkillClip>)Delegate.Combine(skillClip2.OnStateChange, new Action<SkillClip>(delegate (SkillClip skillClip)
            {
                if (skillClip.mState == SkillClip.State.Over)
                {
                    this.CurrState = MainPlayerSkillBase.state.ReleaseOver;
                    this.OnSkill = false;
                }
            }));
        }
    }

    public override void Break(CSkillBreakType type)
    {
        this.OnSkill = false;
        base.Break(type);
    }

    private bool OnSkill;
}
