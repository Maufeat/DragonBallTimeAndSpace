using System;
using Framework.Managers;

public class BeAttackTargetSelect : ISelectTarget
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
        if (charactor is Npc_Pet)
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
        if (charactor == null)
        {
            return;
        }
        if (charactor.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        if (charactor is Npc_Pet)
        {
            Npc_Pet npc_Pet = charactor as Npc_Pet;
            if (npc_Pet.NpcData.MapNpcData != null && npc_Pet.NpcData.MapNpcData.MasterData != null)
            {
                charactor = this.entitiesManager.GetCharactorByID(npc_Pet.NpcData.MapNpcData.MasterData.Eid);
                if (charactor == null)
                {
                    return;
                }
                if (charactor is MainPlayer)
                {
                    return;
                }
                if (!charactor.IsLive || charactor.CharState != CharactorState.CreatComplete)
                {
                    return;
                }
            }
        }
        if (!this.CheckLegal(charactor, false))
        {
            return;
        }
        CharactorBase tempTarget = this.selectMgr.GetTempTarget();
        if (!this.CheckLegal(tempTarget, false))
        {
            this.selectMgr.SetTarget(charactor, MainPlayerTargetSelectMgr.SelectType.BeUnderAttack, 2U, true, true);
        }
        else if (!this.selectMgr.CanAttack(tempTarget))
        {
            if (this.selectMgr.CurrentSelectPriority != 1U)
            {
                this.selectMgr.SetTarget(charactor, MainPlayerTargetSelectMgr.SelectType.BeUnderAttack, 2U, true, true);
            }
        }
        else if (charactor is OtherPlayer && !(tempTarget is OtherPlayer) && this.selectMgr.CurrentSelectPriority != 1U)
        {
            this.selectMgr.SetTarget(charactor, MainPlayerTargetSelectMgr.SelectType.BeUnderAttack, 2U, true, true);
        }
    }

    public void ReqTarget()
    {
    }

    public void BeAttackSetTarget(CharactorBase charactor)
    {
        this.SetTarget(charactor, false, true);
    }

    private MainPlayerTargetSelectMgr selectMgr;

    private EntitiesManager entitiesManager;
}
