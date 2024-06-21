using System;
using Framework.Managers;

public class BufferStatePeace : BufferState
{
    public BufferStatePeace(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        this.RefreshHPState();
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.refreshSkillpanelActive();
        }
    }

    public override void Exit()
    {
        base.Exit();
        this.RefreshHPState();
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        mainView.refreshSkillpanelActive();
    }

    private void RefreshHPState()
    {
        if (this.BufferControl.Owner is OtherPlayer)
        {
            OtherPlayer otherPlayer = this.BufferControl.Owner as OtherPlayer;
            if (otherPlayer.hpdata != null)
            {
                otherPlayer.hpdata.RefreshModel();
            }
        }
    }
}
