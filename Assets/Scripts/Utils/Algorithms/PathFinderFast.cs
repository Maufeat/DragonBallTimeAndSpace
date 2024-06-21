using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using lighttool.NavMesh;
using map;
using UnityEngine;

namespace Algorithms
{
    public class PathFinderFast : IPathFinder
    {
        #region Structs
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct PathFinderNodeFast
        {
            #region Variables Declaration
            public int F; // f = gone + heuristic
            public int G;
            public ushort PX; // Parent
            public ushort PY;
            public byte Status;
            #endregion
        }
        #endregion

        public PathFinderFast(byte[,] grid)
        {
            RefreshData(grid);
        }

        #region Events
        public event PathFinderDebugHandler PathFinderDebug;
        #endregion

        #region Properties
        public bool Stopped
        {
            get { return mStopped; }
        }

        public HeuristicFormula Formula
        {
            get { return mFormula; }
            set { mFormula = value; }
        }

        public bool Diagonals
        {
            get { return mDiagonals; }
            set
            {
                mDiagonals = value;
                if (mDiagonals)
                    mDirection = new sbyte[8, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
                else
                    mDirection = new sbyte[4, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
            }
        }

        public bool HeavyDiagonals
        {
            get { return mHeavyDiagonals; }
            set { mHeavyDiagonals = value; }
        }

        public int HeuristicEstimate
        {
            get { return mHEstimate; }
            set { mHEstimate = value; }
        }

        public bool PunishChangeDirection
        {
            get { return mPunishChangeDirection; }
            set { mPunishChangeDirection = value; }
        }

        public bool TieBreaker
        {
            get { return mTieBreaker; }
            set { mTieBreaker = value; }
        }

        public int SearchLimit
        {
            get { return mSearchLimit; }
            set { mSearchLimit = value; }
        }

        public double CompletedTime
        {
            get { return mCompletedTime; }
            set { mCompletedTime = value; }
        }

        public bool DebugProgress
        {
            get { return mDebugProgress; }
            set { mDebugProgress = value; }
        }

        public bool DebugFoundPath
        {
            get { return mDebugFoundPath; }
            set { mDebugFoundPath = value; }
        }
        #endregion

        #region Methods
        public void FindPathStop()
        {
            mStop = true;
        }

        public void RefreshData(byte[,] grid)
        {
            if (grid == null)
                throw new Exception("Grid cannot be null");
            mGrid = grid;
        }

        public Point GetBestAccessiblePoint(Point start, Point end)
        {
            Point result = null;
            List<Point> list = new List<Point>();
            bool flag = false;
            int num = 1;
            while (!flag)
            {
                int num2 = end.Y + num;
                int num3 = end.Y - num;
                int num4 = end.X - num;
                int num5 = end.X + num;
                for (int i = num4 + 1; i < num5; i++)
                {
                    for (int j = num3 + 1; j < num2; j++)
                    {
                        if (i >= this.mGrid.GetLength(0) || j >= this.mGrid.GetLength(1) || j == -1 || i == -1)
                        {
                            FFDebug.LogWarning(this, string.Concat(new object[]
                            {
                                "GetBestAccessiblePoint exception : i >= mGrid.GetLength(0) || j >= mGrid.GetLength(1) + ",
                                end.X,
                                ",",
                                end.Y
                            }));
                            return result;
                        }
                        if (!GraphUtils.IsBlockPointForMove((int)this.mGrid[j, i]))
                        {
                            list.Add(new Point(i, j));
                            flag = true;
                        }
                    }
                }
                num++;
            }
            if (flag && list.Count > 0)
            {
                list.Sort((Point x, Point y) => this.GetManhattanDistance(x, end).CompareTo(this.GetManhattanDistance(y, end)));
                result = list[0];
            }
            return result;
        }

        internal int GetManhattanDistance(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        public bool IsPointConnected(Vector2 _s, Vector2 _e)
        {
            float num = Vector2.Distance(_s, _e);
            for (float num2 = 0f; num2 < num; num2 += 0.5f)
            {
                Vector2 vector = Vector2.Lerp(_s, _e, num2 / num);
                if (!this.IsFreePoint(vector.x, vector.y))
                {
                    return false;
                }
            }
            return true;
        }

        public List<int> GetPointsInPoly(List<navVec3> ss)
        {
            if (ss == null)
            {
                return null;
            }
            List<int> list = new List<int>();
            for (int i = 0; i < ss.Count; i++)
            {
                list.Add(-1);
            }
            if (MapLoader.info == null || MapLoader.info.nodes == null)
            {
                return null;
            }
            for (int j = 0; j < MapLoader.info.nodes.Length; j++)
            {
                if (this.mStop)
                {
                    return null;
                }
                for (int k = 0; k < ss.Count; k++)
                {
                    if (list[k] < 0 && MapLoader.info.inPoly(ss[k], MapLoader.info.nodes[j].poly))
                    {
                        list[k] = j;
                    }
                }
            }
            return list;
        }

        public navVec3 GetNavOKPoint(navVec3 _navVec, int _numMax)
        {
            float num = 0.5f;
            navVec3 navVec = new navVec3();
            for (int i = 1; i < _numMax; i++)
            {
                double x = _navVec.x + (double)(num * (float)i);
                double z = _navVec.z + (double)(num * (float)i);
                double num2 = _navVec.x - (double)(num * (float)i);
                double num3 = _navVec.z - (double)(num * (float)i);
                int num4 = i * 2 + 1;
                for (int j = 1; j < num4; j++)
                {
                    double x2 = num2 + (double)(num * (float)j);
                    double z2 = num3 + (double)(num * (float)j);
                    double z3 = num3 + (double)(num * (float)j) - 1.0;
                    double x3 = num2 + (double)(num * (float)j) - 1.0;
                    this.sv1.ResetValue(num2, 0.0, z2, -1);
                    this.sv2.ResetValue(x2, 0.0, z, -1);
                    this.sv3.ResetValue(x, 0.0, z3, -1);
                    this.sv4.ResetValue(x3, 0.0, num3, -1);
                    this.nvs.Clear();
                    this.nvs.Add(this.sv1);
                    this.nvs.Add(this.sv2);
                    this.nvs.Add(this.sv3);
                    this.nvs.Add(this.sv4);
                    List<int> pointsInPoly = this.GetPointsInPoly(this.nvs);
                    if (pointsInPoly == null)
                    {
                        return null;
                    }
                    for (int k = 0; k < pointsInPoly.Count; k++)
                    {
                        if (pointsInPoly[k] >= 0)
                        {
                            Vector2 serverPosByWorldPos_new = GraphUtils.GetServerPosByWorldPos_new(new Vector3((float)_navVec.x, (float)_navVec.y, (float)_navVec.z));
                            Vector2 serverPosByWorldPos_new2 = GraphUtils.GetServerPosByWorldPos_new(new Vector3((float)this.nvs[k].x, (float)this.nvs[k].y, (float)this.nvs[k].z));
                            if (this.IsPointConnected(serverPosByWorldPos_new, serverPosByWorldPos_new2))
                            {
                                navVec = this.nvs[k];
                                navVec.polyId = pointsInPoly[k];
                                return navVec;
                            }
                        }
                    }
                }
            }
            return navVec;
        }

        public HashSet<PathFinderNode> FindPathByNavMesh(Point start, Point end, CheckBlockHandle checkblockhandle = null)
        {
            HashSet<PathFinderNode> result;
            lock (this)
            {
                if (MapLoader.info == null)
                {
                    result = null;
                }
                else
                {
                    this.mStop = false;
                    Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(new Vector2((float)start.X, (float)start.Y));
                    Vector3 worldPosByServerPos2 = GraphUtils.GetWorldPosByServerPos(new Vector2((float)end.X, (float)end.Y));
                    this.srcPoly = (this.dstPoly = -1);
                    this.srcVec.x = (double)worldPosByServerPos.x;
                    this.srcVec.y = 0.0;
                    this.srcVec.z = (double)worldPosByServerPos.z;
                    this.dstVec.x = (double)worldPosByServerPos2.x;
                    this.dstVec.y = 0.0;
                    this.dstVec.z = (double)worldPosByServerPos2.z;
                    List<int> pointsInPoly = this.GetPointsInPoly(new List<navVec3>
                    {
                        this.srcVec,
                        this.dstVec
                    });
                    if (pointsInPoly == null)
                    {
                        result = null;
                    }
                    else
                    {
                        this.srcPoly = pointsInPoly[0];
                        this.dstPoly = pointsInPoly[1];
                        navVec3 navVec = this.srcVec.clone();
                        navVec3 navVec2 = this.dstVec.clone();
                        if (this.srcPoly < 0)
                        {
                            navVec = this.GetNavOKPoint(this.srcVec, 10);
                            if (navVec == null)
                            {
                                return null;
                            }
                            this.srcPoly = navVec.polyId;
                        }
                        if (this.dstPoly < 0)
                        {
                            navVec2 = this.GetNavOKPoint(this.dstVec, 10);
                            if (navVec2 == null)
                            {
                                return null;
                            }
                            this.dstPoly = navVec2.polyId;
                        }
                        if (this.srcPoly >= 0 && this.dstPoly >= 0)
                        {
                            int[] array = pathFinding.calcAStarPolyPath(MapLoader.info, this.srcPoly, this.dstPoly, null, 0.1f);
                            if (array == null)
                            {
                                result = null;
                            }
                            else
                            {
                                List<navVec3> list = new List<navVec3>(pathFinding.calcWayPoints(MapLoader.info, navVec, navVec2, array, 0.1f));
                                list.Insert(0, this.srcVec);
                                list.Add(this.dstVec);
                                if (list != null)
                                {
                                    this.mCloseFast.Clear();
                                    Vector3 pos = new Vector3((float)list[0].x, (float)list[0].y, (float)list[0].z);
                                    Vector3 vector = GraphUtils.GetServerPosByWorldPos_new(pos);
                                    this.mCloseFast.Add(this.vector32node(vector));
                                    Vector3 vector2 = vector;
                                    for (int i = 1; i < list.Count; i++)
                                    {
                                        Vector3 pos2 = new Vector3((float)list[i].x, (float)list[i].y, (float)list[i].z);
                                        Vector3 v = GraphUtils.GetServerPosByWorldPos(pos2, true);
                                        if ((int)vector2.x != (int)v.x || (int)vector2.y != (int)v.y)
                                        {
                                            this.mCloseFast.Add(this.vector32node(v));
                                        }
                                    }
                                    result = this.mCloseFast;
                                }
                                else
                                {
                                    result = null;
                                }
                            }
                        }
                        else
                        {
                            result = null;
                        }
                    }
                }
            }
            return result;
        }

        private bool IsFreePoint(float x, float y)
        {
            return x < (float)CellInfos.MapInfos.GetLength(0) && y < (float)CellInfos.MapInfos.GetLength(1) && x >= 0f && y >= 0f && !GraphUtils.IsBlockPointForPathfind(x, y);
        }

        private List<PathFinderNode> getNodes(Vector3 src, Vector3 to, bool xadd, bool xsub, bool yadd, bool ysub, CheckBlockHandle checkblockhandle = null)
        {
            List<PathFinderNode> list = new List<PathFinderNode>();
            if (this.mStop)
            {
                return null;
            }
            int num = (int)src.x - (int)to.x;
            int num2 = (int)src.y - (int)to.y;
            int num3 = (num < 0) ? -1 : 1;
            int num4 = (num2 < 0) ? -1 : 1;
            Vector3 v = src;
            while (v.x != to.x || v.y != to.y)
            {
                if (v.x != to.x)
                {
                    v.x -= (float)num3;
                }
                if (v.y != to.y)
                {
                    v.y -= (float)num4;
                }
                if (v.x == to.x && v.y == to.y)
                {
                    list.Add(this.vector32node(v));
                    return list;
                }
                if (checkblockhandle != null)
                {
                    if (checkblockhandle((int)this.mGrid[(int)v.y, (int)v.x]))
                    {
                    }
                }
                else if (GraphUtils.IsBlockPointForMove((int)this.mGrid[(int)v.y, (int)v.x]))
                {
                }
                bool flag = false;
                if (flag)
                {
                    return null;
                }
                list.Add(this.vector32node(v));
            }
            return null;
        }

        private List<PathFinderNode> getOffsetNodes(Vector3 from, Vector3 to, bool bxadd, bool bxsub, bool byadd, bool bysub, CheckBlockHandle checkblockhandle = null)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = (int)to.x - (int)from.x;
            int num6 = (int)to.y - (int)from.y;
            if (num5 == 0)
            {
                num = 1;
                num3 = -1;
            }
            else if (num6 == 0)
            {
                num2 = 1;
                num4 = -1;
            }
            else if (num5 > 0 && num6 > 0)
            {
                num = 1;
                num2 = 1;
            }
            else if (num5 > 0 && num6 < 0)
            {
                num = 1;
                num4 = -1;
            }
            else if (num5 < 0 && num6 > 0)
            {
                num3 = -1;
                num2 = 1;
            }
            else if (num5 < 0 && num6 < 0)
            {
                num3 = -1;
                num4 = -1;
            }
            List<PathFinderNode> list = new List<PathFinderNode>();
            list.Clear();
            Vector3 vector = from;
            if (num != 0 && bxadd)
            {
                List<PathFinderNode> nodes;
                for (; ; )
                {
                    vector.x += (float)num;
                    if (checkblockhandle != null)
                    {
                        if (checkblockhandle((int)this.mGrid[(int)vector.y, (int)vector.x]))
                        {
                            break;
                        }
                    }
                    else if (GraphUtils.IsBlockPointForMove((int)this.mGrid[(int)vector.y, (int)vector.x]))
                    {
                        break;
                    }
                    list.Add(this.vector32node(vector));
                    nodes = this.getNodes(vector, to, bxadd, false, byadd, bysub, checkblockhandle);
                    if (nodes != null)
                    {
                        goto Block_15;
                    }
                }
                goto IL_199;
            Block_15:
                for (int i = 0; i < nodes.Count; i++)
                {
                    list.Add(nodes[i]);
                }
                return list;
            }
        IL_199:
            list.Clear();
            vector = from;
            if (num3 != 0 && bxsub)
            {
                List<PathFinderNode> nodes2;
                for (; ; )
                {
                    vector.x += (float)num3;
                    if (checkblockhandle != null)
                    {
                        if (checkblockhandle((int)this.mGrid[(int)vector.y, (int)vector.x]))
                        {
                            break;
                        }
                    }
                    else if (GraphUtils.IsBlockPointForMove((int)this.mGrid[(int)vector.y, (int)vector.x]))
                    {
                        break;
                    }
                    list.Add(this.vector32node(vector));
                    nodes2 = this.getNodes(vector, to, false, bxsub, byadd, bysub, checkblockhandle);
                    if (nodes2 != null)
                    {
                        goto Block_21;
                    }
                }
                goto IL_280;
            Block_21:
                for (int j = 0; j < nodes2.Count; j++)
                {
                    list.Add(nodes2[j]);
                }
                return list;
            }
        IL_280:
            list.Clear();
            vector = from;
            if (num2 != 0 && byadd)
            {
                List<PathFinderNode> nodes3;
                for (; ; )
                {
                    vector.y += (float)num2;
                    if (checkblockhandle != null)
                    {
                        if (checkblockhandle((int)this.mGrid[(int)vector.y, (int)vector.x]))
                        {
                            break;
                        }
                    }
                    else if (GraphUtils.IsBlockPointForMove((int)this.mGrid[(int)vector.y, (int)vector.x]))
                    {
                        break;
                    }
                    list.Add(this.vector32node(vector));
                    nodes3 = this.getNodes(vector, to, bxadd, bxsub, byadd, false, checkblockhandle);
                    if (nodes3 != null)
                    {
                        goto Block_27;
                    }
                }
                goto IL_366;
            Block_27:
                for (int k = 0; k < nodes3.Count; k++)
                {
                    list.Add(nodes3[k]);
                }
                return list;
            }
        IL_366:
            list.Clear();
            vector = from;
            if (num4 != 0 && bysub)
            {
                List<PathFinderNode> nodes4;
                for (; ; )
                {
                    vector.y += (float)num4;
                    if (checkblockhandle != null)
                    {
                        if (checkblockhandle((int)this.mGrid[(int)vector.y, (int)vector.x]))
                        {
                            break;
                        }
                    }
                    else if (GraphUtils.IsBlockPointForMove((int)this.mGrid[(int)vector.y, (int)vector.x]))
                    {
                        break;
                    }
                    list.Add(this.vector32node(vector));
                    nodes4 = this.getNodes(vector, to, bxadd, bxsub, false, bysub, checkblockhandle);
                    if (nodes4 != null)
                    {
                        goto Block_33;
                    }
                }
                goto IL_44C;
            Block_33:
                for (int l = 0; l < nodes4.Count; l++)
                {
                    list.Add(nodes4[l]);
                }
                return list;
            }
        IL_44C:
            return null;
        }

        private PathFinderNode vector32node(Vector3 v)
        {
            PathFinderNode result;
            result.F = 0;
            result.G = 0;
            result.H = 0;
            result.PX = (int)v.x;
            result.PY = (int)v.y;
            result.X = (int)v.x;
            result.Y = (int)v.y;
            return result;
        }

        private PathFinderNode point32node(Point v)
        {
            PathFinderNode result;
            result.F = 0;
            result.G = 0;
            result.H = 0;
            result.PX = v.X;
            result.PY = v.Y;
            result.X = v.X;
            result.Y = v.Y;
            return result;
        }

        public List<PathFinderNode> FindPath(Point start, Point end, CheckBlockHandle checkblockhandle = null)
        {
            List<PathFinderNode> result;
            lock (this)
            {
                this.mFound = false;
                this.mStop = false;
                this.mStopped = false;
                this.mCloseNodeCounter = 0;
                this.mOpenNodeValue += 2;
                this.mCloseNodeValue += 2;
                this.mOpen.Clear();
                this.mClose.Clear();
                if (this.mDebugProgress && this.PathFinderDebug != null)
                {
                    this.PathFinderDebug(0, 0, start.X, start.Y, PathFinderNodeType.Start, -1, -1);
                }
                if (this.mDebugProgress && this.PathFinderDebug != null)
                {
                    this.PathFinderDebug(0, 0, end.X, end.Y, PathFinderNodeType.End, -1, -1);
                }
                this.mLocation = (start.Y << (int)this.mGridYLog2) + start.X;
                this.mEndLocation = (end.Y << (int)this.mGridYLog2) + end.X;
                this.mCalcGrid[this.mLocation].G = 0;
                this.mCalcGrid[this.mLocation].F = this.mHEstimate;
                this.mCalcGrid[this.mLocation].PX = (ushort)start.X;
                this.mCalcGrid[this.mLocation].PY = (ushort)start.Y;
                this.mCalcGrid[this.mLocation].Status = this.mOpenNodeValue;
                this.mOpen.Push(this.mLocation);
                while (this.mOpen.Count > 0 && !this.mStop)
                {
                    this.mLocation = this.mOpen.Pop();
                    if (this.mCalcGrid[this.mLocation].Status != this.mCloseNodeValue)
                    {
                        this.mLocationX = (ushort)(this.mLocation & (int)this.mGridXMinus1);
                        this.mLocationY = (ushort)(this.mLocation >> (int)this.mGridYLog2);
                        if (this.mDebugProgress && this.PathFinderDebug != null)
                        {
                            this.PathFinderDebug(0, 0, this.mLocation & (int)this.mGridXMinus1, this.mLocation >> (int)this.mGridYLog2, PathFinderNodeType.Current, -1, -1);
                        }
                        if (this.mLocation == this.mEndLocation)
                        {
                            this.mCalcGrid[this.mLocation].Status = this.mCloseNodeValue;
                            this.mFound = true;
                            break;
                        }
                        if (this.mCloseNodeCounter > this.mSearchLimit && this.mSearchLimit != -1)
                        {
                            this.mStopped = true;
                            return null;
                        }
                        if (this.mPunishChangeDirection)
                        {
                            this.mHoriz = (int)(this.mLocationX - this.mCalcGrid[this.mLocation].PX);
                        }
                        for (int i = 0; i < ((!this.mDiagonals) ? 4 : 8); i++)
                        {
                            this.mNewLocationX = (ushort)((int)this.mLocationX + (int)this.mDirection[i, 0]);
                            this.mNewLocationY = (ushort)((int)this.mLocationY + (int)this.mDirection[i, 1]);
                            this.mNewLocation = ((int)this.mNewLocationY << (int)this.mGridYLog2) + (int)this.mNewLocationX;
                            if (this.mNewLocationX < this.mGridX && this.mNewLocationY < this.mGridY)
                            {
                                if (checkblockhandle != null)
                                {
                                    if (checkblockhandle((int)this.mGrid[(int)this.mNewLocationY, (int)this.mNewLocationX]))
                                    {
                                        goto IL_8FA;
                                    }
                                }
                                else if (GraphUtils.IsBlockPointForMove((int)this.mGrid[(int)this.mNewLocationY, (int)this.mNewLocationX]))
                                {
                                    goto IL_8FA;
                                }
                                if (this.mHeavyDiagonals && i > 3)
                                {
                                    this.mNewG = this.mCalcGrid[this.mLocation].G + (int)((double)this.mGrid[(int)this.mNewLocationY, (int)this.mNewLocationX] * 2.41);
                                }
                                else
                                {
                                    this.mNewG = this.mCalcGrid[this.mLocation].G + (int)this.mGrid[(int)this.mNewLocationY, (int)this.mNewLocationX];
                                }
                                if (this.mPunishChangeDirection)
                                {
                                    if (this.mNewLocationX - this.mLocationX != 0 && this.mHoriz == 0)
                                    {
                                        this.mNewG += Math.Abs((int)this.mNewLocationX - end.X) + Math.Abs((int)this.mNewLocationY - end.Y);
                                    }
                                    if (this.mNewLocationY - this.mLocationY != 0 && this.mHoriz != 0)
                                    {
                                        this.mNewG += Math.Abs((int)this.mNewLocationX - end.X) + Math.Abs((int)this.mNewLocationY - end.Y);
                                    }
                                }
                                if ((this.mCalcGrid[this.mNewLocation].Status != this.mOpenNodeValue && this.mCalcGrid[this.mNewLocation].Status != this.mCloseNodeValue) || this.mCalcGrid[this.mNewLocation].G > this.mNewG)
                                {
                                    this.mCalcGrid[this.mNewLocation].PX = this.mLocationX;
                                    this.mCalcGrid[this.mNewLocation].PY = this.mLocationY;
                                    this.mCalcGrid[this.mNewLocation].G = this.mNewG;
                                    switch (this.mFormula)
                                    {
                                        default:
                                            this.mH = this.mHEstimate * (Math.Abs((int)this.mNewLocationX - end.X) + Math.Abs((int)this.mNewLocationY - end.Y));
                                            break;
                                        case HeuristicFormula.MaxDXDY:
                                            this.mH = this.mHEstimate * Math.Max(Math.Abs((int)this.mNewLocationX - end.X), Math.Abs((int)this.mNewLocationY - end.Y));
                                            break;
                                        case HeuristicFormula.DiagonalShortCut:
                                            {
                                                int num = Math.Min(Math.Abs((int)this.mNewLocationX - end.X), Math.Abs((int)this.mNewLocationY - end.Y));
                                                int num2 = Math.Abs((int)this.mNewLocationX - end.X) + Math.Abs((int)this.mNewLocationY - end.Y);
                                                this.mH = this.mHEstimate * 2 * num + this.mHEstimate * (num2 - 2 * num);
                                                break;
                                            }
                                        case HeuristicFormula.Euclidean:
                                            this.mH = (int)((double)this.mHEstimate * Math.Sqrt(Math.Pow((double)((int)this.mNewLocationY - end.X), 2.0) + Math.Pow((double)((int)this.mNewLocationY - end.Y), 2.0)));
                                            break;
                                        case HeuristicFormula.EuclideanNoSQR:
                                            this.mH = (int)((double)this.mHEstimate * (Math.Pow((double)((int)this.mNewLocationX - end.X), 2.0) + Math.Pow((double)((int)this.mNewLocationY - end.Y), 2.0)));
                                            break;
                                        case HeuristicFormula.Custom1:
                                            {
                                                Point point = new Point(Math.Abs(end.X - (int)this.mNewLocationX), Math.Abs(end.Y - (int)this.mNewLocationY));
                                                int num3 = Math.Abs(point.X - point.Y);
                                                int num4 = Math.Abs((point.X + point.Y - num3) / 2);
                                                this.mH = this.mHEstimate * (num4 + num3 + point.X + point.Y);
                                                break;
                                            }
                                    }
                                    if (this.mTieBreaker)
                                    {
                                        int num5 = (int)this.mLocationX - end.X;
                                        int num6 = (int)this.mLocationY - end.Y;
                                        int num7 = start.X - end.X;
                                        int num8 = start.Y - end.Y;
                                        int num9 = Math.Abs(num5 * num8 - num7 * num6);
                                        this.mH = (int)((double)this.mH + (double)num9 * 0.001);
                                    }
                                    this.mCalcGrid[this.mNewLocation].F = this.mNewG + this.mH;
                                    if (this.mDebugProgress && this.PathFinderDebug != null)
                                    {
                                        this.PathFinderDebug((int)this.mLocationX, (int)this.mLocationY, (int)this.mNewLocationX, (int)this.mNewLocationY, PathFinderNodeType.Open, this.mCalcGrid[this.mNewLocation].F, this.mCalcGrid[this.mNewLocation].G);
                                    }
                                    this.mOpen.Push(this.mNewLocation);
                                    this.mCalcGrid[this.mNewLocation].Status = this.mOpenNodeValue;
                                }
                            }
                        IL_8FA:;
                        }
                        this.mCloseNodeCounter++;
                        this.mCalcGrid[this.mLocation].Status = this.mCloseNodeValue;
                        if (this.mDebugProgress && this.PathFinderDebug != null)
                        {
                            this.PathFinderDebug(0, 0, (int)this.mLocationX, (int)this.mLocationY, PathFinderNodeType.Close, this.mCalcGrid[this.mLocation].F, this.mCalcGrid[this.mLocation].G);
                        }
                    }
                }
                if (this.mStop && !this.mFound)
                {
                    result = null;
                }
                else if (this.mFound)
                {
                    this.mClose.Clear();
                    int num10 = end.X;
                    int num11 = end.Y;
                    PathFinderFast.PathFinderNodeFast pathFinderNodeFast = this.mCalcGrid[(end.Y << (int)this.mGridYLog2) + end.X];
                    PathFinderNode item;
                    item.F = pathFinderNodeFast.F;
                    item.G = pathFinderNodeFast.G;
                    item.H = 0;
                    item.PX = (int)pathFinderNodeFast.PX;
                    item.PY = (int)pathFinderNodeFast.PY;
                    item.X = end.X;
                    item.Y = end.Y;
                    while (item.X != item.PX || item.Y != item.PY)
                    {
                        this.mClose.Add(item);
                        if (this.mDebugFoundPath && this.PathFinderDebug != null)
                        {
                            this.PathFinderDebug(item.PX, item.PY, item.X, item.Y, PathFinderNodeType.Path, item.F, item.G);
                        }
                        num10 = item.PX;
                        num11 = item.PY;
                        pathFinderNodeFast = this.mCalcGrid[(num11 << (int)this.mGridYLog2) + num10];
                        item.F = pathFinderNodeFast.F;
                        item.G = pathFinderNodeFast.G;
                        item.H = 0;
                        item.PX = (int)pathFinderNodeFast.PX;
                        item.PY = (int)pathFinderNodeFast.PY;
                        item.X = num10;
                        item.Y = num11;
                    }
                    this.mClose.Add(item);
                    if (this.mDebugFoundPath && this.PathFinderDebug != null)
                    {
                        this.PathFinderDebug(item.PX, item.PY, item.X, item.Y, PathFinderNodeType.Path, item.F, item.G);
                    }
                    this.mStopped = true;
                    this.mClose.Reverse();
                    result = this.mClose;
                }
                else
                {
                    FFDebug.LogWarning(this, "Path Not Found!");
                    this.mStopped = true;
                    result = null;
                }
            }
            return result;
        }

        public bool GetPointGraphFlag(float fx, float fy, TileFlag flag)
        {
            int num = Math.Max(Mathf.FloorToInt(fx), this.GetWight() - 1);
            int num2 = Math.Max(Mathf.FloorToInt(fy), this.GetHeight() - 1);
            return GraphUtils.IsFlag((int)this.mGrid[num2, num], (int)flag);
        }

        public int GetWight()
        {
            return this.mGrid.GetLength(0);
        }

        public int GetHeight()
        {
            return this.mGrid.GetLength(1);
        }

        #endregion

        private byte[,] mGrid;

        private PriorityQueueB<int> mOpen;

        private List<PathFinderNode> mClose = new List<PathFinderNode>();

        private HashSet<PathFinderNode> mCloseFast = new HashSet<PathFinderNode>();

        private bool mStop;

        private bool mStopped = true;

        private int mHoriz;

        private HeuristicFormula mFormula = HeuristicFormula.Manhattan;

        private bool mDiagonals = true;

        private int mHEstimate = 10;

        private bool mPunishChangeDirection = true;

        private bool mTieBreaker;

        private bool mHeavyDiagonals;

        private int mSearchLimit = int.MaxValue;

        private double mCompletedTime;

        private bool mDebugProgress;

        private bool mDebugFoundPath;

        private PathFinderFast.PathFinderNodeFast[] mCalcGrid;

        private byte mOpenNodeValue = 1;

        private byte mCloseNodeValue = 2;

        private int mH;

        private int mLocation;

        private int mNewLocation;

        private ushort mLocationX;

        private ushort mLocationY;

        private ushort mNewLocationX;

        private ushort mNewLocationY;

        private int mCloseNodeCounter;

        private ushort mGridX;

        private ushort mGridY;

        private ushort mGridXMinus1;

        private ushort mGridYLog2;

        private bool mFound;

        private sbyte[,] mDirection = new sbyte[,]
        {
            {
                0,
                -1
            },
            {
                1,
                0
            },
            {
                0,
                1
            },
            {
                -1,
                0
            },
            {
                1,
                -1
            },
            {
                1,
                1
            },
            {
                -1,
                1
            },
            {
                -1,
                -1
            }
        };

        private int mEndLocation;

        private int mNewG;

        private int srcPoly = -1;

        private int dstPoly = -1;

        private navVec3 srcVec = new navVec3();

        private navVec3 dstVec = new navVec3();

        private List<navVec3> nvs = new List<navVec3>();

        private navVec3 sv1 = new navVec3();

        private navVec3 sv2 = new navVec3();

        private navVec3 sv3 = new navVec3();

        private navVec3 sv4 = new navVec3();

        internal class ComparePFNodeMatrix : IComparer<int>
        {
            public ComparePFNodeMatrix(PathFinderFast.PathFinderNodeFast[] matrix)
            {
                this.mMatrix = matrix;
            }

            public int Compare(int a, int b)
            {
                if (this.mMatrix[a].F > this.mMatrix[b].F)
                {
                    return 1;
                }
                if (this.mMatrix[a].F < this.mMatrix[b].F)
                {
                    return -1;
                }
                return 0;
            }

            private PathFinderFast.PathFinderNodeFast[] mMatrix;
        }
    }
}
