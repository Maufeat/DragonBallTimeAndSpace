using System;

public class BufferStateTurtleShell : BufferStateFlyFlower
{
    public BufferStateTurtleShell(UserState Flag) : base(Flag)
    {
        this.hangPoint = "top";
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
