using System;
using System.Collections.Generic;
using Framework.Managers;
using msg;
using UnityEngine;

public class AttactFollowTeamLeaderTargetSelect : ISelectTarget
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

    public void RetSetTarget(EntryIDType entruID)
    {
        CharactorBase target = null;
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        manager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            if (pair.Value.EID.Equals(entruID))
            {
                target = pair.Value;
            }
        });
        if (target == null)
        {
            return;
        }
        this.SetTarget(target, false, true);
    }

    public void ReqTarget()
    {
        ControllerManager.Instance.GetController<TeamController>().MSG_ReqLeaderAttackTarget_CS();
    }

    private MainPlayerTargetSelectMgr selectMgr;

    private EntitiesManager entitiesManager;

    private float Lasttmptargetdis = 999f;
}
