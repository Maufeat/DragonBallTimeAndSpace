using System;
using Framework.Managers;
using Models;

public class AntiAddictionController : ControllerBase
{
    private UI_AntiAddiction mUIAntiAddiction
    {
        get
        {
            return UIManager.GetUIObject<UI_AntiAddiction>();
        }
    }

    public override void Awake()
    {
        this.mNetWorker = new AntiAddictionNetWorker();
        this.mNetWorker.Initialize();
        base.Awake();
    }

    public void OpenUI(int hour)
    {
        if (ManagerCenter.Instance.GetManager<EntitiesManager>().wegame)
        {
            return;
        }
        UIManager.Instance.ShowUI<UI_AntiAddiction>("UI_fcm", delegate ()
        {
            this.mUIAntiAddiction.SetupPanel(hour);
        }, UIManager.ParentType.CommonUI, false);
    }

    public void CloseUI()
    {
        UIManager.Instance.DeleteUI<UI_AntiAddiction>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override string ControllerName
    {
        get
        {
            return "antiaddiction_controller";
        }
    }

    private AntiAddictionNetWorker mNetWorker;
}
