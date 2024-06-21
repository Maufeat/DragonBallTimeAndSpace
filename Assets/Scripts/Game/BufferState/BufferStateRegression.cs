using System;

public class BufferStateRegression : BufferState
{
    public BufferStateRegression(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        if (this.BufferControl.Owner == MainPlayer.Self)
        {
            UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
            if (null != uiobject)
            {
                uiobject.ShowRegressionButton(true);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (this.BufferControl.Owner == MainPlayer.Self)
        {
            UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
            if (null != uiobject)
            {
                uiobject.ShowRegressionButton(false);
            }
        }
    }
}
