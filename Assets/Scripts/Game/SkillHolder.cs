using System;
using Framework.Managers;
using magic;
using UnityEngine;

public class SkillHolder : IFFComponent, ISkillHolder
{
    public CompnentState State { get; set; }

    private FFBehaviourControl FFBC
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

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owmner = Mgr.Owner;
    }

    public void CompUpdate()
    {
    }

    public void CompDispose()
    {
    }

    public void StartDisplaySkill(MSG_Ret_StartMagicAttack_SC Data)
    {
        if (this.FFBC == null)
        {
            return;
        }
        if (this.FFBC.CurrState is FFBehaviourState_Idle)
        {
            this.Owmner.StopMoveImmediate(delegate
            {
                this.FFBC.ChangeState(ClassPool.GetObject<FFBehaviourState_Skill>());
            });
        }
        else if (this.FFBC.CurrState is FFBehaviourState_Walk)
        {
            this.Owmner.StopMoveImmediate(delegate
            {
                this.FFBC.ChangeState(ClassPool.GetObject<FFBehaviourState_Skill>());
            });
        }
        else if (this.FFBC.CurrState is FFBehaviourState_Skill)
        {
            FFBehaviourState_Skill ffbehaviourState_Skill = this.FFBC.CurrStatebyType<FFBehaviourState_Skill>();
        }
    }

    public void DisplaySkillStage(MSG_Ret_SyncSkillStage_SC Data)
    {
        if (this.FFBC == null)
        {
            return;
        }
        if (Data.stagetype == 2U)
        {
            return;
        }
        this.Owmner.SetPlayerDirection(Data.userdir, false);
        if (this.Skillmgr.IsChantStage(Data.skillstage) || this.Skillmgr.IsSpellChannelStage(Data.skillstage))
        {
            this.StartChant(Data);
            return;
        }
        PlayerBufferControl component = this.Owmner.GetComponent<PlayerBufferControl>();
        if (component != null && component.HasBeControlled(BufferState.ControlType.NoSkill))
        {
            return;
        }
        if (component.HasBeControlled(BufferState.ControlType.NoLieve) && !this.Skillmgr.IsLieveSkill(Data.skillstage))
        {
            return;
        }
        SkillClipServerDate skillClipServerDate = new SkillClipServerDate();
        skillClipServerDate.MoveToTarget = this.Skillmgr.SkillNeedMove(Data.skillstage);
        skillClipServerDate.TargetPos = new Vector2(Data.desx, Data.desy);
        skillClipServerDate.TargetEffectPos = new Vector2(Data.desx, Data.desy);
        SkillClip ffskillAnimClip = this.Skillmgr.GetFFSkillAnimClip(this.Owmner, Data.skillstage, skillClipServerDate);
        if (ffskillAnimClip != null)
        {
            FFBehaviourState_Skill ffbehaviourState_Skill = this.FFBC.CurrStatebyType<FFBehaviourState_Skill>();
            if (ffbehaviourState_Skill == null)
            {
                ffbehaviourState_Skill = ClassPool.GetObject<FFBehaviourState_Skill>();
                this.FFBC.ChangeState(ffbehaviourState_Skill);
            }
            ffbehaviourState_Skill.AddSkillClip(ffskillAnimClip);
            if (ffbehaviourState_Skill.CurrSkillClip != null)
            {
                ulong skillID = this.GetSkillID(ffbehaviourState_Skill.CurrSkillClip.SkillStageId);
                ulong skillID2 = this.GetSkillID(Data.skillstage);
                if (skillID == skillID2)
                {
                    ffbehaviourState_Skill.SkipToNextClip();
                }
            }
        }
    }

    public ulong GetSkillID(ulong skillstageid)
    {
        return skillstageid / 10000UL;
    }

    private void StartChant(MSG_Ret_SyncSkillStage_SC Data)
    {
        if (this.FFBC == null)
        {
            return;
        }
        FFBehaviourState_Skill ffbehaviourState_Skill = this.FFBC.CurrStatebyType<FFBehaviourState_Skill>();
        if (ffbehaviourState_Skill != null)
        {
            SkillClipServerDate skillClipServerDate = new SkillClipServerDate();
            skillClipServerDate.ChantLength = this.Skillmgr.Getlv_config(Data.skillstage).GetField_Uint("chanttime") / 1000f;
            SkillClip ffskillAnimClip = this.Skillmgr.GetFFSkillAnimClip(this.Owmner, Data.skillstage, skillClipServerDate);
            ffbehaviourState_Skill.AddSkillClip(ffskillAnimClip);
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
    }

    public void ResetComp()
    {
    }

    private CharactorBase Owmner;
}
