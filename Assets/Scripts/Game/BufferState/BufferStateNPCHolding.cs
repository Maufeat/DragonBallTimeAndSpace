using System;
using LuaInterface;
using UnityEngine;

public class BufferStateNPCHolding : BufferState
{
    public BufferStateNPCHolding(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        this.pbc = PBControl;
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.InLook));
    }

    public override void Exit()
    {
        base.Exit();
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.InLook));
    }

    private void InLook()
    {
        if (this.FFBC == null)
        {
            this.FFBC = this.BufferControl.Owner.GetComponent<FFBehaviourControl>();
        }
        if (this.pbc.Owner == MainPlayer.Self && this.FFBC != null)
        {
            bool flag = true;
            if (this.FFBC.Owmner is Npc)
            {
                Npc npc = this.FFBC.Owmner as Npc;
                LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
                if (configTable != null)
                {
                    flag = (configTable.GetCacheField_Int("lookat") == 0);
                }
            }
            if (flag)
            {
                Vector3 position = MainPlayer.Self.ModelObj.transform.position;
                position.y = this.FFBC.Owmner.ModelObj.transform.position.y;
                this.FFBC.Owmner.ModelObj.transform.LookAt(position);
            }
        }
    }

    private FFBehaviourControl FFBC;

    private PlayerBufferControl pbc;
}
