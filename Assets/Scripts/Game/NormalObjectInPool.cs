using System;
using Framework.Managers;

public class NormalObjectInPool : ObjectInPoolBase
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
        if (this.ItemObj)
        {
            this.ItemObj.SetActive(false);
        }
    }

    public override bool DisableAndBackToPool(bool autoremove = true)
    {
        base.DisableAndBackToPool(autoremove);
        ObjectPool<NormalObjectInPool> objectPool = ManagerCenter.Instance.GetManager<ObjectPoolManager>().GetObjectPool<NormalObjectInPool>(this.PoolName, autoremove);
        if (objectPool != null)
        {
            objectPool.MakeItemBackToPool(this);
            return true;
        }
        return false;
    }
}
