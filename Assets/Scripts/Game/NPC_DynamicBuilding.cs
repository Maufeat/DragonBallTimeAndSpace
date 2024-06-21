using System;

public class NPC_DynamicBuilding : Npc_Building
{
    public NPC_DynamicBuilding()
    {
        base.Init();
    }

    public override void OnUpdateCharacterBuff(UserState newState)
    {
        if (newState == UserState.USTATE_DOOR_OPEN)
        {
            this.RemoveBlockData();
            if (base.ModelObj != null)
            {
                base.ModelObj.SetActive(false);
            }
        }
    }

    public override void OnRemoveCharacterBuff(UserState oldState)
    {
        if (oldState == UserState.USTATE_DOOR_OPEN)
        {
            this.CombineBlockData();
            if (base.ModelObj != null)
            {
                base.ModelObj.SetActive(true);
            }
        }
    }

    public override void AddBuffCtrl()
    {
        base.AddComponent(new FFBehaviourControl());
        base.AddComponent(new PlayerBufferControl());
        this.ComponentMgr.InitOver();
        PlayerBufferControl component = base.GetComponent<PlayerBufferControl>();
        if (component != null)
        {
            component.AddStateByArray(base.NpcData.MapNpcData.lstState);
        }
    }
}
