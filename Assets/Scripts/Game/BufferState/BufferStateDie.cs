using System;
using Framework.Managers;

public class BufferStateDie : BufferState
{
    public BufferStateDie(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        ManagerCenter.Instance.GetManager<CopyManager>().CheckPlayCopyBossDieCameraMove(this.BufferControl.Owner);
    }
}
