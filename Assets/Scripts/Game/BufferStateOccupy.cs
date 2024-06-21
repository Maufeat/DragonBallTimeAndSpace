using System;
using Framework.Managers;

public class BufferStateOccupy : BufferState
{
    public BufferStateOccupy(UserState Flag)
    {
        base.CurrBuffConfig = base.BufferstateMgr.GetBufferConfig(Flag);
    }

    private OccupyController occupyController
    {
        get
        {
            return ControllerManager.Instance.GetController<OccupyController>();
        }
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        this.BufferControl = PBControl;
        this.relationshipTag = this.occupyController.GetRelationship((this.BufferControl.Owner as Npc).NpcData.MapNpcData.tempid);
        string[] array = base.CurrBuffConfig.GetField_String("BuffEffect").Split(new char[]
        {
            '/'
        });
        if (array.Length == 1)
        {
            this.AddEffect(array[0]);
        }
        else if (array.Length == 2)
        {
            if (this.relationshipTag == 0)
            {
                this.AddEffect(array[0]);
            }
            else if (this.relationshipTag == 1)
            {
                this.AddEffect(array[1]);
            }
        }
    }

    public void RefreshState()
    {
        if (this.BufferControl == null)
        {
            return;
        }
        int relationship = this.occupyController.GetRelationship((this.BufferControl.Owner as Npc).NpcData.MapNpcData.tempid);
        if (relationship == this.relationshipTag)
        {
            return;
        }
        this.relationshipTag = relationship;
        for (int i = 0; i < this.FFeffectList.Count; i++)
        {
            this.FFeffectList[i].mState = FFeffect.State.Over;
        }
        this.FFeffectList.Clear();
        string[] array = base.CurrBuffConfig.GetField_String("BuffEffect").Split(new char[]
        {
            '/'
        });
        if (array.Length == 1)
        {
            this.AddEffect(array[0]);
        }
        else if (array.Length == 2)
        {
            if (this.relationshipTag == 0)
            {
                this.AddEffect(array[0]);
            }
            else if (this.relationshipTag == 1)
            {
                this.AddEffect(array[1]);
            }
        }
    }

    public override void Update(BufferServerDate Date, bool IsNewAdd)
    {
        base.Update(Date, IsNewAdd);
    }

    public override void Exit()
    {
        this.RemoveAllEffect();
    }

    private int relationshipTag;
}
