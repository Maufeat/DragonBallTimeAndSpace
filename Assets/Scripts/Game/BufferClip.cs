using System;
using Framework.Managers;

public class BufferClip
{
    public float TransitionDuration
    {
        get
        {
            return this.AnimData.TransitionDurationF / this.FrameRate;
        }
    }

    public void Init(FFBehaviourState_InBuffAction owner)
    {
        this.OrgLengrh = ManagerCenter.Instance.GetManager<AnimatorControllerMgr>().GetAnimationClipTime(this.AnimData.ACName, this.AnimData.ClipName);
        this.PoseTime = this.OrgLengrh * this.GetFrameRate(this.AnimData.PoseTimeStartF);
        this.PoseloopLength = this.OrgLengrh * this.GetFrameRate(this.AnimData.PoseTimeEndF - this.AnimData.PoseTimeStartF);
    }

    public float GetFrameRate(uint Frame)
    {
        return Frame / this.FrameRate / this.OrgLengrh;
    }

    public BufferClip.State mState
    {
        get
        {
            return this._mState;
        }
    }

    public void ChangeState(BufferClip.State next)
    {
        this._mState = next;
    }

    public FFActionClip AnimData;

    public float PoseTime;

    public float PoseloopLength;

    public float FrameRate = 30f;

    public float OrgLengrh;

    private BufferClip.State _mState;

    public enum State
    {
        None,
        BeforePose,
        OnPose,
        Over
    }
}
