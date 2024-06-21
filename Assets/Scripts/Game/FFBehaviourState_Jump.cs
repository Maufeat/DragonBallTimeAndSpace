using System;
using Framework.Managers;
using UnityEngine;

public class FFBehaviourState_Jump : FFBehaviourBaseState, IstorebAble
{
    public override void OnEnter(StateMachine parent)
    {
        base.OnEnter(parent);
        if (MainPlayer.Self != null)
        {
            MainPlayer.Self.SwitchAutoAttack(false);
        }
        this.Parent.LockState = true;
        this.m_jumpState = FFBehaviourState_Jump.State.BeginJump;
        this.CurrPosY = MapHightDataHolder.GetMapHeight(this.Parent.Owmner.ModelObj.transform.position.x, this.Parent.Owmner.ModelObj.transform.position.z);
        this.JumpHigh = 0f;
        this.v1 = SingletonForMono<GameTime>.Instance.v0;
        this.Parent.Owmner.JumpHeight = 0f;
        this.Parent.Owmner.FallCheckingHeight = this.Parent.Owmner.ModelObj.transform.position.y;
        this.canChange = false;
        this.PlayJump();
        GameObject objParent = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.hpdata.objParent;
        UIFllowTarget component = objParent.GetComponent<UIFllowTarget>();
        float actionTime = this.Parent.GetActionTime(1U);
        float actionTime2 = this.Parent.GetActionTime(4U);
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
        if (this.m_jumpState == FFBehaviourState_Jump.State.BeginJump)
        {
            this.m_jumpState = FFBehaviourState_Jump.State.JumpingOn;
            return true;
        }
        if (this.m_jumpState == FFBehaviourState_Jump.State.JumpingOn)
        {
            this.m_jumpState = FFBehaviourState_Jump.State.JumpingDown;
            return true;
        }
        if (this.m_jumpState == FFBehaviourState_Jump.State.JumpingDown)
        {
            this.m_jumpState = FFBehaviourState_Jump.State.EndJump;
            return true;
        }
        return false;
    }

    private void PlayJump()
    {
        NormalActionId actionid = NormalActionId.BeginJump;
        switch (this.m_jumpState)
        {
            case FFBehaviourState_Jump.State.BeginJump:
                actionid = NormalActionId.BeginJump;
                break;
            case FFBehaviourState_Jump.State.JumpingOn:
                actionid = NormalActionId.Jumping;
                break;
            case FFBehaviourState_Jump.State.JumpingDown:
                actionid = NormalActionId.EndJump;
                break;
            case FFBehaviourState_Jump.State.EndJump:
                actionid = NormalActionId.EndJump;
                break;
        }
        this.m_playTime = this.Parent.PlayNormalAction((uint)actionid, false, 0.1f);
    }

    public override void OnUpdate(StateMachine parent)
    {
        base.OnUpdate(parent);
        this.m_timer += Time.deltaTime;
        if (this.m_jumpState == FFBehaviourState_Jump.State.BeginJump)
        {
            if ((double)this.m_timer < 0.12)
            {
                return;
            }
            float num = this.v1 * Time.deltaTime;
            this.CurrPosY += num;
            this.JumpHigh += num;
            this.Parent.Owmner.FallCheckingHeight += num;
            this.v1 -= SingletonForMono<GameTime>.Instance.a * Time.deltaTime;
            float mapHeight = MapHightDataHolder.GetMapHeight(this.Parent.Owmner.ModelObj.transform.position.x, this.Parent.Owmner.ModelObj.transform.position.z);
            this.Parent.Owmner.JumpHeight = this.CurrPosY - mapHeight;
            if (this.Parent.Owmner.JumpHeight <= 0f)
            {
                this.Parent.Owmner.JumpHeight = 0f;
                this.Parent.Owmner.FallCheckingHeight = mapHeight;
                if ((double)this.m_timer > 0.18)
                {
                    this.Parent.Owmner.OnJumpLand();
                }
            }
            if (this.v1 <= 0f)
            {
                this.v1 = 0f;
            }
            if (this.m_timer >= this.m_playTime)
            {
                this.ChangeToNextState();
                this.PlayJump();
            }
        }
        else if (this.m_jumpState == FFBehaviourState_Jump.State.JumpingOn)
        {
            this.CurrPosY -= this.v1 * Time.deltaTime;
            this.Parent.Owmner.FallCheckingHeight -= this.v1 * Time.deltaTime;
            this.Parent.Owmner.JumpHeight = this.CurrPosY - MapHightDataHolder.GetMapHeight(this.Parent.Owmner.ModelObj.transform.position.x, this.Parent.Owmner.ModelObj.transform.position.z);
            this.v1 += SingletonForMono<GameTime>.Instance.a * Time.deltaTime;
            if ((this.Parent.Owmner.JumpHeight <= this.JumpHigh && !this.Parent.Owmner.isFalling) || (this.Parent.Owmner.isFalling && this.Parent.Owmner.ModelObj.transform.position.y <= 0f))
            {
                this.v1 = (float)((double)this.JumpHigh / 0.27);
                this.ChangeToNextState();
                this.PlayJump();
            }
        }
        else if (this.m_jumpState == FFBehaviourState_Jump.State.JumpingDown)
        {
            this.CurrPosY -= this.v1 * Time.deltaTime;
            this.Parent.Owmner.FallCheckingHeight -= this.v1 * Time.deltaTime;
            float mapHeight2 = MapHightDataHolder.GetMapHeight(this.Parent.Owmner.ModelObj.transform.position.x, this.Parent.Owmner.ModelObj.transform.position.z);
            this.Parent.Owmner.JumpHeight = this.CurrPosY - mapHeight2;
            if (this.Parent.Owmner.JumpHeight <= 0f)
            {
                this.Parent.Owmner.JumpHeight = 0f;
            }
            if (mapHeight2 > 0f && this.Parent.Owmner.FallCheckingHeight > 0f && this.Parent.Owmner.FallCheckingHeight < mapHeight2)
            {
                this.Parent.Owmner.FallCheckingHeight = mapHeight2;
            }
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
        this.CurrMoveDir = -1;
        this.Parent.Owmner.ServerDir = this.Parent.Owmner.FaceDir;
        this.Parent.Owmner.RefreshPhysicsPos();
    }

    public void RestThisObject()
    {
        this.ResetThisObjectWithoutParent();
        this.Parent = null;
    }

    private void ResetThisObjectWithoutParent()
    {
        this.m_jumpState = FFBehaviourState_Jump.State.BeginJump;
        this.m_playTime = 0f;
        this.m_timer = 0f;
        base.RunningTime = 0f;
    }

    public void StoreToPool()
    {
        ClassPool.Store<FFBehaviourState_Jump>(this, 60);
    }

    private const float v0 = 4.6f;

    private const float a = 9.8f;

    public const float RECOVERY_TIME = 0.06666667f;

    private FFBehaviourState_Jump.State m_jumpState;

    private float m_playTime;

    private float m_timer;

    private bool canChange;

    private float v1;

    private float CurrPosY;

    private float JumpHigh;

    public enum State
    {
        BeginJump = 1,
        JumpingOn,
        JumpingDown,
        EndJump
    }
}
