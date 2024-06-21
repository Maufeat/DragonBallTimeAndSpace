using System;

public class BufferStateBellQTE : BufferState
{
    public BufferStateBellQTE(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        MainPlayer.Self.noJump = true;
    }

    public override void Exit()
    {
        base.Exit();
        MainPlayer.Self.noJump = false;
    }
}
