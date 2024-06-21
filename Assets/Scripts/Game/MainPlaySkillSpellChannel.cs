using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class MainPlaySkillSpellChannel : MainPlayerSkillBase
{
    public MainPlaySkillSpellChannel(MainPlayerSkillHolder Holder, uint skillid, uint level, uint cdlength) : base(Holder, skillid, level, cdlength)
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
            base.SendSkill();
            this.OnSkill = true;
        }
    }

    public void StartDisplaySkill(ulong _skillstageid)
    {
        LuaTable luaTable = this.Skillmgr.Gett_skill_lv_config(_skillstageid);
        float num = luaTable.GetField_Uint("chanttime") / 1000f;
        if (num > 0f)
        {
            ProgressUIController controller = ControllerManager.Instance.GetController<ProgressUIController>();
            if (controller != null)
            {
                controller.StrInfo = luaTable.GetField_String("skillname");
                controller.ShowProgressBar(num, SliderDirection.RightToLeft, ProgressUIController.ProgressBarType.Skill, null);
            }
        }
    }

    public override void OnServerConfirm()
    {
        base.OnServerConfirm();
        for (int i = 0; i < this.Stage_configList.Count; i++)
        {
            this.SelfDisplaySkillStage(this.Stage_configList[i]);
        }
    }

    public override void Break(CSkillBreakType type)
    {
        ControllerManager.Instance.GetController<ProgressUIController>().BreakProgressBar();
        this.OnSkill = false;
        base.Break(type);
    }

    private void SelfDisplaySkillStage(LuaTable SkillStage)
    {
        ulong SkillStageid = SkillStage.GetField_ULong("id");
        FFBehaviourState_Skill ffbehaviourState_Skill = MainPlayer.Self.GetComponent<FFBehaviourControl>().CurrStatebyType<FFBehaviourState_Skill>();
        if (ffbehaviourState_Skill == null)
        {
            return;
        }
        SkillClipServerDate skillClipServerDate = new SkillClipServerDate();
        ulong field_ULong = SkillStage.GetField_ULong("id");
        LuaTable luaTable = this.Skillmgr.Gett_skill_lv_config(field_ULong);
        skillClipServerDate.ChantLength = luaTable.GetField_Uint("chanttime") / 1000f;
        bool flag = false;
        Vector2 Pos = base.GetTargetPos(SkillStage, "MoveDis", out flag);
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
        SkillClip skillClip2 = ffskillAnimClip;
        skillClip2.OnStateChange = (Action<SkillClip>)Delegate.Combine(skillClip2.OnStateChange, new Action<SkillClip>(delegate (SkillClip skillClip)
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
            else if (skillClip.mState == SkillClip.State.Attack)
            {
                if (skillClip.AnimData.AttackTimeF > 0U)
                {
                    this.SendSkillStage(SkillStageid, 2U, Pos, default(EntitiesID));
                }
            }
            else if (skillClip.mState != SkillClip.State.CloseFist)
            {
                if (skillClip.mState == SkillClip.State.Over)
                {
                    this.OnSkill = false;
                }
            }
        }));
    }

    private bool OnSkill;
}
