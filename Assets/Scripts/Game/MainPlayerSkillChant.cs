using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class MainPlayerSkillChant : MainPlayerSkillBase
{
    public MainPlayerSkillChant(MainPlayerSkillHolder Holder, uint skillid, uint level, uint cdlength) : base(Holder, skillid, level, cdlength)
    {
    }

    public override bool CheckCanEnter()
    {
        return base.CheckCanEnter() && base.CurrState == MainPlayerSkillBase.state.Standby;
    }

    public override void OnSendSkillEvent()
    {
        if (base.CurrState != MainPlayerSkillBase.state.Standby)
        {
            return;
        }
        if (!this.OnSkill)
        {
            base.OnSendSkillEvent();
            this.chantCount = 1;
            base.SendSkill();
            this.OnSkill = true;
        }
    }

    private ulong chantStageId
    {
        get
        {
            if (this.Stage_configList.Count == 0)
            {
                FFDebug.LogError(this, "Stage_configList.Count == 0 Wholeskillid = " + this.Wholeskillid);
                return 0UL;
            }
            this.chantCount = ((this.chantCount <= this.Stage_configList.Count) ? this.chantCount : this.Stage_configList.Count);
            return this.Stage_configList[this.chantCount - 1].GetField_ULong("id");
        }
    }

    public override void OnServerConfirm()
    {
        base.CurrState = MainPlayerSkillBase.state.Release;
        base.ActivatePublicCD();
        bool flag = !this.Skillmgr.CheckChantSkillLimit(this.Skillid);
        int i = (!flag) ? 0 : 1;
        if (i != 0)
        {
            this.chantCount++;
        }
        while (i < this.Stage_configList.Count)
        {
            this.SelfDisplaySkillStage(this.Stage_configList[i], flag);
            i++;
        }
    }

    public void StartChant(ulong _skillstageid)
    {
        if (this.Skillmgr.IsChantStage(_skillstageid))
        {
            LuaTable luaTable = this.Skillmgr.Gett_skill_lv_config(_skillstageid);
            float durationtime = luaTable.GetField_Uint("chanttime") / 1000f;
            ProgressUIController controller = ControllerManager.Instance.GetController<ProgressUIController>();
            if (controller != null)
            {
                controller.StrInfo = luaTable.GetField_String("skillname");
                controller.ShowProgressBar(durationtime, ProgressUIController.ProgressBarType.Skill, null);
            }
        }
    }

    public override void Break(CSkillBreakType type)
    {
        if (this.Skillmgr.IsChantStage(this.chantStageId))
        {
            ControllerManager.Instance.GetController<ProgressUIController>().BreakProgressBar();
        }
        this.OnSkill = false;
        base.Break(type);
    }

    public override void CheckTargetRange()
    {
        if (this.needTarget && this.Skillmgr.IsChantStage(this.chantStageId))
        {
            CharactorBase target = base.GetTarget(false, false);
            if (target != null && target.CurrMoveData != null)
            {
                float[] skillRangeParam = base.GetSkillRangeParam(base.SkillConfig.GetField_String("SkillRange"));
                float num = Vector2.Distance(target.CurrentPosition2D, MainPlayer.Self.CurrentPosition2D);
                if (target is Npc)
                {
                    uint baseid = (target as Npc).NpcData.MapNpcData.baseid;
                    LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)baseid);
                    if (configTable != null)
                    {
                        num -= configTable.GetField_Uint("volume");
                    }
                }
                if (num > skillRangeParam[1])
                {
                    this.Break(CSkillBreakType.TargetRangeOut);
                }
            }
        }
    }

    private void SelfDisplaySkillStage(LuaTable SkillStage, bool attack)
    {
        FFBehaviourState_Skill ffbehaviourState_Skill = MainPlayer.Self.GetComponent<FFBehaviourControl>().CurrStatebyType<FFBehaviourState_Skill>();
        if (ffbehaviourState_Skill == null)
        {
            FFDebug.LogWarning(this, "Player Not InSkillState when skill :" + SkillStage);
            return;
        }
        SkillClipServerDate skillClipServerDate = new SkillClipServerDate();
        ulong skillstageid = SkillStage.GetField_ULong("id");
        LuaTable luaTable = this.Skillmgr.Gett_skill_lv_config(skillstageid);
        skillClipServerDate.ChantLength = luaTable.GetField_Uint("chanttime") / 1000f;
        bool flag = false;
        Vector2 Pos = base.GetTargetPos(SkillStage, "MoveDis", out flag);
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
        ffbehaviourState_Skill.AddSkillClip(ffskillAnimClip);
        SkillClip skillClip3 = ffskillAnimClip;
        skillClip3.OnStateChange = (Action<SkillClip>)Delegate.Combine(skillClip3.OnStateChange, new Action<SkillClip>(delegate (SkillClip skillClip)
        {
            if (skillClip.mState == SkillClip.State.None)
            {
                if (this.Skillmgr.GetStageCount(this.chantStageId) == 2UL)
                {
                    this.ActivateCD();
                }
            }
            else if (skillClip.mState == SkillClip.State.BeforePose)
            {
                if (skillClip.AnimData.AttackTimeF > 0U && !attack)
                {
                    this.SendSkillStage(skillstageid, 1U, Pos, skillClip.ServerData.targetID);
                }
                else
                {
                    this.SendSkillStage(skillstageid, 3U, Pos, skillClip.ServerData.targetID);
                }
            }
            else if (skillClip.mState == SkillClip.State.Attack)
            {
                if (skillClip.AnimData.AttackTimeF > 0U)
                {
                    this.SendSkillStage(skillstageid, 2U, Pos, skillClip.ServerData.targetID);
                }
            }
            else if (skillClip.mState == SkillClip.State.CloseFist)
            {
                this.chantCount++;
            }
        }));
        if (skillstageid == (ulong)base.LastStage.GetField_Uint("id"))
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

    private bool OnSkill;

    private int chantCount = 1;
}
