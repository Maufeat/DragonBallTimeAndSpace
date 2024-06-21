using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using UnityEngine;

public class ObjectPoolManager : IManager
{
    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public void OnUpdate()
    {
        this.ObjectPools.BetterForeach(delegate (KeyValuePair<string, IObjectPool> item)
        {
            item.Value.OnUpdate();
        });
    }

    public void Init()
    {
        this.ObjectPoolRoot = GameObject.Find("ObjectPoolRoot").transform;
    }

    public ObjectPool<T> GetObjectPool<T>(string name, bool autoremove = true) where T : ObjectInPoolBase, new()
    {
        if (autoremove)
        {
            if (!this.ObjectPools.ContainsKey(name))
            {
                return null;
            }
            if (this.ObjectPools[name] is ObjectPool<T>)
            {
                return this.ObjectPools[name] as ObjectPool<T>;
            }
        }
        else
        {
            if (!this.ObjectPoolsNotRemove.ContainsKey(name))
            {
                return null;
            }
            if (this.ObjectPoolsNotRemove[name] is ObjectPool<T>)
            {
                return this.ObjectPoolsNotRemove[name] as ObjectPool<T>;
            }
        }
        return null;
    }

    public void RemoveObjectPool(string name, bool autoremove = true)
    {
        if (autoremove)
        {
            if (this.ObjectPools.ContainsKey(name))
            {
                this.ObjectPools[name].Dispose();
                this.ObjectPools.Remove(name);
            }
        }
        else if (this.ObjectPoolsNotRemove.ContainsKey(name))
        {
            this.ObjectPoolsNotRemove[name].Dispose();
            this.ObjectPoolsNotRemove.Remove(name);
        }
    }

    public ObjectPool<T> CreatPool<T>(GameObject prefab, Action<T> onitemenable = null, Action<T> onitemdisable = null, bool autoremove = true, string controlname = "") where T : ObjectInPoolBase, new()
    {
        FFDebug.Log(this, FFLogType.Default, "CreatPool with prefab " + prefab);
        if (prefab.scene.isLoaded)
        {
            prefab.transform.SetParent(this.ObjectPoolRoot);
        }
        ObjectPool<T> objectPool = new ObjectPool<T>(prefab, controlname, onitemenable, onitemdisable);
        if (autoremove)
        {
            this.ObjectPools[objectPool.PoolName] = objectPool;
        }
        else
        {
            this.ObjectPoolsNotRemove[objectPool.PoolName] = objectPool;
        }
        return objectPool;
    }

    public void CloneCharactorCorpse(CharactorBase charactor)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(charactor.ModelObj);
        GlobalRegister.RemoveEffectLayerObject(gameObject);
        gameObject.transform.SetParent(this.ObjectPoolRoot);
        int selectindex = (!charactor.IsFly) ? 0 : 1;
        FFActionClip ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(charactor.animatorControllerName, 1000U, selectindex);
        Animator component = gameObject.GetComponent<Animator>();
        if (component != null && ffactionClip != null)
        {
            component.CrossFade(ffactionClip.ClipName, 0.1f);
        }
        if (this.ObjectCorpsePools.ContainsKey(charactor.EID.Id))
        {
            UnityEngine.Object.Destroy(this.ObjectCorpsePools[charactor.EID.Id]);
            this.ObjectCorpsePools[charactor.EID.Id] = gameObject;
        }
        else
        {
            this.ObjectCorpsePools.Add(charactor.EID.Id, gameObject);
        }
    }

    public void ClearCorpse()
    {
        this.ObjectCorpsePools.BetterForeach(delegate (KeyValuePair<ulong, GameObject> Item)
        {
            UnityEngine.Object.Destroy(Item.Value);
        });
        this.ObjectCorpsePools.Clear();
    }

    public void RemoveAllObjectPool()
    {
        this.ObjectPools.BetterForeach(delegate (KeyValuePair<string, IObjectPool> Item)
        {
            Item.Value.Dispose();
        });
        this.ObjectPools.Clear();
        this.ClearCorpse();
    }

    public void RemoveAllNotAutoRemoveObjectPool()
    {
        this.ObjectPoolsNotRemove.BetterForeach(delegate (KeyValuePair<string, IObjectPool> Item)
        {
            Item.Value.Dispose();
        });
        this.ObjectPoolsNotRemove.Clear();
    }

    public void OnReSet()
    {
        this.RemoveAllObjectPool();
    }

    public Transform ObjectPoolRoot;

    public BetterDictionary<string, IObjectPool> ObjectPools = new BetterDictionary<string, IObjectPool>();

    public BetterDictionary<string, IObjectPool> ObjectPoolsNotRemove = new BetterDictionary<string, IObjectPool>();

    public BetterDictionary<ulong, GameObject> ObjectCorpsePools = new BetterDictionary<ulong, GameObject>();
}
