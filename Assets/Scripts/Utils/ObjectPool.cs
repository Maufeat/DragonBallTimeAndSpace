using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public interface IObjectPool
{
    void OnUpdate();
    void Dispose();
}

public enum LoadType
{
    Resources,
    Assetbundle
}

public enum PoolItemState
{
    Active,
    DisActive,
    MarkAsDeleted,
    Deleted
}

public class ObjectPool<T> : IObjectPool where T : ObjectInPoolBase, new()
{
    private GameObject poolPrefab;
    private Queue<T> objQueue = new Queue<T>();
    private bool isInitComplete;
    private Queue<Action> GetItemActions = new Queue<Action>();
    private Action<T> onItemEnable;
    private Action<T> onItemDisable;
    private Action onDisable;
    private string poolName = string.Empty;
    private string aniControlName = string.Empty;
    private int deleteInterval = 3;
    private int deletePerFrame = 5;
    private int frameFlag;
    private Queue<T> deletedQueue = new Queue<T>();

    public ObjectPool(string prefabPath, LoadType type, Action<T> onItemEnable, Action<T> onItemDisable)
    {
        poolName = prefabPath;
        this.onItemEnable = onItemEnable;
        this.onItemDisable = onItemDisable;
        GetItemActions.Clear();
        LoadPrefab(prefabPath, type);
    }

    public ObjectPool(GameObject prefab, string controlName, Action<T> onItemEnable, Action<T> onItemDisable)
    {
        aniControlName = controlName;
        poolName = prefab.name;
        prefab.SetActive(false);
        prefab.transform.position = new Vector3(-99999f, -99999f, -99999f);
        this.onItemEnable = onItemEnable;
        this.onItemDisable = onItemDisable;
        GetItemActions.Clear();
        poolPrefab = prefab;
        isInitComplete = true;
    }

    public string PoolName
    {
        get { return poolName; }
        set { poolName = value; }
    }

    public string AniControlName
    {
        get { return aniControlName; }
        set { aniControlName = value; }
    }

    private ObjectPoolManager PoolManager
    {
        get { return ManagerCenter.Instance.GetManager<ObjectPoolManager>(); }
    }

    private void LoadPrefab(string path, LoadType type)
    {
        if (type == LoadType.Resources)
        {
            poolPrefab = Resources.Load<GameObject>(path);
            isInitComplete = true;
        }
    }

    public void GetItemFromPool(Action<T> callback)
    {
        T item = default(T);
        while (objQueue.Count > 0)
        {
            item = objQueue.Dequeue();
            if (item.ItemState != PoolItemState.Deleted && item.ItemObj != null)
            {
                item.Enable();
                if (onItemEnable != null)
                {
                    onItemEnable(item);
                }
                item.ItemObj.SetActive(true);
                callback(item);
                return;
            }
        }

        if (objQueue.Count == 0)
        {
            item = Activator.CreateInstance<T>();
            item.Init(poolPrefab, poolName, aniControlName);
            item.Enable();
            if (item.ItemObj != null)
            {
                item.ItemObj.transform.SetParent(PoolManager.ObjectPoolRoot);
            }
            if (onItemEnable != null)
            {
                onItemEnable(item);
            }
            item.ItemObj.SetActive(true);
            callback(item);
        }
    }

    public T GetItemFromPool()
    {
        T item = default(T);
        while (objQueue.Count > 0)
        {
            item = objQueue.Dequeue();
            if (item.ItemState != PoolItemState.Deleted && item.ItemObj != null)
            {
                item.Enable();
                if (onItemEnable != null)
                {
                    onItemEnable(item);
                }
                return item;
            }
        }

        if (objQueue.Count == 0)
        {
            item = Activator.CreateInstance<T>();
            item.Init(poolPrefab, poolName, aniControlName);
            item.Enable();
            if (item.ItemObj != null)
            {
                item.ItemObj.transform.SetParent(PoolManager.ObjectPoolRoot);
            }
            if (onItemEnable != null)
            {
                onItemEnable(item);
            }
            return item;
        }

        return default(T);
    }

    public void MakeItemBackToPool(T item)
    {
        if (item == null)
        {
            return;
        }
        item.Disable();
        if (onItemDisable != null)
        {
            onItemDisable(item);
        }
        if (item.ItemObj != null)
        {
            item.ItemObj.transform.SetParent(PoolManager.ObjectPoolRoot);
        }
        objQueue.Enqueue(item);
    }

    private void ProcessDeleteQueue()
    {
        frameFlag++;
        if (frameFlag > 9999)
        {
            frameFlag = 0;
        }

        foreach (T item in objQueue)
        {
            item.OnUpdate();
        }

        while (objQueue.Count > 0 && objQueue.Peek().ItemState == PoolItemState.MarkAsDeleted)
        {
            deletedQueue.Enqueue(objQueue.Dequeue());
        }

        if (frameFlag % deleteInterval == 0)
        {
            for (int i = 0; i < deletePerFrame; i++)
            {
                if (deletedQueue.Count <= 0)
                {
                    break;
                }
                T item = deletedQueue.Dequeue();
                item.DestroyThis();
            }
        }
    }

    private void ProcessSpawnQueue()
    {
        while (GetItemActions.Count > 0)
        {
            Action action = GetItemActions.Dequeue();
            action();
        }
    }

    public void OnUpdate()
    {
        if (!isInitComplete)
        {
            return;
        }
        ProcessDeleteQueue();
        ProcessSpawnQueue();
    }

    public void Dispose()
    {
        foreach (T item in objQueue.ToArray())
        {
            item.DestroyThis();
        }
        objQueue.Clear();
        if (onDisable != null)
        {
            onDisable();
        }
        if (poolPrefab != null)
        {
            UnityEngine.Object.DestroyImmediate(poolPrefab, true);
        }
        poolName = string.Empty;
    }
}
