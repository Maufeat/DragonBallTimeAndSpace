using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class AutoAttackTargetSelect : ISelectTarget
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
        if (!this.CheckLegal(charactor, false))
        {
            return;
        }
        CharactorBase tempTarget = this.selectMgr.GetTempTarget();
        if (this.CheckLegal(tempTarget, false))
        {
            return;
        }
        if (this.selectMgr.CurrentSelectPriority != 1U)
        {
            this.selectMgr.SetTarget(charactor, MainPlayerTargetSelectMgr.SelectType.AutoAttackSelect, 3U, true, true);
        }
    }

    public CharactorBase SearchAutoAttackTarget()
    {
        float Lasttmptargetdis = 9999f;
        CharactorBase besttarget = null;
        this.entitiesManager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            CharactorBase value = pair.Value;
            if (value is Npc && this.CheckLegal(value, false))
            {
                float num = Vector3.Distance(CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position), CommonTools.DismissYSize(value.ModelObj.transform.position));
                if (num <= this.selectMgr.FindEnemyDistence && num < Lasttmptargetdis)
                {
                    besttarget = value;
                    Lasttmptargetdis = num;
                }
            }
        });
        return besttarget;
    }

    public void ReqTarget()
    {
        if (!this.CheckLegal(this.selectMgr.TargetCharactor, false))
        {
            this.SetTarget(this.SearchAutoAttackTarget(), false, true);
        }
    }

    private MainPlayerTargetSelectMgr selectMgr;

    private EntitiesManager entitiesManager;
}
