using System;

public class BufferStateBeGather : BufferState
{
    public BufferStateBeGather(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        if (this.BufferControl.Owner.hpdata != null)
        {
            this.BufferControl.Owner.hpdata.ActiveName();
        }
        else
        {
            FFDebug.Log(this, FFLogType.Buff, "   this.BufferControl.Owner.hpdata==null ");
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (this.BufferControl.Owner.hpdata != null)
        {
            this.BufferControl.Owner.hpdata.DisActiveName();
        }
    }
}
