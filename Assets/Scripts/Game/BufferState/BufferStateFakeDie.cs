using System;

public class BufferStateFakeDie : BufferState
{
    public BufferStateFakeDie(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        if (this.BufferControl.Owner != null && this.BufferControl.Owner.hpdata != null)
        {
            this.BufferControl.Owner.hpdata.RefreshModel();
            this.BufferControl.Owner.hpdata.SetHPActive(false);
        }
        if (this.BufferControl.Owner != MainPlayer.Self)
        {
            MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            if (component.TargetCharactor == this.BufferControl.Owner)
            {
                component.SetTargetNull();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (this.BufferControl.Owner != null && this.BufferControl.Owner.hpdata != null)
        {
            this.BufferControl.Owner.hpdata.RefreshModel();
            this.BufferControl.Owner.hpdata.SetHPActive(true);
        }
    }
}
