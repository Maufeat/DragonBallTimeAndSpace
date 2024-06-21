using System;
using UnityEngine;

public class ObjectInPoolBase
{
    public virtual void Init(GameObject itemprefab, string poolname, string controlname)
    {
        this.PoolName = string.Empty;
        this.AniControlName = controlname;
        if (itemprefab != null)
        {
            this.ItemObj = (UnityEngine.Object.Instantiate(itemprefab, Vector3.zero, Quaternion.identity) as GameObject);
            this.ItemObj.transform.position = new Vector3(-99999f, -99999f, -99999f);
            this.PoolName = poolname;
        }
    }

    public virtual void Enable()
    {
        if (!this.CheckSelf())
        {
            return;
        }
    }

    public virtual void Disable()
    {
        if (!this.CheckSelf())
        {
            return;
        }
    }

    public virtual bool CheckSelf()
    {
        if (this.ItemObj == null)
        {
            this.DestroyThis();
            return false;
        }
        return true;
    }

    public virtual void DestroyThis()
    {
        if (this.ItemObj != null)
        {
            UnityEngine.Object.DestroyImmediate(this.ItemObj);
            this.ItemObj = null;
        }
        this.ItemState = PoolItemState.Deleted;
    }

    public virtual void OnUpdate()
    {
        if (this.ItemState == PoolItemState.DisActive)
        {
            if (this.lastDisableTime < 0f)
            {
                this.ItemState = PoolItemState.MarkAsDeleted;
                return;
            }
            this.lastDisableTime -= Scheduler.Instance.realDeltaTime;
        }
    }

    public virtual bool DisableAndBackToPool(bool autoremove = true)
    {
        return true;
    }

    public GameObject ItemObj;

    public PoolItemState ItemState;

    protected float destroyDelayTime = 60f;

    protected float lastDisableTime;

    protected string PoolName = string.Empty;

    protected string AniControlName = string.Empty;
}
