using System;

public class BufferStateDoorOpen : BufferState
{
    public BufferStateDoorOpen(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
