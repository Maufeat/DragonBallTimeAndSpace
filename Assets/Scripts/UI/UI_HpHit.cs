using System;
using Framework.Managers;
using magic;
using UnityEngine;

public class UI_HpHit : UIPanelBase
{
    private const string ContentPath = "Content";
    private const string JumpNumberPath = "Content/jumpNumber";
    private const string HitPath = "hit";
    private const string RevertPath = "revert";
    private const string ExpPath = "exp";

    public GameObject mRoot;
    private ObjectPool<NormalObjectInPool> modePool;
    private GameObject hitValue;
    private Transform hpHitRoot;

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        mRoot = root.gameObject;
        mRoot.SetLayer(Const.Layer.Charactor, true);
        hpHitRoot = root.Find(ContentPath);
        hitValue = root.Find(JumpNumberPath).gameObject;

        var poolManager = ManagerCenter.Instance.GetManager<ObjectPoolManager>();
        modePool = poolManager.GetObjectPool<NormalObjectInPool>("hphit", false)
                   ?? poolManager.CreatPool<NormalObjectInPool>(hitValue, null, null, false, string.Empty);
    }

    private Hpchange Reset(NormalObjectInPool obj, Transform target)
    {
        GameObject itemObj = obj.ItemObj;
        itemObj.transform.SetParent(hpHitRoot);
        itemObj.transform.Reset();
        itemObj.transform.position = target.position;
        SetActiveState(itemObj, HitPath, false);
        SetActiveState(itemObj, RevertPath, false);
        SetActiveState(itemObj, ExpPath, false);
        itemObj.SetActive(true);

        var hpchange = itemObj.GetComponent<Hpchange>() ?? itemObj.AddComponent<Hpchange>();
        hpchange.Init(obj);
        return hpchange;
    }

    private void SetActiveState(GameObject itemObj, string path, bool state)
    {
        itemObj.transform.Find(path).gameObject.SetActive(state);
    }

    public override void OnDispose()
    {
        var poolManager = ManagerCenter.Instance.GetManager<ObjectPoolManager>();
        poolManager.RemoveObjectPool(modePool.PoolName, false);
        base.OnDispose();
        UnInit();

        if (mRoot != null)
        {
            UnityEngine.Object.Destroy(mRoot.gameObject);
            mRoot = null;
        }
    }

    public void UnInit()
    {
        // No implementation needed in current context
    }

    public void SetHit(EntitiesID Owner, OneSkillHitResult HitResult, bool AttIsMainPlayer, Transform target)
    {
        Action<Hpchange.EHarmType> action = type =>
        {
            modePool.GetItemFromPool(obj =>
            {
                var hpchange = Reset(obj, target);
                var aType = ATTACKRESULT.ATTACKRESULT_NORMAL;

                foreach (var attCode in HitResult.AttcodeList)
                {
                    if (IsSpecialAttack(attCode))
                    {
                        aType = attCode;
                        hpchange.SetViewType(attCode);
                    }
                }

                hpchange.HitValue(type, aType, HitResult.HpChange);
            });
        };

        var entitiesManager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        var charactorByID = entitiesManager.GetCharactorByID(Owner);
        var charactorByID2 = entitiesManager.GetCharactorByID(HitResult.Att);

        var npcOwnID = GetNpcOwnID(charactorByID2 as Npc);
        var npcOwnID2 = GetNpcOwnID(charactorByID as Npc);

        if (charactorByID2 is Npc_Pet && npcOwnID.Equals(MainPlayer.Self.EID))
        {
            action(Hpchange.EHarmType.SummonAtt);
        }
        else if (charactorByID is MainPlayer || charactorByID is Npc_Pet || npcOwnID2.Equals(MainPlayer.Self.EID))
        {
            action(Hpchange.EHarmType.PlayerBeAtt);
        }
        else if (charactorByID2 is MainPlayer || npcOwnID.Equals(MainPlayer.Self.EID))
        {
            action(Hpchange.EHarmType.PlayerAtt);
        }
    }

    private bool IsSpecialAttack(ATTACKRESULT attCode)
    {
        switch (attCode)
        {
            case ATTACKRESULT.ATTACKRESULT_MISS:
            case ATTACKRESULT.ATTACKRESULT_BANG:
            case ATTACKRESULT.ATTACKRESULT_HOLD:
            case ATTACKRESULT.ATTACKRESULT_BLOCK:
            case ATTACKRESULT.ATTACKRESULT_DEFLECT:
                return true;
            default:
                return false;
        }
    }

    public void SetRevert(EntitiesID Owner, float Change, Transform target)
    {
        modePool.GetItemFromPool(obj =>
        {
            var hpchange = Reset(obj, target);
            hpchange.SetViewType(ATTACKRESULT.ATTACKRESULT_NORMAL);
            hpchange.SetRevertValue((int)Mathf.Abs(Change));
        });
    }

    public void SetExp(float Change, Transform target)
    {
        modePool.GetItemFromPool(obj =>
        {
            var hpchange = Reset(obj, target);
            hpchange.SetExp((int)Change);
        });
    }

    private EntitiesID GetNpcOwnID(Npc npc)
    {
        if (npc != null && npc.NpcData != null && npc.NpcData.MapNpcData != null && npc.NpcData.MapNpcData.MasterData != null)
        {
            return npc.NpcData.MapNpcData.MasterData.Eid;
        }
        return default(EntitiesID);
    }
}
