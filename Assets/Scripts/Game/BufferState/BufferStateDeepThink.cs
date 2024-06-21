using System;
using Framework.Managers;

public class BufferStateDeepThink : BufferState
{
    public BufferStateDeepThink(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        if (PBControl.Owner == MainPlayer.Self)
        {
            this.m_onetimes = true;
        }
    }

    public override void Update(BufferServerDate newData, bool IsNewAdd)
    {
        base.Update(newData, IsNewAdd);
        if (this.m_onetimes)
        {
            this.m_onetimes = false;
            if (this.lstBuffData != null && this.lstBuffData.Count > 0)
            {
                for (int i = 0; i < this.lstBuffData.Count; i++)
                {
                    this.ShowProgressBar(this.lstBuffData[i].duartion / 1000f);
                }
            }
        }
    }

    private void ShowProgressBar(float length)
    {
        ProgressUIController controller = ControllerManager.Instance.GetController<ProgressUIController>();
        if (controller != null)
        {
            controller.ShowProgressBar(length, SliderDirection.RightToLeft, ProgressUIController.ProgressBarType.Normal, null);
        }
    }

    public override void Exit()
    {
        this.m_onetimes = true;
        base.Exit();
    }

    private bool m_onetimes;
}
