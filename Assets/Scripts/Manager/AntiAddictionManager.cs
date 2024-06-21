using System;
using Framework.Base;
using Framework.Managers;

public class AntiAddictionManager : IManager
{
    public string ManagerName
    {
        get
        {
            return "antiaddiction_manager";
        }
    }

    public void Awake()
    {
        Scheduler.Instance.AddTimer(60f, true, new Scheduler.OnScheduler(this.PlayWarn));
    }

    private void PlayWarn()
    {
        if (DateTime.Now.Hour != this.oldHour)
        {
            this.oldHour = DateTime.Now.Hour;
            uint num = 0U;
            if (DateTime.Now.Minute < 3)
            {
                int hour = DateTime.Now.Hour;
                switch (hour)
                {
                    case 17:
                        num = 4010U;
                        break;
                    default:
                        if (hour != 12)
                        {
                            if (hour == 23)
                            {
                                num = 4012U;
                            }
                        }
                        else
                        {
                            num = 4009U;
                        }
                        break;
                    case 20:
                        num = 4011U;
                        break;
                }
            }
            if (num != 0U)
            {
                TipsWindow.ShowWindow(num);
            }
        }
    }

    public void NotifyAntiAddictCb(bool isInAntiAddictState, int lastsecond, bool isNotSecondHourSend)
    {
        if (!isInAntiAddictState)
        {
            return;
        }
        int hour = lastsecond / 3600;
        if (isInAntiAddictState && isNotSecondHourSend)
        {
            hour = -1;
        }
        ControllerManager.Instance.GetController<AntiAddictionController>().OpenUI(hour);
    }

    public void OnReSet()
    {
    }

    public void OnUpdate()
    {
    }

    private int oldHour;

    public bool mIsInAntiAddictState = true;
}
