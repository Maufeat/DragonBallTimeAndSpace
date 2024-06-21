using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class FFBehaviourState_Skill : FFBehaviourBaseState, IstorebAble
{
    public SkillClip CurrSkillClip
    {
        get
        {
            return this._currSkillClip;
        }
        private set
        {
            if (this._currSkillClip != value)
            {
                this.OnCurrSkillClipChange(this._currSkillClip, value);
                this._currSkillClip = value;
            }
        }
    }

    public float SpeedCut
    {
        get
        {
            if (this._speedcut < 0f)
            {
                return 0f;
            }
            if (this._speedcut > 1f)
            {
                return 1f;
            }
            return this._speedcut;
        }
    }

    public override void OnEnter(StateMachine parent)
    {
        base.OnEnter(parent);
        this.Parent.Owmner.StopMoveImmediate(null);
        FFWeaponHold component = this.Parent.Owmner.GetComponent<FFWeaponHold>();
        if (component != null)
        {
            component.ChangeState(true);
        }
    }

    public override void OnUpdate(StateMachine parent)
    {
        if (this.CurrSkillClip == null)
        {
            this.SkipToNextClip();
            return;
        }
        if (this.CurrSkillClip.TotalSkillTime > 0f && base.RunningTime > this.CurrSkillClip.TotalSkillTime)
        {
            this.CurrSkillClip.ChangeState(SkillClip.State.CloseFist);
        }
        base.OnUpdate(parent);
        switch (this.CurrSkillClip.mState)
        {
            case SkillClip.State.None:
                this.OnStartSkillClip();
                break;
            case SkillClip.State.BeforePose:
                this.OnBeforePose();
                break;
            case SkillClip.State.OnPose:
                this.OnPose();
                break;
            case SkillClip.State.AfterPose:
                this.OnAfterPose();
                break;
            case SkillClip.State.Attack:
                this.OnAttack();
                break;
            case SkillClip.State.CloseFist:
                this.OnCloseFist();
                break;
        }
        this.UpdateMove();
        if (this.Parent.Owmner.ModelObj != null && this.CurrSkillClip != null)
        {
            this.UpdataOnceAudio(this.Parent.Owmner.ModelObj.transform, this.CurrSkillClip.AnimData.ClipName, base.RunningTime);
        }
    }

    public bool CanMoveCancelSkil
    {
        get
        {
            return this.CurrSkillClip == null || this.CurrSkillClip.AnimConfig == null || (this.CurrSkillClip.AnimConfig.MoveCancelCloseFist && base.RunningTime >= this.CurrSkillClip.CanMoveCancelCloseFistTime) || (this.CurrSkillClip.AnimConfig.CanCancelPreFist && base.RunningTime < this.CurrSkillClip.CanMoveCancelSkillTime);
        }
    }

    public SkillClip.State CurrSkillClipState
    {
        get
        {
            if (this.CurrSkillClip == null)
            {
                return SkillClip.State.Over;
            }
            return this.CurrSkillClip.mState;
        }
    }

    public bool IsDirty { get; set; }

    public void ClearSkillClipQueue()
    {
        this.SkillClipQueue.Clear();
    }

    public void RefreshSkillClipQueue(CharactorBase owner, SkillManager skillmgr)
    {
        if (owner == null || skillmgr == null || this.SkillClipQueue == null)
        {
            return;
        }
        List<SkillClip> list = new List<SkillClip>();
        while (this.SkillClipQueue.Count > 0)
        {
            SkillClip skillClip = this.SkillClipQueue.Dequeue();
            list.Add(skillmgr.GetFFSkillAnimClip(owner, skillClip.SkillStageId, skillClip.ServerData));
        }
        for (int i = 0; i < list.Count; i++)
        {
            this.SkillClipQueue.Enqueue(list[i]);
        }
        list.Clear();
    }

    public void AddSkillClip(SkillClip clip)
    {
        if (clip == null)
        {
            FFDebug.LogWarning(this, "AddSkillClip Clip null");
            return;
        }
        this.SkillClipQueue.Enqueue(clip);
        if (this.CurrSkillClip == null)
        {
            this.SkipToNextClip();
        }
    }

    private void UpdateMove()
    {
        if (this.CurrSkillClip == null)
        {
            return;
        }
        if (this.CurrSkillClip.ServerData == null)
        {
            return;
        }
        if (this.CurrHasMove)
        {
            return;
        }
        if (this.CurrSkillClip.ServerData.TargetPos != Vector2.zero && this.CurrSkillClip.ServerData.MoveToTarget && this.CurrSkillClip.LvConfig.UseType != 7U && base.RunningTime >= this.CurrSkillClip.StartMoveTime)
        {
            this.CurrHasMove = true;
            if (this.CurrSkillClip.MoveLength > 0f)
            {
                this.Parent.Owmner.FastMoveTo(this.CurrSkillClip.ServerData.TargetPos, this.CurrSkillClip.MoveLength, this.CurrSkillClip.MirrorTotalTime, this.CurrSkillClip.MirrorFadeSpeed);
            }
        }
    }

    private void OnStartSkillClip()
    {
        this.CurrHasMove = false;
        this._speedcut = this.CurrSkillClip.AnimConfig.SpeedCut;
        this.Parent.PlayAnim(this.CurrSkillClip.AnimData.ClipName, this.CurrSkillClip.TransitionDuration, 0f);
        this.CurrSkillClip.ChangeState(SkillClip.State.BeforePose);
        base.RunningTime = 0f;
        this.AddCurrClipEffect();
    }

    private void OnBeforePose()
    {
        if (this.CurrSkillClip.PoseTime > this.CurrSkillClip.AttackTime && base.RunningTime >= this.CurrSkillClip.AttackTime)
        {
            this.CurrSkillClip.ChangeState(SkillClip.State.Attack);
            this.OnUpdate(this.Parent);
        }
        else if (base.RunningTime >= this.CurrSkillClip.PoseTime)
        {
            this.CurrSkillClip.ChangeState(SkillClip.State.OnPose);
            if (this.CurrSkillClip.AnimData.mPoseType == FFActionClip.PoseType.Pauce)
            {
                if (this.Parent.m_animator != null)
                {
                    this.Parent.m_animator.speed = 0f;
                }
                this.PlayLoopAudio(this.Parent.Owmner.ModelObj.transform, this.CurrSkillClip.AnimData.ClipName, true);
            }
            else if (this.CurrSkillClip.AnimData.mPoseType == FFActionClip.PoseType.Loop)
            {
                this.LoopTime = 0f;
                this.PlayLoopAudio(this.Parent.Owmner.ModelObj.transform, this.CurrSkillClip.AnimData.ClipName, true);
            }
        }
    }

    private void OnPose()
    {
        if (this.CurrSkillClip.PoseLength == 0f)
        {
            this.CurrSkillClip.ChangeState(SkillClip.State.AfterPose);
        }
        this.LoopTime += Time.deltaTime;
        if (this.CurrSkillClip.AnimData.mPoseType == FFActionClip.PoseType.Loop && this.LoopTime >= this.CurrSkillClip.PoseloopLength)
        {
            this.Parent.PlayAnim(this.CurrSkillClip.AnimData.ClipName, 0f, this.CurrSkillClip.PoseTime);
            this.LoopTime = 0f;
        }
        if (base.RunningTime >= this.CurrSkillClip.PoseLength)
        {
            if (this.CurrSkillClip.AnimData.mPoseType == FFActionClip.PoseType.Loop)
            {
                base.RunningTime = this.CurrSkillClip.PoseTime + this.CurrSkillClip.PoseLength + this.CurrSkillClip.PoseloopLength - this.LoopTime;
            }
            this.PlayLoopAudio(this.Parent.Owmner.ModelObj.transform, this.CurrSkillClip.AnimData.ClipName, false);
            this.CurrSkillClip.ChangeState(SkillClip.State.AfterPose);
            this.OnUpdate(this.Parent);
            if (this.Parent.m_animator != null)
            {
                this.Parent.m_animator.speed = 1f;
            }
        }
    }

    private void OnAfterPose()
    {
        if (this.CurrSkillClip.PoseTime > this.CurrSkillClip.AttackTime && base.RunningTime >= this.CurrSkillClip.CloseFistTime)
        {
            this.CurrSkillClip.ChangeState(SkillClip.State.CloseFist);
            this.OnUpdate(this.Parent);
        }
        else if (base.RunningTime >= this.CurrSkillClip.AttackTime)
        {
            this.CurrSkillClip.ChangeState(SkillClip.State.Attack);
            this.OnUpdate(this.Parent);
        }
    }

    private void OnAttack()
    {
        if (this.CurrSkillClip.PoseTime > this.CurrSkillClip.AttackTime && base.RunningTime >= this.CurrSkillClip.PoseTime)
        {
            this.CurrSkillClip.ChangeState(SkillClip.State.OnPose);
            if (this.CurrSkillClip.AnimData.mPoseType == FFActionClip.PoseType.Pauce)
            {
                if (this.Parent.m_animator != null)
                {
                    this.Parent.m_animator.speed = 0f;
                }
                this.PlayLoopAudio(this.Parent.Owmner.ModelObj.transform, this.CurrSkillClip.AnimData.ClipName, true);
            }
            else if (this.CurrSkillClip.AnimData.mPoseType == FFActionClip.PoseType.Loop)
            {
                this.LoopTime = 0f;
                this.PlayLoopAudio(this.Parent.Owmner.ModelObj.transform, this.CurrSkillClip.AnimData.ClipName, true);
            }
        }
        else if (base.RunningTime >= this.CurrSkillClip.CloseFistTime)
        {
            this.CurrSkillClip.ChangeState(SkillClip.State.CloseFist);
            this.OnUpdate(this.Parent);
        }
    }

    private void OnCloseFist()
    {
        if (this.CurrSkillClip.AnimConfig.CanCancelCloseFist && this.SkipToNextClip())
        {
            return;
        }
        if (base.RunningTime >= this.CurrSkillClip.TotalLength)
        {
            this.CurrSkillClip.ChangeState(SkillClip.State.Over);
            if (this.SkillClipQueue.Count == 0)
            {
                this.CurrSkillClip = null;
                PlayerBufferControl component = this.Parent.Owmner.GetComponent<PlayerBufferControl>();
                if (component == null || !component.ResetSetPlayerBuffActionBehaviour())
                {
                    this.Parent.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
                }
                if (this.Parent.Owmner == MainPlayer.Self)
                {
                    MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().OnBreak(CSkillBreakType.SelfTimeOut);
                    MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().OnChangeAttack();
                }
            }
            else
            {
                this.SkipToNextClip();
            }
        }
    }

    public bool SkipToNextClip()
    {
        if (this.SkillClipQueue.Count == 0)
        {
            return false;
        }
        SkillClip currSkillClip = this.SkillClipQueue.Dequeue();
        if (this.CurrSkillClip != null)
        {
            this.CurrSkillClip.ChangeState(SkillClip.State.Over);
            if (this.CurrSkillClip.AnimData.mPoseType != FFActionClip.PoseType.None)
            {
                this.PlayLoopAudio(this.Parent.Owmner.ModelObj.transform, this.CurrSkillClip.AnimData.ClipName, false);
            }
        }
        this.CurrSkillClip = currSkillClip;
        this.CurrSkillClip.ChangeState(SkillClip.State.None);
        this.CurrSkillClip.Init(this);
        base.RunningTime = 0f;
        return true;
    }

    private void OnCurrSkillClipChange(SkillClip oldsc, SkillClip newsc)
    {
        if (oldsc != null && oldsc.AnimData.mPoseType != FFActionClip.PoseType.None)
        {
            FlyObjControl component = this.Parent.Owmner.GetComponent<FlyObjControl>();
            component.DisposeFlyObjHold(this.CurrFlyobj);
        }
    }

    public override void OnExit(StateMachine parent)
    {
        for (int i = 0; i < this.FFeffectlist.Count; i++)
        {
            FFeffect ffeffect = this.FFeffectlist[i];
            if (ffeffect.mState != FFeffect.State.Play || ffeffect.Clip.IsInfinite)
            {
                ffeffect.mState = FFeffect.State.Over;
            }
        }
        SkillClip[] array = this.SkillClipQueue.ToArray();
        for (int j = 0; j < array.Length; j++)
        {
            array[j].ChangeState(SkillClip.State.Over);
        }
        if (this.Parent.Owmner == MainPlayer.Self)
        {
            MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().OnBreak(CSkillBreakType.SelfTimeOut);
        }
        if (this.CurrSkillClip != null && this.CurrSkillClip.AnimData.mPoseType != FFActionClip.PoseType.None)
        {
            this.PlayLoopAudio(this.Parent.Owmner.ModelObj.transform, this.CurrSkillClip.AnimData.ClipName, false);
        }
        this.CurrSkillClip = null;
        if (this.Parent.m_animator != null)
        {
            this.Parent.m_animator.speed = 1f;
        }
        base.OnExit(parent);
        this.StoreToPool();
    }

    private void AddCurrClipEffect()
    {
        foreach (string name in this.CurrSkillClip.AnimData.GetEffectsByGroupID(FFActionClip.EffectType.Type_Skill, this.CurrSkillClip.AnimConfig.EffectId))
        {
            this.AddEffect(name, this.CurrSkillClip.PoseTime + this.CurrSkillClip.PoseloopLength, this.CurrSkillClip.PoseLength);
        }
        if (this.CurrSkillClip.ServerData != null && this.CurrSkillClip.ServerData.TargetPos != Vector2.zero)
        {
            Vector3 pos = GraphUtils.GetWorldPosByServerPos(this.CurrSkillClip.ServerData.TargetPos);
            pos = this.Parent.Owmner.GetCharactorY(pos);
            FFEffectControl component = this.Parent.Owmner.ComponentMgr.GetComponent<FFEffectControl>();
            if (component != null)
            {
                MainPlayerSkillHolder component2 = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
                if (component2 != null && this.CurrSkillClip != null)
                {
                    CharactorBase targetFromEntitiesID = component2.GetTargetFromEntitiesID(this.CurrSkillClip.ServerData.targetID);
                    component.SetTarget(this.CurrSkillClip.AnimData.ClipName, pos, (targetFromEntitiesID == null) ? null : targetFromEntitiesID.ModelObj);
                }
            }
        }
        if (this.CurrSkillClip.ServerData != null && this.CurrSkillClip.ServerData.TargetEffectPos != Vector2.zero)
        {
            Vector3 pos2 = GraphUtils.GetWorldPosByServerPos(this.CurrSkillClip.ServerData.TargetEffectPos);
            pos2 = this.Parent.Owmner.GetCharactorY(pos2);
            FlyObjControl component3 = this.Parent.Owmner.GetComponent<FlyObjControl>();
            if (component3 != null)
            {
                this.CurrFlyobj = component3.AddFlyObjArray(this.CurrSkillClip.AnimData.GetEffectsByGroupID(FFActionClip.EffectType.Type_Fly, 1U), pos2, FlyObjConfig.LaunchType.ByStart);
            }
        }
        else
        {
            CharactorBase targetCharactor = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().TargetCharactor;
            if (targetCharactor != null)
            {
                EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
                RelationType relationType = manager.CheckRelationBaseMainPlayer(targetCharactor);
                if (targetCharactor != null && (relationType == RelationType.Enemy || relationType == RelationType.Neutral))
                {
                    FlyObjControl component4 = this.Parent.Owmner.GetComponent<FlyObjControl>();
                    if (component4 != null)
                    {
                        component4.AddFlyObjArray(this.CurrSkillClip.AnimData.GetEffectsByGroupID(FFActionClip.EffectType.Type_Fly, 1U), targetCharactor.ModelObj.transform, FlyObjConfig.LaunchType.ByStart);
                    }
                }
            }
        }
    }

    private void AddEffect(string name, float posetime, float poselangth)
    {
        FFEffectControl component = this.Parent.Owmner.ComponentMgr.GetComponent<FFEffectControl>();
        if (component == null)
        {
            return;
        }
        FFeffect eff = component.AddEffect(name, null, null);
        if (eff == null)
        {
            return;
        }
        this.FFeffectlist.Add(eff);
        eff.Tag = this.CurrSkillClip.AnimData.ClipName;
        if (this.CurrSkillClip.AnimData.mPoseType != FFActionClip.PoseType.None && eff.StartTime > posetime)
        {
            eff.StartTime += poselangth;
        }
        if (eff.Clip.IsInfinite)
        {
            SkillClip currSkillClip = this.CurrSkillClip;
            currSkillClip.OnStateChange = (Action<SkillClip>)Delegate.Combine(currSkillClip.OnStateChange, new Action<SkillClip>(delegate (SkillClip sc)
            {
                if (sc.mState == SkillClip.State.Over)
                {
                    eff.mState = FFeffect.State.Over;
                }
            }));
        }
    }

    private void AddMatEffect(string name, float posetime, float poselangth)
    {
        FFMaterialEffectControl component = this.Parent.Owmner.ComponentMgr.GetComponent<FFMaterialEffectControl>();
        if (component == null)
        {
            return;
        }
        FFMaterialAnimClip clip = ManagerCenter.Instance.GetManager<FFMaterialEffectManager>().GetClip(name);
        if (clip == null)
        {
            return;
        }
        FFMateffect ffmateffect = new FFMateffect(clip);
        if (ffmateffect.StartTime > posetime)
        {
            ffmateffect.StartTime += poselangth;
        }
        component.AddEffect(ffmateffect);
    }

    public void UpdataOnceAudio(Transform tran, string BindName, float runningtime)
    {
    }

    public void PlayLoopAudio(Transform tran, string BindName, bool startOrEnd)
    {
    }

    public void RestThisObject()
    {
        this._speedcut = 0f;
        this.CurrSkillClip = null;
        this.CurrHasMove = false;
        this.LoopTime = 0f;
        base.RunningTime = 0f;
        this.Parent = null;
        this.FFeffectlist.Clear();
        this.SkillClipQueue.Clear();
    }

    public void StoreToPool()
    {
        ClassPool.Store<FFBehaviourState_Skill>(this, 40);
    }

    private float _speedcut;

    public bool HasNewSkill;

    private SkillClip _currSkillClip;

    private bool CurrHasMove;

    private float LoopTime;

    public Queue<SkillClip> SkillClipQueue = new Queue<SkillClip>();

    private FlyObjHold[] CurrFlyobj;

    private List<FFeffect> FFeffectlist = new List<FFeffect>();
}
