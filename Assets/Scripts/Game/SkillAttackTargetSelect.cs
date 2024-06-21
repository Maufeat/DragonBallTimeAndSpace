using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class SkillAttackTargetSelect : ISelectTarget
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

    public bool CheckLegalCharactor(CharactorBase charactor, bool ignoredeath = true)
    {
        bool flag = false;
        if (charactor == null)
        {
            return flag;
        }
        return ((ignoredeath || charactor.IsLive) && charactor.CharState == CharactorState.CreatComplete) || flag;
    }

    public bool CheckLegal(CharactorBase charactor, bool ignoredeath = false)
    {
        bool result = false;
        if (!this.CheckLegalCharactor(charactor, ignoredeath))
        {
            return result;
        }
        if (this.SelectType != 0)
        {
            if (!GraphUtils.IsFlag(this.SelectType, 1) || !(charactor is MainPlayer))
            {
                if (!GraphUtils.IsFlag(this.SelectType, 32) || !(charactor is Npc_Pet) || this.entitiesManager.CheckRelationBaseMainPlayer(charactor) != RelationType.Friend)
                {
                    if (!GraphUtils.IsFlag(this.SelectType, 2) || !(charactor is OtherPlayer) || this.entitiesManager.CheckRelationBaseMainPlayer(charactor) != RelationType.Friend)
                    {
                        if (!GraphUtils.IsFlag(this.SelectType, 4) || !(charactor is OtherPlayer) || this.entitiesManager.CheckRelationBaseMainPlayer(charactor) != RelationType.Enemy || !this.selectMgr.CanAttack(charactor))
                        {
                            if (!GraphUtils.IsFlag(this.SelectType, 8) || !(charactor is Npc) || (this.entitiesManager.CheckRelationBaseMainPlayer(charactor) != RelationType.Enemy && this.entitiesManager.CheckRelationBaseMainPlayer(charactor) != RelationType.Neutral) || !this.selectMgr.CanAttack(charactor))
                            {
                                if (!GraphUtils.IsFlag(this.SelectType, 16) || !(charactor is Npc) || this.entitiesManager.CheckRelationBaseMainPlayer(charactor) != RelationType.Friend)
                                {
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    public void SetSelectType(uint Type)
    {
        this.SelectType = (int)Type;
    }

    public void SetTarget(CharactorBase charactor, bool ignoredeath = false, bool switchAutoAttack = true)
    {
        if (this.CheckLegal(charactor, false))
        {
            this.selectMgr.SetTarget(charactor, MainPlayerTargetSelectMgr.SelectType.SkillAttackSelect, 3U, true, true);
            return;
        }
    }

    public void ReqTarget()
    {
        if (!this.CheckLegal(this.selectMgr.TargetCharactor, false))
        {
            this.SetTarget(this.SearchSkillAttackTarget(this.selectMgr.FindEnemyDistence), false, true);
        }
    }

    public CharactorBase SearchSkillAttackTarget(float range)
    {
        float Lasttmptargetdis = 9999f;
        CharactorBase besttarget = null;
        this.entitiesManager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            CharactorBase value = pair.Value;
            if (this.CheckLegal(value, false))
            {
                float num = Vector3.Distance(CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position), CommonTools.DismissYSize(value.ModelObj.transform.position));
                if (num <= range && num < Lasttmptargetdis)
                {
                    besttarget = value;
                    Lasttmptargetdis = num;
                }
            }
        });
        this.entitiesManager.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> pair)
        {
            CharactorBase value = pair.Value;
            if (this.CheckLegal(value, false))
            {
                float num = Vector3.Distance(CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position), CommonTools.DismissYSize(value.ModelObj.transform.position));
                if (num <= range && num < Lasttmptargetdis)
                {
                    besttarget = value;
                    Lasttmptargetdis = num;
                }
            }
        });
        return besttarget;
    }

    private MainPlayerTargetSelectMgr selectMgr;

    private EntitiesManager entitiesManager;

    private int SelectType;
}
