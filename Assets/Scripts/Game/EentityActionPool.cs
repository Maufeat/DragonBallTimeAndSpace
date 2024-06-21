using System;
using System.Collections.Generic;

public class EentityActionPool : IstorebAble
{
    public bool IsDirty { get; set; }

    public void AddAction(Action<CharactorBase> action)
    {
        if (action == null)
        {
            return;
        }
        this.ActionQueue.Enqueue(action);
    }

    public void RunAllAction(CharactorBase chara)
    {
        if (chara == null)
        {
            return;
        }
        while (this.ActionQueue.Count > 0)
        {
            try
            {
                this.ActionQueue.Dequeue()(chara);
            }
            catch (Exception arg)
            {
                FFDebug.LogError(this, this.Eid + " RunAllCacheAction: " + arg);
            }
        }
    }

    public void Dispose()
    {
        this.ActionQueue.Clear();
        this.StoreToPool();
    }

    public void RestThisObject()
    {
        this.ActionQueue.Clear();
        this.Eid = default(EntitiesID);
    }

    public void StoreToPool()
    {
        ClassPool.Store<EentityActionPool>(this, 100);
    }

    public EntitiesID Eid;

    private Queue<Action<CharactorBase>> ActionQueue = new Queue<Action<CharactorBase>>();
}
