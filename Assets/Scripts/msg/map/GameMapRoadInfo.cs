using System;
using System.Collections.Generic;
using Net;
using UnityEngine;

namespace map
{
    public class GameMapRoadInfo : StructData
    {
        public override OctetsStream WriteData(OctetsStream ot)
        {
            this.reBuildRoad();
            this.CalNodeRelation();
            ot.marshal_uint(this.uMagic);
            ot.marshal_uint(this.uVersion);
            ot.marshal_uint(this.uWidth);
            ot.marshal_uint(this.uHeight);
            ot.marshal_uint(this.uRealWidth);
            ot.marshal_uint(this.uRealHeight);
            ot.marshal_int(this.nodes.Count);
            for (int i = 0; i < this.nodes.Count; i++)
            {
                for (int j = 0; j < this.nodes.Count; j++)
                {
                    ot.marshal_int(this.noderelation[i, j]);
                }
            }
            for (int k = 0; k < this.nodes.Count; k++)
            {
                for (int l = 0; l < this.nodes.Count; l++)
                {
                    ot.marshal_struct(this.noderelationTile[k, l]);
                }
            }
            if (this.nodes != null)
            {
                int count = this.nodes.Count;
                ot.marshal_int(count);
                for (int m = 0; m < count; m++)
                {
                    this.nodes[m].WriteData(ot);
                }
            }
            return ot;
        }

        public bool checkInRelationRange(int x, int y)
        {
            if (x == 0 && y == 0)
            {
                return false;
            }
            for (int i = 0; i < this.nodes.Count; i++)
            {
                for (int j = 0; j < this.nodes.Count; j++)
                {
                    if ((int)this.noderelationTile[i, j].x == x && (int)this.noderelationTile[i, j].y == y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void reBuildRoad()
        {
            List<RoadData> list = new List<RoadData>();
            for (int i = 0; i < this.nodes.Count; i++)
            {
                List<RoadTile> list2 = this.nodes[i].nodes;
                if (list2.Count > 10)
                {
                    int num = 9;
                    int num2 = 0;
                    while (num < list2.Count && num2 < list2.Count - 1)
                    {
                        list.Add(new RoadData());
                        RoadData roadData = list[list.Count - 1];
                        roadData.ParentId = this.nodes[i].ParentId;
                        roadData.nodes = new List<RoadTile>();
                        num2 = num + 9;
                        int num3 = num;
                        while (num3 <= num2 && num3 < list2.Count)
                        {
                            roadData.nodes.Add(list2[num3]);
                            num3++;
                        }
                        num += 9;
                        int num4 = list2.Count - num;
                        if (num4 < 6)
                        {
                            for (int j = num + 1; j < list2.Count; j++)
                            {
                                roadData.nodes.Add(list2[j]);
                            }
                            break;
                        }
                    }
                    list2.RemoveRange(10, list2.Count - 10);
                }
            }
            for (int k = 0; k < list.Count; k++)
            {
                this.nodes.Add(list[k]);
            }
        }

        public void CalNodeRelation()
        {
            this.noderelation = new int[this.nodes.Count, this.nodes.Count];
            this.noderelationTile = new RoadTile[this.nodes.Count, this.nodes.Count];
            int count = this.nodes.Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    this.noderelationTile[i, j] = new RoadTile();
                    if (i == j)
                    {
                        this.noderelation[i, j] = 0;
                    }
                    else
                    {
                        this.noderelation[i, j] = 32767;
                    }
                    if (i != j)
                    {
                        if (this.nodes[i].ParentId < 0 && this.nodes[j].ParentId < 0)
                        {
                            for (int k = 0; k < this.nodes[i].nodes.Count; k++)
                            {
                                for (int l = 0; l < this.nodes[j].nodes.Count; l++)
                                {
                                    if (this.nodes[i].nodes[k].x == this.nodes[j].nodes[l].x && this.nodes[i].nodes[k].y == this.nodes[j].nodes[l].y)
                                    {
                                        this.noderelation[i, j] = 1;
                                        this.noderelationTile[i, j] = this.nodes[i].nodes[k];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool CheckPoint(int roadid, ushort x, ushort y)
        {
            if (roadid >= this.nodes.Count)
            {
                FFDebug.LogError(this, string.Format("路线{0}不存在，无法添加路点", roadid));
                return false;
            }
            if (this.nodes[roadid].nodes.Count >= 2)
            {
                Vector2 from = new Vector2((float)(this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 1].x - this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 2].x), (float)(this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 1].y - this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 2].y));
                Vector2 to = new Vector2((float)(x - this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 1].x), (float)(y - this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 1].y));
                float num = Vector2.Angle(from, to);
                if (num > 45f)
                {
                    FFDebug.LogError(this, string.Format("路点{0}，与({1},{2})， ({3},{4})不合法，夹角 {5} 度大于45度", new object[]
                    {
                        roadid,
                        this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 1].x,
                        this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 1].y,
                        this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 2].x,
                        this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 2].y,
                        num
                    }));
                    return false;
                }
            }
            return true;
        }

