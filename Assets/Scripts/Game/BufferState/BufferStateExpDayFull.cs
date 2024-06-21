using System;
using Framework.Managers;

public class BufferStateExpDayFull : BufferState
{
    public BufferStateExpDayFull(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        ControllerManager.Instance.GetController<MainUIController>().AddMessage(MessageType.ExpFull, 1, delegate
        {
            TipsWindow.ShowWindow(TipsType.EXP_DAY_FULL, null);
        });
    }

    public override void Exit()
    {
        base.Exit();
        ControllerManager.Instance.GetController<MainUIController>().ReadMessage(MessageType.ExpFull);
    }
}
