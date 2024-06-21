using System;
using Framework.Managers;
using Team;

public class UITargetSelect : ISelectTarget
{
    public void Init()
    {
        this.selectMgr = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        this.entitiesManager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        SingletonForMono<InputController>.Instance.mScreenEventController.AddListener(new ScreenEventController.OnScreenEvent(this.OnClickEvent));
    }

    public void Dispose()
    {
        this.selectMgr = null;
        this.entitiesManager = null;
        SingletonForMono<InputController>.Instance.mScreenEventController.RemoveListener(new ScreenEventController.OnScreenEvent(this.OnClickEvent));
    }

    public bool CheckLegal(CharactorBase charactor, bool ignoredeath = false)
    {
        bool flag = false;
        if (charactor == null)
        {
            return flag;
        }
        return ((ignoredeath || charactor.IsLive) && charactor.CharState == CharactorState.CreatComplete) || flag;
    }

    public void SetTarget(CharactorBase charactor, bool ignoredeath = false, bool switchAutoAttack = true)
    {
        if (!this.CheckLegal(charactor, ignoredeath))
        {
            return;
        }
        this.selectMgr.SetTarget(charactor, MainPlayerTargetSelectMgr.SelectType.UISelect, 1U, true, true);
    }

    public void SetTargetNotInRange(Memember memember)
    {
        MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().SetTargetNull();
        EntitiesID eid = new EntitiesID(ulong.Parse(memember.mememberid), CharactorType.Player);
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.ShowTargetInfo(eid, true, memember);
        }
    }

    public void OnClickEvent(ScreenEvent SE)
    {
        if (SE.mTpye != ScreenEvent.EventType.Click && SE.mTpye != ScreenEvent.EventType.Select)
        {
            return;
        }
    }

    public void ReqTarget()
    {
    }

    private void OnTimerReducePriority()
    {
        this.selectMgr.CurrentSelectPriority = 2U;
    }

    public const float ManualSelectPriorityDelay = 2f;

    private MainPlayerTargetSelectMgr selectMgr;

    private EntitiesManager entitiesManager;
}
