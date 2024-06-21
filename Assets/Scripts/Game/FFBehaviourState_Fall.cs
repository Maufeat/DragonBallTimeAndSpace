using System;
using Framework.Managers;
using UnityEngine;

public class FFBehaviourState_Fall : FFBehaviourBaseState, IstorebAble
{
    public override void OnEnter(StateMachine parent)
    {
        base.OnEnter(parent);
        this.Parent.LockState = true;
        this.m_jumpState = FFBehaviourState_Fall.State.JumpingOn;
        this.v1 = SingletonForMono<GameTime>.Instance.v0;
        this.Parent.Owmner.JumpHeight = 0f;
        this.canChange = false;
        this.PlayJump();
        GameObject objParent = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.hpdata.objParent;
        UIFllowTarget component = objParent.GetComponent<UIFllowTarget>();
        if (this.Parent.Owmner is MainPlayer)
        {
            this.CurrMoveDir = SingletonForMono<InputController>.Instance.InputDir;
        }
    }

    public bool CanChange()
    {
        return this.canChange || this.canChange;
    }

    private bool ChangeToNextState()
    {
        this.m_timer = 0f;
        if (this.m_jumpState == FFBehaviourState_Fall.State.JumpingOn)
        {
            this.m_jumpState = FFBehaviourState_Fall.State.JumpingDown;
            return true;
        }
        if (this.m_jumpState == FFBehaviourState_Fall.State.JumpingDown)
        {
            this.m_jumpState = FFBehaviourState_Fall.State.EndJump;
            return true;
        }
        return false;
    }

    private void PlayJump()
    {
        NormalActionId actionid = NormalActionId.BeginJump;
        switch (this.m_jumpState)
        {
            case FFBehaviourState_Fall.State.JumpingOn:
                actionid = NormalActionId.Jumping;
                break;
            case FFBehaviourState_Fall.State.JumpingDown:
                actionid = NormalActionId.EndJump;
                break;
            case FFBehaviourState_Fall.State.EndJump:
                actionid = NormalActionId.EndJump;
                break;
        }
        this.m_playTime = this.Parent.PlayNormalAction((uint)actionid, false, 0.1f);
    }

    public override void OnUpdate(StateMachine parent)
    {
        base.OnUpdate(parent);
        this.m_timer += Time.deltaTime;
        if (this.m_jumpState == FFBehaviourState_Fall.State.JumpingOn)
        {
            this.Parent.Owmner.FallCheckingHeight -= this.v1 * Time.deltaTime;
            this.v1 += SingletonForMono<GameTime>.Instance.a * Time.deltaTime;
            if (this.Parent.Owmner.FallCheckingHeight <= 0f)
            {
                this.Parent.Owmner.FallCheckingHeight = 0f;
                this.ChangeToNextState();
                this.PlayJump();
            }
        }
        else if (this.m_jumpState == FFBehaviourState_Fall.State.JumpingDown)
        {
            this.Parent.Owmner.FallCheckingHeight -= this.v1 * Time.deltaTime;
            float mapHeight = MapHightDataHolder.GetMapHeight(this.Parent.Owmner.ModelObj.transform.position.x, this.Parent.Owmner.ModelObj.transform.position.z);
            if (this.Parent.Owmner.FallCheckingHeight <= 0f)
            {
                this.Parent.Owmner.FallCheckingHeight = 0f;
            }
            if ((double)this.m_timer + 0.2 >= (double)this.m_playTime)
            {
                this.Parent.Owmner.JumpHeight = 0f;
                this.Parent.LockState = false;
                this.canChange = true;
            }
            if ((double)this.m_timer + 0.12 >= (double)this.m_playTime && this.Parent.Owmner is MainPlayer)
            {
                if (!this.Parent.Owmner.Pfc.isPathfinding)
                {
                    this.Parent.Owmner.StopMoveImmediate(null);
                }
                this.CurrMoveDir = -1;
            }
            if (this.m_timer >= this.m_playTime)
            {
                this.Parent.Owmner.JumpHeight = 0f;
                this.CurrMoveDir = -1;
                this.Parent.Owmner.RefreshPhysicsPos();
                this.Parent.Owmner.OnJumpLand();
            }
        }
        if (this.Parent.Owmner is MainPlayer)
        {
            if (!this.Parent.Owmner.Pfc.isPathfinding && SingletonForMono<InputController>.Instance.InputDir == -1 && this.CurrMoveDir == -1)
            {
                this.Parent.Owmner.RefreshPhysicsPos();
            }
        }
        else if (!this.Parent.Owmner.IsMoving)
        {
            this.Parent.Owmner.RefreshPhysicsPos();
        }
    }

    public bool IsDirty { get; set; }

    public override void OnExit(StateMachine parent)
    {
        base.OnExit(parent);
        this.StoreToPool();
        this.Parent.LockState = false;
        this.Parent.Owmner.JumpHeight = 0f;
    }

    public void RestThisObject()
    {
        this.ResetThisObjectWithoutParent();
        this.Parent = null;
    }

    private void ResetThisObjectWithoutParent()
    {
        this.m_jumpState = FFBehaviourState_Fall.State.JumpingOn;
        this.m_playTime = 0f;
        this.m_timer = 0f;
        base.RunningTime = 0f;
    }

    public void StoreToPool()
    {
        ClassPool.Store<FFBehaviourState_Fall>(this, 60);
    }

    private FFBehaviourState_Fall.State m_jumpState;

    private float m_playTime;

    private float m_timer;

    private bool canChange;

    private float v1;

    public enum State
    {
        JumpingOn = 1,
        JumpingDown,
        EndJump
    }
}
