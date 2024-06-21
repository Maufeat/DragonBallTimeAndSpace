using System;
using Framework.Managers;
using Models;

public class ImgMaskController : ControllerBase
{
    private UIManager UImanager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<UIManager>();
        }
    }

    public void ViewMask(float time)
    {
        float time2 = time / 1000f;
        this.UImanager.SetMaskImageAlpha(0f);
        this.UImanager.SetMaskImage(true);
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.ViewMaskCallBack));
        Scheduler.Instance.AddTimer(time2, false, new Scheduler.OnScheduler(this.ViewMaskCallBack));
    }

    private void ViewMaskCallBack()
    {
        this.UImanager.SetMaskImage(false);
        this.UImanager.SetMaskImageAlpha(1f);
    }

    public override void Awake()
    {
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "imagemask_controller";
        }
    }
}
