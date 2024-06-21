using System;
using Framework.Managers;
using UnityEngine;

public class ModleObjInPool : ObjectInPoolBase
{
    public override void Enable()
    {
        base.Enable();
        this.ItemState = PoolItemState.Active;
        this.lastDisableTime = this.destroyDelayTime;
        this.ItemObj.SetActive(true);
    }

    public override void Disable()
    {
        base.Disable();
        this.ItemState = PoolItemState.DisActive;
        this.lastDisableTime = this.destroyDelayTime;
        if (this.ItemObj)
        {
            this.ItemObj.SetActive(false);
        }
    }

    public override bool DisableAndBackToPool(bool autoremove = true)
    {
        if (this.ItemObj == null)
        {
            return false;
        }
        base.DisableAndBackToPool(autoremove);
        ObjectPool<ModleObjInPool> objectPool = ManagerCenter.Instance.GetManager<ObjectPoolManager>().GetObjectPool<ModleObjInPool>(this.PoolName, autoremove);
        if (objectPool != null)
        {
            DynamicBone[] componentsInChildren = this.ItemObj.GetComponentsInChildren<DynamicBone>();
            if (componentsInChildren != null)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].enabled = false;
                }
            }
            objectPool.MakeItemBackToPool(this);
            return true;
        }
        UnityEngine.Object.Destroy(this.ItemObj);
        this.ItemObj = null;
        return false;
    }
}
