using System;
using Framework.Managers;
using UnityEngine;

public class EffectObjInPool : ObjectInPoolBase
{
    public override void Enable()
    {
        base.Enable();
        this.ItemState = PoolItemState.Active;
        this.lastDisableTime = this.destroyDelayTime;
    }

    public override void Disable()
    {
        base.Disable();
        this.ItemState = PoolItemState.DisActive;
        this.lastDisableTime = this.destroyDelayTime;
        if (this.ItemObj != null)
        {
            this.ItemObj.transform.position = EffectObjInPool.PoolBackPos;
            this.ItemObj.SetActive(false);
        }
    }

    public override bool DisableAndBackToPool(bool autoremove = true)
    {
        base.DisableAndBackToPool(autoremove);
        ObjectPool<EffectObjInPool> objectPool = ManagerCenter.Instance.GetManager<ObjectPoolManager>().GetObjectPool<EffectObjInPool>(this.PoolName, true);
        if (objectPool != null)
        {
            objectPool.MakeItemBackToPool(this);
            return true;
        }
        return false;
    }

    private static Vector3 PoolBackPos = new Vector3(-9999f, -9999f, -9999f);
}
