using System;

public class BufferStateRun : BufferState
{
    public BufferStateRun(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        FFBehaviourControl component = this.BufferControl.Owner.GetComponent<FFBehaviourControl>();
        component.EnterRun();
    }

    public override void Exit()
    {
        base.Exit();
        FFBehaviourControl component = this.BufferControl.Owner.GetComponent<FFBehaviourControl>();
        component.ExitRun();
    }
}
