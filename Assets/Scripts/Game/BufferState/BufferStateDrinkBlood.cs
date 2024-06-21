using System;

public class BufferStateDrinkBlood : BufferState
{
    public BufferStateDrinkBlood(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        if (this.BufferControl.Owner.hpdata != null)
        {
            this.BufferControl.Owner.hpdata.SetActiveKilledBtn(true);
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
            this.BufferControl.Owner.hpdata.SetActiveKilledBtn(false);
        }
    }
}
