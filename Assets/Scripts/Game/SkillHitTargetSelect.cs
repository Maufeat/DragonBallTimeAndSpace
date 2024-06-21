using System;
using Framework.Managers;

public class SkillHitTargetSelect : ISelectTarget
{
    public void Init()
    {
        this.selectMgr = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        this.entitiesManager = ManagerCenter.Instance.GetManager<EntitiesManager>();
    }

    public void Dispose()
    {
        this.selectMgr = null;
        this.entitiesManager = null;
    }

    public bool CheckLegal(CharactorBase charactor, bool ignoredeath = false)
    {
        bool result = false;
        if (charactor == null)
        {
            return result;
        }
        if (charactor is MainPlayer)
        {
            return result;
        }
        if (!charactor.IsLive || charactor.CharState != CharactorState.CreatComplete)
        {
            return result;
        }
        bool flag = this.selectMgr.CanAttack(charactor);
        if (flag)
        {
            result = true;
        }
        return result;
    }

    public void SetTarget(CharactorBase charactor, bool ignoredeath = false, bool switchAutoAttack = true)
    {
        if (!this.CheckLegal(charactor, false))
        {
            return;
        }
        CharactorBase targetCharactor = this.selectMgr.TargetCharactor;
        if (targetCharactor != null)
        {
            return;
        }
        this.selectMgr.SetTarget(charactor, MainPlayerTargetSelectMgr.SelectType.BeUnderAttack, 2U, true, true);
    }

    public void ReqTarget()
    {
    }

    public void SkillHitSetTarget(CharactorBase[] charactors)
    {
        if (charactors != null)
        {
            foreach (CharactorBase charactor in charactors)
            {
                if (this.CheckLegal(charactor, false))
                {
                    this.SetTarget(charactor, false, true);
                    break;
                }
            }
        }
    }

    private MainPlayerTargetSelectMgr selectMgr;

    private EntitiesManager entitiesManager;
}
