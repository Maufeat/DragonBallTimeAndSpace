using System;
using Framework.Managers;

public class BufferStateDogWatched : BufferState
{
    public BufferStateDogWatched(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        ControllerManager.Instance.GetController<AlertController>().ShowAlert(1);
    }

    public override void Exit()
    {
        base.Exit();
        ControllerManager.Instance.GetController<AlertController>().CloseAlert(1);
    }
}
