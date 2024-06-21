using System;
using System.Collections.Generic;
using map;
using UnityEngine;

namespace Algorithms
{
    #region Structs
    public struct PathFinderNode
    {
        #region Variables Declaration
        public int F;
        public int G;
        public int H;  // f = gone + heuristic
        public int X;
        public int Y;
        public int PX; // Parent
        public int PY;
        #endregion
    }
    #endregion

    #region Enum
    public enum PathFinderNodeType
    {
        Start = 1,
        End = 2,
        Open = 4,
        Close = 8,
        Current = 16,
        Path = 32
    }

    public enum HeuristicFormula
    {
        Manhattan = 1,
        MaxDXDY = 2,
        DiagonalShortCut = 3,
        Euclidean = 4,
        EuclideanNoSQR = 5,
        Custom1 = 6
    }
    #endregion

    #region Delegates
    public delegate void PathFinderDebugHandler(int fromX, int fromY, int x, int y, PathFinderNodeType type, int totalCost, int cost);
    public delegate bool CheckBlockHandle(int i);
    #endregion

    public class PathFinder : IPathFinder
    {
        #region Events
        public event PathFinderDebugHandler PathFinderDebug;
        #endregion

        #region Variables Declaration
        private TileFlag[,] mGrid = null;
        private PriorityQueueB<PathFinderNode> mOpen = new PriorityQueueB<PathFinderNode>(new ComparePFNode());
        private List<PathFinderNode> mClose = new List<PathFinderNode>();
        private bool mStop = false;
        private bool mStopped = true;
        private int mHoriz = 100;
        private HeuristicFormula mFormula = HeuristicFormula.Manhattan;
        private bool mDiagonals = true;
        private int mHEstimate = 10;
        private bool mPunishChangeDirection = true;
        private bool mTieBreaker = false;
        private bool mHeavyDiagonals = false;
        private int mSearchLimit = 20000;
        private double mCompletedTime = 0;
        private bool mDebugProgress = false;
        private bool mDebugFoundPath = false;
        #endregion

        #region Constructors
        public PathFinder(TileFlag[,] grid)
        {
            if (grid == null)
                throw new Exception("Grid cannot be null");

            mGrid = grid;
        }
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
            set { mDiagonals = value; }
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

        public void RefreshData(TileFlag[,] grid)
        {
            if (grid == null)
                throw new Exception("Grid cannot be null");
            mGrid = grid;
        }

        public Point GetBestAccessiblePoint(Point start, Point end)
        {
            Point result = null;
            List<ManhattanPoint> list = new List<ManhattanPoint>();
            bool flag = false;
            int num = 1;
            while (!flag)
            {
                list.Clear();
                int num2 = end.Y + num;
                int num3 = end.Y - num;
                int num4 = end.X - num;
                int num5 = end.X + num;
                for (int i = num4; i <= num5; i++)
                {
                    for (int j = num3; j <= num2; j++)
                    {
                        if (i == num4 || i == num5 || j == num3 || j == num2)
                        {
                            list.Add(new ManhattanPoint(i, j, GetManhattan(new Vector2(i, j), new Vector2(end.X, end.Y))));
                            if (!GraphUtils.IsBlockPointForPathfind((int)mGrid[i, j]))
                            {
                                flag = true;
                            }
                        }
                    }
                }
                num++;
            }
            List<ManhattanPoint> list2 = new List<ManhattanPoint>();
            if (flag)
            {
                foreach (ManhattanPoint manhattanPoint in list)
                {
                    if (!GraphUtils.IsBlockPointForPathfind((int)this.mGrid[manhattanPoint.X, manhattanPoint.Y]))
                    {
                        list2.Add(manhattanPoint);
                    }
                }
                list2.Sort(new CompareWithManhattan());
                result = new Point(list2[0].X, list2[0].Y);
            }
            return result;
        }

        public int GetManhattan(Vector2 p1, Vector2 p2)
        {
            return (int)Math.Abs(p1.x - p2.x) + (int)Math.Abs(p1.y - p2.y);
        }

