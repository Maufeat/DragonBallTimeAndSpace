using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Managers;
using UnityEngine;

public class FFBehaviourState_InBuffAction : FFBehaviourBaseState, IstorebAble
{
    public override void OnEnter(StateMachine parent)
    {
        base.OnEnter(parent);
        this.Parent.Owmner.StopMoveImmediate(null);
    }

    public override void OnUpdate(StateMachine parent)
    {
        this.CheckHighestPriority();
        if (this.CurrBufferClip == null)
        {
            this.SkipToNextClip();
            return;
        }
        base.OnUpdate(parent);
        switch (this.CurrBufferClip.mState)
        {
            case BufferClip.State.None:
                this.OnStartBuffClip();
                break;
            case BufferClip.State.BeforePose:
                this.OnBeforePose();
                break;
            case BufferClip.State.OnPose:
                this.OnPose();
                break;
        }
    }

    public override void OnExit(StateMachine parent)
    {
        if (this.Parent.m_animator != null)
        {
            this.Parent.m_animator.speed = 1f;
        }
        base.OnExit(parent);
    }

    private void AddBufferClip(BufferClip clip)
    {
        if (clip == null)
        {
            FFDebug.LogWarning(this, this.Parent.Owmner.animatorControllerName + "add null BufferClip");
            return;
        }
        if (clip.AnimData.ClipName == "none")
        {
            return;
        }
        if (!this.Parent.allAnimationClip.ContainsKey(clip.AnimData.ClipName))
        {
            FFDebug.LogWarning(this, this.Parent.Owmner.animatorControllerName + "Dont have Clip : [" + clip.AnimData.ClipName + "]");
            return;
        }
        this.BufferClipQueue.Enqueue(clip);
    }

    public void OnStartBuffClip()
    {
        this.CurrBufferClip.Init(this);
        if (this.PlayOnstart)
        {
            if (this.PlayImmediately)
            {
                this.Parent.PlayAnim(this.CurrBufferClip.AnimData.ClipName, 0f, this.CurrBufferClip.PoseTime);
                base.RunningTime = this.CurrBufferClip.PoseTime;
                this.PlayImmediately = false;
            }
            else
            {
                this.Parent.PlayAnim(this.CurrBufferClip.AnimData.ClipName, this.CurrBufferClip.TransitionDuration, 0f);
                base.RunningTime = 0f;
            }
        }
        this.CurrBufferClip.ChangeState(BufferClip.State.BeforePose);
        this.Parent.m_animator.speed = 1f;
    }

    public void OnBeforePose()
    {
        if (this.CurrBufferClip.AnimData.mPoseType == FFActionClip.PoseType.None)
        {
            if (base.RunningTime >= this.CurrBufferClip.OrgLengrh)
            {
                this.SkipToNextClip();
            }
        }
        else if (base.RunningTime >= this.CurrBufferClip.PoseTime)
        {
            this.CurrBufferClip.ChangeState(BufferClip.State.OnPose);
            if (this.CurrBufferClip.AnimData.mPoseType == FFActionClip.PoseType.Pauce)
            {
                if (this.Parent.m_animator != null)
                {
                    this.Parent.m_animator.speed = 0f;
                }
            }
            else if (this.CurrBufferClip.AnimData.mPoseType == FFActionClip.PoseType.Loop)
            {
                this.LoopTime = 0f;
            }
        }
    }

    public void OnPose()
    {
        this.LoopTime += Time.deltaTime;
        if (this.CurrBufferClip.AnimData.mPoseType == FFActionClip.PoseType.Loop && this.LoopTime >= this.CurrBufferClip.PoseloopLength)
        {
            this.Parent.PlayAnim(this.CurrBufferClip.AnimData.ClipName, 0f, this.CurrBufferClip.PoseTime);
            this.LoopTime = 0f;
        }
    }

    private void SkipToNextClip()
    {
        if (this.BufferClipQueue.Count == 0)
        {
            this.Parent.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
            return;
        }
        BufferClip currBufferClip = this.BufferClipQueue.Dequeue();
        if (this.CurrBufferClip != null)
        {
            this.CurrBufferClip.ChangeState(BufferClip.State.Over);
        }
        this.CurrBufferClip = currBufferClip;
        this.CurrBufferClip.ChangeState(BufferClip.State.None);
    }

