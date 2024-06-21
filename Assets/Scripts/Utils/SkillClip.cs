using System;
using Framework.Managers;

public class SkillClip : IstorebAble
{
    public SkillClip.State mState
    {
        get
        {
            return this._mState;
        }
    }

    public float TransitionDuration
    {
        get
        {
            return this.AnimData.TransitionDurationF / this.FrameRate;
        }
    }

    public void Init(FFBehaviourState_Skill owner)
    {
        this.OrgLengrh = this.GetFrameTime(this.AnimData.CloseFistTimeF);
        if (owner.Parent.m_animator != null && owner.Parent.allAnimationClip.ContainsKey(this.AnimData.ClipName))
        {
            this.OrgLengrh = ManagerCenter.Instance.GetManager<AnimatorControllerMgr>().GetAnimationClipTime(this.AnimData.ACName, this.AnimData.ClipName);
        }
        if (this.ServerData != null && this.ServerData.ChantLength > 0f)
        {
            this.TurePoseLength = this.ServerData.ChantLength;
            this.PoseLength = this.ServerData.ChantLength;
        }
        else
        {
            this.TurePoseLength = this.AnimConfig.PoseTime;
            this.PoseLength = this.TurePoseLength;
        }
        this.TurePoseLength = ((this.TurePoseLength <= 0f) ? 0f : this.TurePoseLength);
        this.TotalLength = this.OrgLengrh + this.AnimConfig.PoseTime;
        this.PoseTime = this.GetFrameTime(this.AnimData.PoseTimeStartF);
        this.PoseOverTime = this.GetFrameTime(this.AnimData.PoseTimeEndF);
        this.PoseloopLength = this.GetFrameTime(this.AnimData.PoseTimeEndF - this.AnimData.PoseTimeStartF);
        this.AttackTime = this.GetFrameTime(this.AnimData.AttackTimeF);
        this.CloseFistTime = this.GetFrameTime(this.AnimData.CloseFistTimeF);
        this.StartMoveTime = this.GetFrameTime(this.AnimData.StartMoveF);
        this.MoveLength = this.GetFrameTime(this.AnimData.EndMoveF - this.AnimData.StartMoveF);
        this.MirrorTotalTime = this.AnimData.MirrorTotalTime;
        this.MirrorFadeSpeed = this.AnimData.MirrorFadeSpeed;
        this.CanMoveCancelSkillTime = this.GetFrameTime(this.AnimData.CanMoveCancelSkillTime);
        this.CanMoveCancelCloseFistTime = this.GetFrameTime(this.AnimData.CanMoveCancelCloseFistTime);
        this.TotalSkillTime = this.LvConfig.SkillTotalTime;
        if (this.AnimData.StartMoveF > this.AnimData.PoseTimeEndF)
        {
            this.StartMoveTime += this.TurePoseLength;
        }
    }

    public float GetFrameTime(uint Frame)
    {
        return Frame / this.FrameRate;
    }

    public void ChangeState(SkillClip.State next)
    {
        this._mState = next;
        if (this.OnStateChange != null)
        {
            this.OnStateChange(this);
        }
        if (this._mState == SkillClip.State.Over)
        {
            this.StoreToPool();
        }
    }

    public bool IsDirty { get; set; }

    public void RestThisObject()
    {
        this.AnimData = null;
        this.AnimConfig = null;
        this.ServerData = null;
        this.LvConfig = null;
        this.TotalLength = 0f;
        this.PoseTime = 0f;
        this.PoseLength = 0f;
        this.PoseloopLength = 0f;
        this.AttackTime = 0f;
        this.CloseFistTime = 0f;
        this.StartMoveTime = 0f;
        this.MoveLength = 0f;
        this.CanMoveCancelSkillTime = 0f;
        this.CanMoveCancelCloseFistTime = 0f;
        this._mState = SkillClip.State.None;
        this.OrgLengrh = 0f;
        this.TurePoseLength = 0f;
        this.OnStateChange = null;
    }

    public void StoreToPool()
    {
        if (this.AnimConfig != null)
        {
            this.AnimConfig.StoreToPool();
        }
        this.AnimConfig = null;
        if (this.LvConfig != null)
        {
            this.LvConfig.StoreToPool();
        }
        this.LvConfig = null;
        ClassPool.Store<SkillClip>(this, 40);
    }

    public FFActionClip AnimData;

    public SkillClipConfig AnimConfig;

    public SkillClipServerDate ServerData;

    public LVSkillConfig LvConfig;

    public float TotalLength;

    public float PoseTime;

    public float PoseOverTime;

    public float PoseLength;

    public float PoseloopLength;

    public float AttackTime;

    public float CloseFistTime;

    public float StartMoveTime;

    public float MoveLength;

    public float CanMoveCancelSkillTime;

    public float CanMoveCancelCloseFistTime;

    private SkillClip.State _mState;

    private float OrgLengrh;

    private float TurePoseLength;

    public Action<SkillClip> OnStateChange;

    public float MirrorTotalTime;

    public float MirrorFadeSpeed;

    public ulong SkillStageId;

    public float TotalSkillTime;

    public float FrameRate = 30f;

    public enum State
    {
        None,
        BeforePose,
        OnPose,
        AfterPose,
        Attack,
        CloseFist,
        Over
    }
}
