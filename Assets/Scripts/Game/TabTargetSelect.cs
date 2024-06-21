using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class TabTargetSelect : ISelectTarget
{
    public void Init()
    {
        this.selectMgr = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        this.entitiesManager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        this.TabSearchEnemyDistance = LuaConfigManager.GetXmlConfigTable("targetSelectConfig").GetCacheField_Table("TabSeachEnemyDistence").GetCacheField_Float("value");
        this.tabDistaceComparer = new CompareCharsInTabDistace();
        this.charsInTabDistace = new List<CharactorBase>();
    }

    public void Dispose()
    {
        this.selectMgr = null;
        this.entitiesManager = null;
        this.charsInTabDistace.Clear();
        this.tabDistaceComparer = null;
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
        this.selectMgr.SetTarget(charactor, MainPlayerTargetSelectMgr.SelectType.ManualSelect, 1U, true, true);
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.OnTimerReducePriority));
        Scheduler.Instance.AddTimer(2f, false, new Scheduler.OnScheduler(this.OnTimerReducePriority));
    }

    public void SetFirstTargetSearch()
    {
        this.FirstTargetSearch = true;
    }

    private void OnTimerReducePriority()
    {
        this.selectMgr.CurrentSelectPriority = 2U;
    }

    public void ReqTarget()
    {
        this.SetTarget(this.SearcTargetInTabDistace(), false, true);
    }

    public CharactorBase SearcTargetInTabDistace()
    {
        float Lasttmptargetdis = 9999f;
        CharactorBase result = null;
        this.charsInTabDistace.Clear();
        this.entitiesManager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            CharactorBase value = pair.Value;
            if (this.CheckLegal(value, false))
            {
                float num2 = Vector3.Distance(CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position), CommonTools.DismissYSize(value.ModelObj.transform.position));
                if (num2 <= this.TabSearchEnemyDistance)
                {
                    this.charsInTabDistace.Add(value);
                }
            }
        });
        this.entitiesManager.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> pair)
        {
            CharactorBase value = pair.Value;
            if (this.CheckLegal(value, false))
            {
                float num2 = Vector3.Distance(CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position), CommonTools.DismissYSize(value.ModelObj.transform.position));
                if (num2 <= this.TabSearchEnemyDistance && num2 < Lasttmptargetdis)
                {
                    this.charsInTabDistace.Add(value);
                }
            }
        });
        if (this.charsInTabDistace.Count > 0)
        {
            this.charsInTabDistace.Sort(this.tabDistaceComparer);
            CharactorBase tempTarget = this.selectMgr.GetTempTarget();
            if (this.FirstTargetSearch)
            {
                this.FirstTargetSearch = false;
                if (tempTarget == this.charsInTabDistace[0] && this.charsInTabDistace.Count > 1)
                {
                    return this.charsInTabDistace[1];
                }
                return this.charsInTabDistace[0];
            }
            else if (tempTarget == null || !this.charsInTabDistace.Contains(tempTarget))
            {
                result = this.charsInTabDistace[0];
            }
            else
            {
                int num = -1;
                for (int i = 0; i < this.charsInTabDistace.Count; i++)
                {
                    if (this.charsInTabDistace[i] == tempTarget)
                    {
                        num = i;
                        break;
                    }
                }
                if (num == this.charsInTabDistace.Count - 1)
                {
                    result = this.charsInTabDistace[0];
                }
                else if (this.charsInTabDistace.Count > num + 1)
                {
                    result = this.charsInTabDistace[num + 1];
                }
            }
        }
        return result;
    }

    public const float ManualSelectPriorityDelay = 2f;

    private MainPlayerTargetSelectMgr selectMgr;

    private EntitiesManager entitiesManager;

    private bool FirstTargetSearch = true;

    private float TabSearchEnemyDistance = 9999f;

    private CompareCharsInTabDistace tabDistaceComparer;

    private List<CharactorBase> charsInTabDistace;
}
