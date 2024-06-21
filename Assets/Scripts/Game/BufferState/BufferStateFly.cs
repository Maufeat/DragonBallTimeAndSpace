using System;

public class BufferStateFly : BufferState
{
    public BufferStateFly(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        this.BufferControl.Owner.RefreshPhysicsPos();
        FFBehaviourControl component = this.BufferControl.Owner.GetComponent<FFBehaviourControl>();
        if (component != null)
        {
            FFBehaviourState_Idle ffbehaviourState_Idle = component.CurrStatebyType<FFBehaviourState_Idle>();
            if (ffbehaviourState_Idle != null)
            {
                ffbehaviourState_Idle.playIdle(false);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        this.BufferControl.Owner.RefreshPhysicsPos();
        FFBehaviourControl component = this.BufferControl.Owner.GetComponent<FFBehaviourControl>();
        if (component != null)
        {
            FFBehaviourState_Idle ffbehaviourState_Idle = component.CurrStatebyType<FFBehaviourState_Idle>();
            if (ffbehaviourState_Idle != null)
            {
                ffbehaviourState_Idle.playIdle(false);
            }
        }
    }
}