        public List<PathFinderNode> FindPath(Point start, Point end, CheckBlockHandle checkblockhandle = null)
        {
            bool flag = false;
            int upperBound = this.mGrid.GetUpperBound(0);
            int upperBound2 = this.mGrid.GetUpperBound(1);
            this.mStop = false;
            this.mStopped = false;
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
            sbyte[,] array;
            if (this.mDiagonals)
            {
                array = new sbyte[,]
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
            }
            else
            {
                array = new sbyte[,]
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
                    }
                };
            }
            PathFinderNode item;
            item.G = 0;
            item.H = this.mHEstimate;
            item.F = item.G + item.H;
            item.X = start.X;
            item.Y = start.Y;
            item.PX = item.X;
            item.PY = item.Y;
            this.mOpen.Push(item);
            while (this.mOpen.Count > 0 && !this.mStop)
            {
                item = this.mOpen.Pop();
                if (this.mDebugProgress && this.PathFinderDebug != null)
                {
                    this.PathFinderDebug(0, 0, item.X, item.Y, PathFinderNodeType.Current, -1, -1);
                }
                if (item.X == end.X && item.Y == end.Y)
                {
                    this.mClose.Add(item);
                    flag = true;
                    break;
                }
                if (this.mClose.Count > this.mSearchLimit && this.mSearchLimit != -1)
                {
                    this.mStopped = true;
                    return null;
                }
                if (this.mPunishChangeDirection)
                {
                    this.mHoriz = item.X - item.PX;
                }
                for (int i = 0; i < ((!this.mDiagonals) ? 4 : 8); i++)
                {
                    PathFinderNode item2;
                    item2.X = item.X + (int)array[i, 0];
                    item2.Y = item.Y + (int)array[i, 1];
                    if (item2.X >= 0 && item2.Y >= 0 && item2.X <= upperBound && item2.Y <= upperBound2)
                    {
                        int num;
                        if (this.mHeavyDiagonals && i > 3)
                        {
                            num = item.G + 14;
                        }
                        else
                        {
                            num = item.G + 10;
                        }
                        if (this.mPunishChangeDirection)
                        {
                            if (item2.X - item.X != 0 && this.mHoriz == 0)
                            {
                                num += 20;
                            }
                            if (item2.Y - item.Y != 0 && this.mHoriz != 0)
                            {
                                num += 20;
                            }
                        }
                        if (checkblockhandle != null)
                        {
                            if (checkblockhandle((int)this.mGrid[item2.X, item2.Y]))
                            {
                                goto IL_7F9;
                            }
                        }
                        else if (GraphUtils.IsBlockPointForPathfind((int)this.mGrid[item2.X, item2.Y]))
                        {
                            goto IL_7F9;
                        }
                        int num2 = -1;
                        for (int j = 0; j < this.mOpen.Count; j++)
                        {
                            if (this.mOpen[j].X == item2.X && this.mOpen[j].Y == item2.Y)
                            {
                                num2 = j;
                                break;
                            }
                        }
                        if (num2 == -1 || this.mOpen[num2].G > num)
                        {
                            int num3 = -1;
                            for (int k = 0; k < this.mClose.Count; k++)
                            {
                                if (this.mClose[k].X == item2.X && this.mClose[k].Y == item2.Y)
                                {
                                    num3 = k;
                                    break;
                                }
                            }
                            if (num3 == -1 || this.mClose[num3].G > num)
                            {
                                item2.PX = item.X;
                                item2.PY = item.Y;
                                item2.G = num;
                                switch (this.mFormula)
                                {
                                    default:
                                        item2.H = this.mHEstimate * (Math.Abs(item2.X - end.X) + Math.Abs(item2.Y - end.Y));
                                        break;
                                    case HeuristicFormula.MaxDXDY:
                                        item2.H = this.mHEstimate * Math.Max(Math.Abs(item2.X - end.X), Math.Abs(item2.Y - end.Y));
                                        break;
                                    case HeuristicFormula.DiagonalShortCut:
                                        {
                                            int num4 = Math.Min(Math.Abs(item2.X - end.X), Math.Abs(item2.Y - end.Y));
                                            int num5 = Math.Abs(item2.X - end.X) + Math.Abs(item2.Y - end.Y);
                                            item2.H = this.mHEstimate * 2 * num4 + this.mHEstimate * (num5 - 2 * num4);
                                            break;
                                        }
                                    case HeuristicFormula.Euclidean:
                                        item2.H = (int)((double)this.mHEstimate * Math.Sqrt(Math.Pow((double)(item2.X - end.X), 2.0) + Math.Pow((double)(item2.Y - end.Y), 2.0)));
                                        break;
                                    case HeuristicFormula.EuclideanNoSQR:
                                        item2.H = (int)((double)this.mHEstimate * (Math.Pow((double)(item2.X - end.X), 2.0) + Math.Pow((double)(item2.Y - end.Y), 2.0)));
                                        break;
                                    case HeuristicFormula.Custom1:
                                        {
                                            Point point = new Point(Math.Abs(end.X - item2.X), Math.Abs(end.Y - item2.Y));
                                            int num6 = Math.Abs(point.X - point.Y);
                                            int num7 = Math.Abs((point.X + point.Y - num6) / 2);
                                            item2.H = this.mHEstimate * (num7 + num6 + point.X + point.Y);
                                            break;
                                        }
                                }
                                if (this.mTieBreaker)
                                {
                                    int num8 = item.X - end.X;
                                    int num9 = item.Y - end.Y;
                                    int num10 = start.X - end.X;
                                    int num11 = start.Y - end.Y;
                                    int num12 = Math.Abs(num8 * num11 - num10 * num9);
                                    item2.H = (int)((double)item2.H + (double)num12 * 0.001);
                                }
                                item2.F = item2.G + item2.H;
                                if (this.mDebugProgress && this.PathFinderDebug != null)
                                {
                                    this.PathFinderDebug(item.X, item.Y, item2.X, item2.Y, PathFinderNodeType.Open, item2.F, item2.G);
                                }
                                this.mOpen.Push(item2);
                            }
                        }
                    }
                IL_7F9:;
                }
                this.mClose.Add(item);
                if (this.mDebugProgress && this.PathFinderDebug != null)
                {
                    this.PathFinderDebug(0, 0, item.X, item.Y, PathFinderNodeType.Close, item.F, item.G);
                }
            }
            if (this.mStop && !flag)
            {
                return null;
            }
            if (flag)
            {
                PathFinderNode pathFinderNode = this.mClose[this.mClose.Count - 1];
                for (int l = this.mClose.Count - 1; l >= 0; l--)
                {
                    if ((pathFinderNode.PX == this.mClose[l].X && pathFinderNode.PY == this.mClose[l].Y) || l == this.mClose.Count - 1)
                    {
                        if (this.mDebugFoundPath && this.PathFinderDebug != null)
                        {
                            this.PathFinderDebug(pathFinderNode.X, pathFinderNode.Y, this.mClose[l].X, this.mClose[l].Y, PathFinderNodeType.Path, this.mClose[l].F, this.mClose[l].G);
                        }
                        pathFinderNode = this.mClose[l];
                    }
                    else
                    {
                        this.mClose.RemoveAt(l);
                    }
                }
                this.mStopped = true;
                return this.mClose;
            }
            FFDebug.LogWarning(this, "Path Not Found!");
            this.mStopped = true;
            return null;
        }

        public bool GetPointGraphFlag(float fx, float fy, TileFlag flag)
        {
            return GraphUtils.IsContainsFlag(this.mGrid[Mathf.FloorToInt(fx), Mathf.FloorToInt(fy)], flag);
        }
        #endregion

        public class ManhattanPoint
        {
            public ManhattanPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public ManhattanPoint(int x, int y, int mandis)
            {
                this.X = x;
                this.Y = y;
                this.ManhattanDistance = mandis;
            }

            public int X;

            public int Y;

            public int ManhattanDistance;
        }

        public class CompareWithManhattan : IComparer<ManhattanPoint>
        {
            public int Compare(ManhattanPoint p1, ManhattanPoint p2)
            {
                return p1.ManhattanDistance.CompareTo(p1.ManhattanDistance);
            }
        }

        internal class ComparePFNode : IComparer<PathFinderNode>
        {
            public int Compare(PathFinderNode x, PathFinderNode y)
            {
                if (x.F > y.F)
                {
                    return 1;
                }
                if (x.F < y.F)
                {
                    return -1;
                }
                return 0;
            }
        }
    }
}
