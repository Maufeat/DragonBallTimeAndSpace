using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class FFBehaviourState_Idle : FFBehaviourBaseState, IstorebAble, HandleNormalHit
{
    public override void OnEnter(StateMachine parent)
    {
        base.OnEnter(parent);
        this.playIdle(false);
    }

    public float playIdle(bool force = false)
    {
        this.bRelax = false;
        float num = -1f;
        PlayerBufferControl component = this.Parent.Owmner.GetComponent<PlayerBufferControl>();
        if (component != null && component.ContainsState(UserState.USTATE_BATTLE))
        {
            num = this.Parent.PlayNormalAction(3U, force, 0.1f);
        }
        if (num < 0f)
        {
            num = this.Parent.PlayNormalAction(1U, force, 0.1f);
        }
        return num;
    }

    public void ChangeToXiuxianIdle()
    {
        this.playRelaxAnimation();
    }

    public override void OnUpdate(StateMachine parent)
    {
        base.OnUpdate(parent);
        this.UpdateNormalHit();
        this.UpdateIdle();
    }

    public void HandleNormalHit(CharactorBase From)
    {
        this.onHitAnim = true;
        this.HitAnimLength = this.Parent.PlayNormalAction(5U, true, 0.1f);
        this.HitAnimTime = 0f;
    }

    private void UpdateNormalHit()
    {
        if (!this.onHitAnim)
        {
            return;
        }
        this.HitAnimTime += Time.deltaTime;
        if (this.HitAnimTime >= this.HitAnimLength)
        {
            this.playIdle(true);
            this.onHitAnim = false;
            this.HitAnimTime = 0f;
        }
    }

    private void playRelaxAnimation()
    {
        if (this.Parent.Owmner.IsBattleState)
        {
            return;
        }
        this.bRelax = true;
        this.relaxTimeLength = this.Parent.PlayRandomAction(2U);
    }

    private void PlayNextAnim()
    {
        UnityEngine.Random.seed = 1;
        this.changeAnimInterval = (float)UnityEngine.Random.Range(15, 30);
        this.idleAnimClip = (FFBehaviourState_Idle.IdleAnimClip)UnityEngine.Random.Range(0, 1);
        FFBehaviourState_Idle.IdleAnimClip idleAnimClip = this.idleAnimClip;
        if (idleAnimClip != FFBehaviourState_Idle.IdleAnimClip.NormalIdle)
        {
            if (idleAnimClip == FFBehaviourState_Idle.IdleAnimClip.RelaxIdle)
            {
                this.playRelaxAnimation();
            }
        }
        else
        {
            this.playIdle(false);
        }
    }

    public bool IsDirty { get; set; }

    private void UpdateIdle()
    {
        this.idleTime += Time.deltaTime;
        if (this.idleTime >= this.changeAnimInterval)
        {
            this.PlayNextAnim();
            this.idleTime = 0f;
        }
    }

    public override void OnExit(StateMachine parent)
    {
        base.OnExit(parent);
        this.StoreToPool();
    }

    public void AddClipEffect()
    {
        if (this.Parent.Owmner.EID.Etype != CharactorType.NPC)
        {
            return;
        }
        int selectindex = (!this.Parent.Owmner.IsFly) ? 0 : 1;
        FFActionClip ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(this.Parent.Owmner.animatorControllerName, 1U, selectindex);
        if (ffactionClip == null)
        {
            return;
        }
        if (this.Parent.Owmner.ComponentMgr.GetComponent<FFEffectControl>() == null)
        {
            return;
        }
        string[] effectsByGroupID = ffactionClip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Skill, 1U);
        if (effectsByGroupID == null)
        {
            return;
        }
        for (int i = 0; i < effectsByGroupID.Length; i++)
        {
            this.effectlist.Add(effectsByGroupID[i]);
        }
    }

    public void RestThisObject()
    {
        this.effectlist.Clear();
        this.bRelax = false;
        this.relaxTimeLength = 0f;
        this.idleTime = 0f;
        this.onHitAnim = false;
        this.HitAnimTime = 0f;
        this.HitAnimLength = 0f;
        base.RunningTime = 0f;
        this.Parent = null;
    }

    public void StoreToPool()
    {
        ClassPool.Store<FFBehaviourState_Idle>(this, 60);
    }

    private bool bRelax;

    private float relaxTimeLength;

    private float idleTime;

    private bool onHitAnim;

    private float HitAnimTime;

    private float HitAnimLength;

    private float changeAnimInterval = 15f;

    private FFBehaviourState_Idle.IdleAnimClip idleAnimClip;

    private List<string> effectlist = new List<string>();

    public enum IdleAnimClip
    {
        NormalIdle,
        RelaxIdle
    }
}