        public bool AddPoint(int roadid, ushort x, ushort y)
        {
            if (roadid >= this.nodes.Count)
            {
                FFDebug.LogError(this, string.Format("路线{0}不存在，无法添加路点", roadid));
                return false;
            }
            if (this.nodes[roadid].nodes.Count >= 2)
            {
                Vector2 from = new Vector2((float)(this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 1].x - this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 2].x), (float)(this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 1].y - this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 2].y));
                Vector2 to = new Vector2((float)(x - this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 1].x), (float)(y - this.nodes[roadid].nodes[this.nodes[roadid].nodes.Count - 1].y));
                if (Vector2.Angle(from, to) > 45f)
                {
                    FFDebug.LogError(this, string.Format("路线{0}不存在，无法添加路点", roadid));
                    return false;
                }
            }
            for (int i = 0; i < this.nodes[roadid].nodes.Count; i++)
            {
                if (this.nodes[roadid].nodes[i].x == x && this.nodes[roadid].nodes[i].y == y)
                {
                    FFDebug.LogError(this, string.Format("路线{0}, 路点x = {1}, y = {2}已存在，无法添加路点", roadid, x, y));
                    return false;
                }
            }
            this.nodes[roadid].nodes.Add(new RoadTile(x, y));
            this.CalNodeRelation();
            return true;
        }

        public int CheckPointExist(int roadid, ushort x, ushort y)
        {
            for (int i = 0; i < this.nodes[roadid].nodes.Count; i++)
            {
                if (this.nodes[roadid].nodes[i].x == x && this.nodes[roadid].nodes[i].y == y)
                {
                    FFDebug.LogError(this, string.Format("路线{0}, 路点x = {1}, y = {2}已存在，无法添加路点", roadid, x, y));
                    return i;
                }
            }
            return -1;
        }

        public bool InsertPoint(int roadid, int index, ushort x, ushort y)
        {
            if (roadid >= this.nodes.Count)
            {
                FFDebug.LogError(this, string.Format("路线{0}不存在，无法添加路点", roadid));
                return false;
            }
            if (this.nodes[roadid].nodes.Count <= index)
            {
                FFDebug.LogError(this, string.Format("路线{0}当前点无法插入", roadid));
                return false;
            }
            for (int i = 0; i < this.nodes[roadid].nodes.Count; i++)
            {
                if (this.nodes[roadid].nodes[i].x == x && this.nodes[roadid].nodes[i].y == y)
                {
                    FFDebug.LogError(this, string.Format("路线{0}, 路点x = {1}, y = {2}已存在，无法添加路点", roadid, x, y));
                    return false;
                }
            }
            if (index >= 1)
            {
                Vector2 from = new Vector2((float)(this.nodes[roadid].nodes[index].x - this.nodes[roadid].nodes[index - 1].x), (float)(this.nodes[roadid].nodes[index].y - this.nodes[roadid].nodes[index - 1].y));
                Vector2 to = new Vector2((float)(x - this.nodes[roadid].nodes[index].x), (float)(y - this.nodes[roadid].nodes[index].y));
                if (Vector2.Angle(from, to) > 45f)
                {
                    FFDebug.LogError(this, string.Format("路线{0}角度不合法，无法添加路点", roadid));
                    return false;
                }
            }
            index++;
            if (index >= this.nodes[roadid].nodes.Count)
            {
                this.nodes[roadid].nodes.Add(new RoadTile(x, y));
            }
            else
            {
                this.nodes[roadid].nodes.Insert(index, new RoadTile(x, y));
            }
            this.CalNodeRelation();
            return true;
        }

        public bool RemovePoint(int roadid, ushort x, ushort y)
        {
            if (roadid >= this.nodes.Count)
            {
                FFDebug.LogError(this, string.Format("路线{0}不存在，无法删除路点", roadid));
                return false;
            }
            for (int i = 0; i < this.nodes[roadid].nodes.Count; i++)
            {
                if (this.nodes[roadid].nodes[i].x == x && this.nodes[roadid].nodes[i].y == y)
                {
                    this.nodes[roadid].nodes.RemoveAt(i);
                    this.CalNodeRelation();
                    return true;
                }
            }
            FFDebug.LogError(this, string.Format("路线{0}, 路点x = {1}, y = {2}不存在，无法删除", roadid, x, y));
            return false;
        }

        public bool checkRoadPoint(int x, int y, out int roadid)
        {
            roadid = -1;
            for (int i = 0; i < this.nodes.Count; i++)
            {
                for (int j = 0; j < this.nodes[i].nodes.Count; j++)
                {
                    if ((int)this.nodes[i].nodes[j].x == x && (int)this.nodes[i].nodes[j].y == y)
                    {
                        roadid = i;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool checkRoadPoint(int excluderoadid, int x, int y, out int roadid)
        {
            roadid = -1;
            for (int i = 0; i < this.nodes.Count; i++)
            {
                if (i != excluderoadid)
                {
                    for (int j = 0; j < this.nodes[i].nodes.Count; j++)
                    {
                        if ((int)this.nodes[i].nodes[j].x == x && (int)this.nodes[i].nodes[j].y == y)
                        {
                            roadid = i;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public OctetsStream ReadOldData(OctetsStream ot)
        {
            this.uMagic = ot.unmarshal_uint();
            this.uVersion = ot.unmarshal_uint();
            this.uWidth = ot.unmarshal_uint();
            this.uHeight = ot.unmarshal_uint();
            this.uRealWidth = ot.unmarshal_uint();
            this.uRealHeight = ot.unmarshal_uint();
            int num = ot.unmarshal_int();
            this.noderelation = new int[num, num];
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    this.noderelation[i, j] = ot.unmarshal_int();
                }
            }
            this.noderelationTile = new RoadTile[num, num];
            for (int k = 0; k < num; k++)
            {
                for (int l = 0; l < num; l++)
                {
                    RoadTile roadTile = new RoadTile();
                    ot.unmarshal_struct(roadTile);
                    this.noderelationTile[k, l] = roadTile;
                }
            }
            uint num2 = this.uWidth;
            uint num3 = this.uHeight;
            this.nodes = new List<RoadData>();
            try
            {
                int num4 = ot.unmarshal_int();
                for (int m = 0; m < num4; m++)
                {
                    RoadData roadData = new RoadData();
                    roadData.nodes = new List<RoadTile>();
                    this.nodes.Add(roadData);
                    roadData.ReadOldData(ot);
                }
            }
            catch (Exception ex)
            {
            }
            return ot;
        }

        public override OctetsStream ReadData(OctetsStream ot)
        {
            this.uMagic = ot.unmarshal_uint();
            this.uVersion = ot.unmarshal_uint();
            this.uWidth = ot.unmarshal_uint();
            this.uHeight = ot.unmarshal_uint();
            this.uRealWidth = ot.unmarshal_uint();
            this.uRealHeight = ot.unmarshal_uint();
            int num = ot.unmarshal_int();
            this.noderelation = new int[num, num];
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    this.noderelation[i, j] = ot.unmarshal_int();
                }
            }
            this.noderelationTile = new RoadTile[num, num];
            for (int k = 0; k < num; k++)
            {
                for (int l = 0; l < num; l++)
                {
                    RoadTile roadTile = new RoadTile();
                    ot.unmarshal_struct(roadTile);
                    this.noderelationTile[k, l] = roadTile;
                }
            }
            uint num2 = this.uWidth;
            uint num3 = this.uHeight;
            this.nodes = new List<RoadData>();
            try
            {
                int num4 = ot.unmarshal_int();
                for (int m = 0; m < num4; m++)
                {
                    RoadData roadData = new RoadData();
                    roadData.nodes = new List<RoadTile>();
                    this.nodes.Add(roadData);
                    roadData.ReadData(ot);
                }
            }
            catch (Exception ex)
            {
            }
            return ot;
        }

        public bool RoadCheck()
        {
            bool result = false;
            int count = this.nodes.Count;
            for (int i = 0; i < count; i++)
            {
                bool flag = true;
                for (int j = 0; j < count; j++)
                {
                    if (this.noderelation[i, j] == 1)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    result = true;
                    FFDebug.LogError(this, string.Format("路线{0}是独立的路线，请检查是否有错误！", i));
                }
            }
            return result;
        }

        private const int INF = 32767;

        public uint uMagic;

        public uint uVersion;

        public uint uWidth;

        public uint uHeight;

        public uint uRealWidth;

        public uint uRealHeight;

        public int[,] noderelation;

        public RoadTile[,] noderelationTile;

        public List<RoadData> nodes;
    }
}
