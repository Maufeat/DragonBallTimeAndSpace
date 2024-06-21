using System;
using System.Collections.Generic;

public class BetterDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    private List<KeyValuePair<TKey, TValue>> _keyValuePairs = new List<KeyValuePair<TKey, TValue>>();
    private List<TValue> _listValues = new List<TValue>();
    private bool _changed;
    private bool inforeach;
    private List<TKey> RmoveCache;

    public List<KeyValuePair<TKey, TValue>> KeyValuePairs
    {
        get
        {
            if (this._changed)
            {
                this._keyValuePairs.Clear();
                Dictionary<TKey, TValue>.Enumerator enumerator = this.GetEnumerator();
                while (enumerator.MoveNext())
                    this._keyValuePairs.Add(new KeyValuePair<TKey, TValue>(enumerator.Current.Key, enumerator.Current.Value));
                enumerator.Dispose();
                this._changed = false;
            }
            return this._keyValuePairs;
        }
    }

    public List<TValue> ListValues
    {
        get
        {
            return this._listValues;
        }
    }

    public void AddData(TKey key, TValue value)
    {
        this.Add(key, value);
        this._listValues.Add(value);
    }

    public void RemoveData(TKey key)
    {
        if (!this.ContainsKey(key))
            return;
        this._listValues.Remove(this[key]);
        this.Remove(key);
    }

    public void BetterForeach(Action<KeyValuePair<TKey, TValue>> callback)
    {
        this.inforeach = true;
        for (int index = 0; index < this.KeyValuePairs.Count; ++index)
            callback(this.KeyValuePairs[index]);
        this.inforeach = false;
        this.RemoveFromCache();
    }

    public bool CheckBetterForeach(BetterDictionary<TKey, TValue>.CheckList callback)
    {
        bool flag = false;
        this.inforeach = true;
        for (int index = 0; index < this.KeyValuePairs.Count; ++index)
        {
            if (!callback(this.KeyValuePairs[index]))
            {
                flag = true;
                break;
            }
        }
        this.inforeach = false;
        this.RemoveFromCache();
        return !flag;
    }

    public void BetterForeach(
      Action<KeyValuePair<TKey, TValue>> callback,
      int indexEnd,
      int indexSatrt = 0)
    {
        this.inforeach = true;
        indexEnd = indexEnd <= this.KeyValuePairs.Count ? indexEnd : this.KeyValuePairs.Count;
        for (int index = indexSatrt; index < indexEnd; ++index)
            callback(this.KeyValuePairs[index]);
        this.inforeach = false;
        this.RemoveFromCache();
    }

    public void BetterForeach(
      BetterDictionary<TKey, TValue>.ForeachHandle callback)
    {
        this.inforeach = true;
        int index = 0;
        while (index < this.KeyValuePairs.Count && callback(index, this.KeyValuePairs[index]))
            ++index;
        this.inforeach = false;
        this.RemoveFromCache();
    }

    private void CacheRemove(TKey key)
    {
        if (this.RmoveCache == null)
            this.RmoveCache = new List<TKey>();
        this.RmoveCache.Add(key);
    }

    private void RemoveFromCache()
    {
        if (this.RmoveCache == null)
            return;
        for (int index = 0; index < this.RmoveCache.Count; ++index)
            this.Remove(this.RmoveCache[index]);
        this.RmoveCache.Clear();
    }

    public new void Add(TKey key, TValue value)
    {
        this._changed = true;
        base.Add(key, value);
    }

    public new void Remove(TKey key)
    {
        if (this.inforeach)
        {
            this.CacheRemove(key);
        }
        else
        {
            this._changed = true;
            base.Remove(key);
        }
    }

    public new TValue this[TKey key]
    {
        get
        {
            return base[key];
        }
        set
        {
            this._changed = true;
            base[key] = value;
        }
    }

    public new void Clear()
    {
        this._changed = true;
        base.Clear();
        if (this.RmoveCache != null)
            this.RmoveCache.Clear();
        this._keyValuePairs.Clear();
    }

    public delegate bool CheckList(KeyValuePair<TKey, TValue> map);

    public delegate bool ForeachHandle(int index, KeyValuePair<TKey, TValue> item);
}
