using System;
using System.Collections.Generic;
using UnityEngine;

public struct MoveDataCache
{
    public cs_MoveData checkNeedSync(cs_MoveData data, bool force)
    {
        if (this.caches == null)
        {
            this.caches = new List<cs_MoveData>();
        }
        if (this.poolCachedData == null)
        {
            this.poolCachedData = new List<cs_MoveData>();
        }
        if (force)
        {
            data.step = this.caches.Count + 1;
            if (this.caches.Count > 0 && this.poolCachedData.Count < 5)
            {
                this.poolCachedData.AddRange(this.caches);
            }
            this.caches.Clear();
            return data;
        }
        if (this.caches.Count >= 5)
        {
            int num = Mathf.Abs((int)this.caches[0].pos.fx - (int)data.pos.fx);
            int num2 = Mathf.Abs((int)this.caches[0].pos.fy - (int)data.pos.fy);
            int num3 = (num <= num2) ? num2 : num;
            int num4 = Mathf.Abs((int)(this.caches[0].dir - data.dir));
            if (num3 < 6 && num4 < 15)
            {
                this.caches[this.caches.Count - 1].pos.fx = data.pos.fx;
                this.caches[this.caches.Count - 1].pos.fy = data.pos.fy;
                this.caches[this.caches.Count - 1].dir = data.dir;
                return null;
            }
            data.step = this.caches.Count + 1;
            if (this.caches.Count > 0 && this.poolCachedData.Count < 5)
            {
                this.poolCachedData.AddRange(this.caches);
            }
            this.caches.Clear();
            return data;
        }
        else
        {
            if (this.caches.Count <= 0)
            {
                cs_MoveData cs_MoveData;
                if (this.poolCachedData.Count > 0)
                {
                    cs_MoveData = this.poolCachedData[0];
                    this.poolCachedData.RemoveAt(0);
                }
                else
                {
                    cs_MoveData = new cs_MoveData();
                }
                cs_MoveData.pos = data.pos;
                cs_MoveData.dir = data.dir;
                cs_MoveData.step = data.step;
                this.caches.Add(cs_MoveData);
                return null;
            }
            if (Mathf.Abs((int)(this.caches[0].dir - data.dir)) >= 15)
            {
                data.step = this.caches.Count + 1;
                if (this.caches.Count > 0 && this.poolCachedData.Count < 5)
                {
                    this.poolCachedData.AddRange(this.caches);
                }
                this.caches.Clear();
                return data;
            }
            if (data.dir != this.caches[this.caches.Count - 1].dir || (int)data.pos.fx != (int)this.caches[this.caches.Count - 1].pos.fx || (int)data.pos.fy != (int)this.caches[this.caches.Count - 1].pos.fy)
            {
                int num5 = Mathf.Abs((int)data.pos.fx - (int)this.caches[0].pos.fx);
                int num6 = Mathf.Abs((int)data.pos.fy - (int)this.caches[0].pos.fy);
                int num7 = (num5 <= num6) ? num6 : num5;
                if (num7 >= 6)
                {
                    data.step = this.caches.Count + 1;
                    if (this.caches.Count > 0 && this.poolCachedData.Count < 5)
                    {
                        this.poolCachedData.AddRange(this.caches);
                    }
                    this.caches.Clear();
                    return data;
                }
                cs_MoveData cs_MoveData2;
                if (this.poolCachedData.Count > 0)
                {
                    cs_MoveData2 = this.poolCachedData[0];
                    this.poolCachedData.RemoveAt(0);
                }
                else
                {
                    cs_MoveData2 = new cs_MoveData();
                }
                cs_MoveData2.pos = data.pos;
                cs_MoveData2.dir = data.dir;
                cs_MoveData2.step = data.step;
                this.caches.Add(cs_MoveData2);
            }
            return null;
        }
    }

    public cs_MoveData getSyncData()
    {
        if (this.caches == null)
        {
            return null;
        }
        if (this.caches.Count == 0)
        {
            return null;
        }
        cs_MoveData cs_MoveData = this.caches[this.caches.Count - 1];
        cs_MoveData.step = this.caches.Count;
        this.caches.Clear();
        return cs_MoveData;
    }

    public void ClearMoveCachesData()
    {
        if (this.caches != null)
        {
            this.caches.Clear();
        }
    }

    public List<cs_MoveData> caches;

    private Vector2 lastPlayerPos;

    private List<cs_MoveData> poolCachedData;
}
