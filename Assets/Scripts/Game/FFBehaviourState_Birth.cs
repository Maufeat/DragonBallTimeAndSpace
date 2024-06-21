using System;
using LuaInterface;
using UnityEngine;

public class FFBehaviourState_Birth : FFBehaviourBaseState, IstorebAble
{
    public override void OnEnter(StateMachine parent)
    {
        base.OnEnter(parent);
        this.HitAnimTime = 0f;
        this.PlayAnim();
    }

    public uint getBirthClip()
    {
        if (this.Parent.Owmner.EID.Etype == CharactorType.NPC)
        {
            Npc npc = this.Parent.Owmner as Npc;
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
            return configTable.GetField_Uint("bornaction");
        }
        return 0U;
    }

    public void PlayAnim()
    {
        uint birthClip = this.getBirthClip();
        if (birthClip > 0U)
        {
            this.HitAnimLength = this.Parent.PlayNormalAction(birthClip, false, 0f);
            this.onBirthAnim = true;
        }
        else
        {
            this.ChangeToIdleState();
        }
        if (this.Parent.Owmner is Npc)
        {
            Npc npc = this.Parent.Owmner as Npc;
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
            if (string.IsNullOrEmpty(configTable.GetField_String("borneffect")) || string.Equals(configTable.GetField_String("borneffect"), "0"))
            {
                return;
            }
            FFEffectControl component = this.Parent.Owmner.GetComponent<FFEffectControl>();
            if (component == null)
            {
                FFDebug.LogWarning(this, "FFEffectControl is null");
                return;
            }
            component.AddEffectGroupOnce(configTable.GetField_String("borneffect"));
        }
    }

    private void UpdateNormalHit()
    {
        if (!this.onBirthAnim)
        {
            return;
        }
        this.HitAnimTime += Time.deltaTime;
        if (this.HitAnimTime >= this.HitAnimLength)
        {
            this.ChangeToIdleState();
            this.onBirthAnim = false;
            this.HitAnimTime = 0f;
        }
    }

    private void ChangeToIdleState()
    {
        this.Parent.Owmner.GetComponent<FFBehaviourControl>().ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
    }

    public override void OnUpdate(StateMachine parent)
    {
        base.OnUpdate(parent);
        this.UpdateNormalHit();
    }

    public override void OnExit(StateMachine parent)
    {
        base.OnExit(parent);
        this.Parent.Owmner.CharState = CharactorState.CreatComplete;
        uint birthClip = this.getBirthClip();
        this.Parent.DisPoseEffect(birthClip);
        if (this.OnBirthOver != null)
        {
            this.OnBirthOver();
            this.OnBirthOver = null;
        }
        this.StoreToPool();
    }

    public bool IsDirty { get; set; }

    public void RestThisObject()
    {
        this.OnBirthOver = null;
        this.onBirthAnim = false;
        this.HitAnimTime = 0f;
        this.HitAnimLength = 0f;
        base.RunningTime = 0f;
        this.Parent = null;
    }

    public void StoreToPool()
    {
        ClassPool.Store<FFBehaviourState_Birth>(this, 60);
    }

    private bool onBirthAnim;

    private float HitAnimTime;

    private float HitAnimLength;

    public Action OnBirthOver;
}