    public void SetBuffer(BufferState buffer)
    {
        FFDebug.Log(this, FFLogType.Buff, string.Format("{0} SetBuffer In FFBehaviourState_InBuffAction {1}", this.Parent.Owmner.EID.ToString(), buffer.mFlag.ToString()));
        if (!this.ControllBufferlist.ContainsKey(buffer.mFlag))
        {
            this.ControllBufferlist[buffer.mFlag] = buffer;
        }
    }

    public void RemoveBuffer(UserState Flag, bool playRevertAnim)
    {
        if (this.ControllBufferlist.ContainsKey(Flag))
        {
            this.ControllBufferlist.Remove(Flag);
            if (this.CurrBufferState == null)
            {
                FFDebug.Log(this, FFLogType.Buff, "CurrBufferState null");
                return;
            }
            if (this.CurrBufferState.mFlag == Flag)
            {
                if (playRevertAnim)
                {
                    BufferClip bufferClip = this.GetBufferClip(this.CurrBufferState.CurrBuffConfig.GetField_Uint("RevertAnim"));
                    if (bufferClip != null)
                    {
                        this.AddBufferClip(bufferClip);
                    }
                }
                this.SkipToNextClip();
                this.CurrBufferState = null;
            }
        }
    }

    public bool IsDirty { get; set; }

    private void CheckHighestPriority()
    {
        BufferState bufferState = this.CurrBufferState;
        for (int i = 0; i < this.ControllBufferlist.Count; i++)
        {
            BufferState bufferState2 = this.ControllBufferlist.Values.ElementAt(i);
            if (bufferState == null)
            {
                bufferState = bufferState2;
            }
            if (bufferState2.CurrBuffConfig.GetField_Uint("AnimPriority") >= bufferState.CurrBuffConfig.GetField_Uint("AnimPriority"))
            {
                bufferState = bufferState2;
            }
        }
        if (bufferState != this.CurrBufferState)
        {
            this.ChangeCurrBufferState(bufferState);
        }
    }

    private void ChangeCurrBufferState(BufferState buffer)
    {
        if (buffer == this.CurrBufferState)
        {
            return;
        }
        this.CurrBufferState = buffer;
        BufferClip bufferClip = this.GetBufferClip(this.CurrBufferState.CurrBuffConfig.GetField_Uint("BuffAnim"));
        this.PlayOnstart = (this.CurrBufferState.CurrBuffConfig.GetField_Uint("Animtype") != 3U);
        if (bufferClip != null)
        {
            this.AddBufferClip(bufferClip);
            this.CurrBufferClip = null;
        }
        else
        {
            FFDebug.LogWarning(this, string.Format("ChangeCurrBufferState get BufferClip {0}  RevertAnim {1}", buffer.mFlag, buffer.CurrBuffConfig.GetField_Uint("RevertAnim")));
        }
    }

    private BufferClip GetBufferClip(uint actionid)
    {
        if (string.IsNullOrEmpty(this.Parent.Owmner.animatorControllerName))
        {
            return null;
        }
        int selectindex = (!this.Parent.Owmner.IsFly) ? 0 : 1;
        FFActionClip ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(this.Parent.Owmner.animatorControllerName, actionid, selectindex);
        if (ffactionClip == null)
        {
            return null;
        }
        BufferClip bufferClip = new BufferClip();
        bufferClip.AnimData = ffactionClip;
        FFDebug.Log(this, FFLogType.Buff, string.Format("BufferClip :{0}", ffactionClip.ClipName));
        return bufferClip;
    }

    public void RestThisObject()
    {
        this.LoopTime = 0f;
        this.PlayImmediately = false;
        base.RunningTime = 0f;
        this.Parent = null;
        this.BufferClipQueue.Clear();
        this.ControllBufferlist.Clear();
    }

    public void StoreToPool()
    {
        ClassPool.Store<FFBehaviourState_InBuffAction>(this, 60);
    }

    private float LoopTime;

    public bool PlayImmediately;

    public bool PlayOnstart = true;

    private BufferClip CurrBufferClip;

    private Queue<BufferClip> BufferClipQueue = new Queue<BufferClip>();

    private BufferState CurrBufferState;

    private Dictionary<UserState, BufferState> ControllBufferlist = new Dictionary<UserState, BufferState>();
}
