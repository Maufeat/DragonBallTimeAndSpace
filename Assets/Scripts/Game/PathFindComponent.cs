using System;
using System.Collections.Generic;
using Algorithms;
using Framework.Managers;
using lighttool.NavMesh;
using ResoureManager;
using UnityEngine;

public class PathFindComponent : IFFComponent
{
    public static event Action OnAstarChangeMapInfo;

    public Action OnPathFindEnd
    {
        get
        {
            return this.OnPathFindEnd_;
        }
        set
        {
            this.OnPathFindEnd_ = value;
        }
    }

    public static void InitPathFind()
    {
        PathFindComponent.mPathFinder = null;
        PathFindComponent.mPathFinder = new PathFinderFast(CellInfos.MapInfos);
        PathFindComponent.ConfigNearDistance = Const.DistNpcVisitResponse;
        if (PathFindComponent.OnAstarChangeMapInfo != null)
        {
            PathFindComponent.OnAstarChangeMapInfo();
        }
    }

    public static void RefreshMapData()
    {
        if (PathFindComponent.mPathFinder != null)
        {
            PathFindComponent.mPathFinder.RefreshData(CellInfos.MapInfos);
            if (PathFindComponent.OnAstarChangeMapInfo != null)
            {
                PathFindComponent.OnAstarChangeMapInfo();
            }
        }
    }

    public void BeginFindPath(Vector2 destination, PathFindComponent.AutoMoveState autoMoveState, float nearDistance, Action onpathfindend = null, CheckBlockHandle checkblockhandle = null, float npcRadius = 0f)
    {
        MainPlayer.Self.Pfc.findPathEndPos = new Vector3?(GraphUtils.GetWorldPosByServerPos(destination));
        MainPlayer.Self.Pfc.visitNpcRadius = npcRadius;
        if (autoMoveState == PathFindComponent.AutoMoveState.MoveToPointByServerPos)
        {
            if (CommonTools.IsInDistance(this.Owner.CurrServerPos, destination, nearDistance))
            {
                this.OnPathFindEnd = onpathfindend;
                this.EndPathFind(PathFindState.Complete, true);
                return;
            }
            this.Offset.x = destination.x - (float)Mathf.FloorToInt(destination.x);
            this.Offset.y = destination.y - (float)Mathf.FloorToInt(destination.y);
            this.BeginFindPath(new Point(Mathf.FloorToInt(this.Owner.CurrServerPos.x), Mathf.FloorToInt(this.Owner.CurrServerPos.y)), new Point(Mathf.FloorToInt(destination.x), Mathf.FloorToInt(destination.y)), autoMoveState, onpathfindend, checkblockhandle);
        }
        else
        {
            if (CommonTools.IsInDistance(this.Owner.CurrentPosition2D, destination, nearDistance))
            {
                this.OnPathFindEnd = onpathfindend;
                this.EndPathFind(PathFindState.Complete, true);
                return;
            }
            this.Offset.x = destination.x - (float)Mathf.FloorToInt(destination.x);
            this.Offset.y = destination.y - (float)Mathf.FloorToInt(destination.y);
            this.InitMouseMoveEffect(GraphUtils.GetWorldPosByServerPos(destination), autoMoveState, delegate
            {
                this.BeginFindPath(new Point(Mathf.FloorToInt(this.Owner.NextPosition2D.x), Mathf.FloorToInt(this.Owner.NextPosition2D.y)), new Point(Mathf.FloorToInt(destination.x), Mathf.FloorToInt(destination.y)), autoMoveState, onpathfindend, checkblockhandle);
            });
        }
    }

    public void BeginFindPath(Vector2 destination, PathFindComponent.AutoMoveState autoMoveState, Action onpathfindend = null, CheckBlockHandle checkblockhandle = null)
    {
        this.BeginFindPath(destination, autoMoveState, 0f, onpathfindend, checkblockhandle, 0f);
    }

    public void PathFindToNpc(uint npcid, float posx, float posy, Action callbackwhenreach = null, Action callbakcwhenbreak = null)
    {
        FFDetectionNpcControl component = this.Owner.GetComponent<FFDetectionNpcControl>();
        if (component == null)
        {
            return;
        }
        component.PriorityVisiteNPCID = (ulong)npcid;
        Vector2 destination = new Vector2(posx, posy);
        PathFindComponent.CurrentPathFindNpc = npcid;
        this.PathFindOfDeviation(destination, delegate
        {
            if (CommonTools.IsInDistance(this.Owner.CurrentPosition2D, destination, PathFindComponent.ConfigNearDistance))
            {
                FFDebug.Log(this, FFLogType.Pathfind, "PathFind Reach");
                if (callbackwhenreach != null)
                {
                    callbackwhenreach();
                }
            }
            else
            {
                FFDebug.Log(this, FFLogType.Pathfind, "PathFind Break");
                if (callbakcwhenbreak != null)
                {
                    callbakcwhenbreak();
                }
            }
        });
    }

    public void PathFindOfDeviation(Vector2 destination, Action callback = null)
    {
        if (CommonTools.IsInDistance(this.Owner.CurrentPosition2D, destination, PathFindComponent.ConfigNearDistance))
        {
            this.OnPathFindEnd = callback;
            this.EndPathFind(PathFindState.Complete, true);
            return;
        }
        this.BeginFindPath(this.GetRandomDestination(destination), PathFindComponent.AutoMoveState.MoveToTaskNpc, 0f, delegate ()
        {
            if (callback != null)
            {
                callback();
            }
        }, new CheckBlockHandle(GraphUtils.IsBlockPointForPathfind), 0f);
    }

    private Vector2 GetRandomDestination(Vector2 destination)
    {
        int num = 10;
        Vector2 zero = Vector2.zero;
        while (num-- >= 0)
        {
            Vector2 vector = this.Owner.CurrentPosition2D - destination;
            float num2 = Mathf.Atan2(vector.y, vector.x);
            float num3 = UnityEngine.Random.Range(-0.7853982f, 0.7853982f);
            num3 += num2;
            float num4;
            if (PathFindComponent.CurrentPathFindNpc != 0U)
            {
                num4 = LuaConfigManager.GetConfigTable("npc_data", (ulong)PathFindComponent.CurrentPathFindNpc).GetField_Uint("volume");
            }
            else
            {
                num4 = UnityEngine.Random.Range(1f, 2f);
            }
            PathFindComponent.CurrentPathFindNpc = 0U;
            zero.x = num4 * Mathf.Cos(num3);
            zero.y = num4 * Mathf.Sin(num3);
            if (this.IsFreePoint(zero.x + destination.x, zero.y + destination.y))
            {
                break;
            }
        }
        destination += zero;
        return destination;
    }

    private void BeginFindPath(Point scrp, Point dstp, PathFindComponent.AutoMoveState autoMoveState, Action onEndPathFind = null, CheckBlockHandle checkblockhandle = null)
    {
        if (MainPlayer.Self == this.Owner && !MainPlayer.Self.IsLive)
        {
            return;
        }
        this.EnterState(PathFindState.Start);
        if (autoMoveState == PathFindComponent.AutoMoveState.Off)
        {
            return;
        }
        this.CurDestPoint = dstp;
        this.OnPathFindEnd = onEndPathFind;
        this.Owner.BaseData.SetCharaVolume((autoMoveState < PathFindComponent.AutoMoveState.MoveToPoint) ? 0f : 0.3f);
        if (scrp.X == dstp.X && scrp.Y == dstp.Y)
        {
            FFDebug.Log(this, FFLogType.Pathfind, "Destpoint equal to Scrpoint");
            this.EndPathFind(PathFindState.Complete, true);
            return;
        }
        if (PathFindComponent.mPathFinder == null)
        {
            this.EnterState(PathFindState.Error);
            FFDebug.LogWarning(this, "PathFinder == null");
            return;
        }
        this.EnterState(PathFindState.Stoping);
        PathFindComponent.mPathFinder.FindPathStop();
        this.EnterState(PathFindState.Finding);
        if (!this.IsFreePoint((float)dstp.X, (float)dstp.Y))
        {
            FFDebug.LogWarning(this, "Destpoint is BLOCK");
            Point bestAccessiblePoint = PathFindComponent.mPathFinder.GetBestAccessiblePoint(scrp, dstp);
            if (bestAccessiblePoint == null)
            {
                FFDebug.LogWarning(this, "Not Found BestAccessiblePoint");
                this.EndPathFind(PathFindState.NotFind, true);
                return;
            }
            dstp = bestAccessiblePoint;
        }
        if (this.isPathfinding)
        {
            this.CurrAutoMoveState = PathFindComponent.AutoMoveState.Off;
        }
        this.Owner.SwitchAutoAttack(false);
        this.CurrAutoMoveState = autoMoveState;
        bool getUseNavMesh = ManagerCenter.Instance.GetManager<ResourceManager>().getUseNavMesh;
        if (getUseNavMesh)
        {
            HashSet<PathFinderNode> hashSet = new HashSet<PathFinderNode>();
            NavMeshPath navMeshPath = new NavMeshPath();
            Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(new Vector2((float)scrp.X, (float)scrp.Y));
            Vector3 worldPosByServerPos2 = GraphUtils.GetWorldPosByServerPos(new Vector2((float)dstp.X, (float)dstp.Y));
            bool flag = NavMesh.CalculatePath(worldPosByServerPos, worldPosByServerPos2, 9, navMeshPath);
            if (flag && navMeshPath.corners.Length > 0)
            {
                Vector3 a = navMeshPath.corners[navMeshPath.corners.Length - 1];
                if ((a - worldPosByServerPos2).x * (a - worldPosByServerPos2).x + (a - worldPosByServerPos2).z * (a - worldPosByServerPos2).z < 0.01f)
                {
                    for (int i = 0; i < navMeshPath.corners.Length; i++)
                    {
                        Vector3 pos = navMeshPath.corners[i];
                        Vector2 serverPosByWorldPos = GraphUtils.GetServerPosByWorldPos(pos, true);
                        hashSet.Add(this.vector32node(serverPosByWorldPos));
                    }
                    Queue<cs_MoveData> astarPathDataQueue = this.Owner.AStarPathDataQueue;
                    lock (astarPathDataQueue)
                    {
                        this.GenStraightenPath(hashSet);
                        this.EnterState(PathFindState.FindComplete);
                        this.startMove();
                    }
                    return;
                }
            }
        }
        LSingleton<ThreadManager>.Instance.RunAsync(delegate
        {
            HashSet<PathFinderNode> hashSet2 = null;
            PathFinderFast obj = PathFindComponent.mPathFinder;
            lock (obj)
            {
                Queue<cs_MoveData> astarPathDataQueue2 = this.Owner.AStarPathDataQueue;
                lock (astarPathDataQueue2)
                {
                    if (this.GenStraightenPath(scrp, dstp))
                    {
                        this.EnterState(PathFindState.FindComplete);
                        LSingleton<ThreadManager>.Instance.RunOnMainThread(new Action(this.startMove));
                        return;
                    }
                }
                if (!this.FindPathByRoad(scrp, dstp, ref hashSet2))
                {
                    hashSet2 = PathFindComponent.mPathFinder.FindPathByNavMesh(scrp, dstp, checkblockhandle);
                }
                this.Owner.AStarPathDataQueue.Clear();
                if (hashSet2 == null)
                {
                    if (PathFindComponent.mPathFinder.Stopped)
                    {
                        LSingleton<ThreadManager>.Instance.RunOnMainThread(delegate ()
                        {
                            this.EndPathFind(PathFindState.NotFind, true);
                        });
                    }
                }
                else
                {
                    Queue<cs_MoveData> astarPathDataQueue3 = this.Owner.AStarPathDataQueue;
                    lock (astarPathDataQueue3)
                    {
                        this.GenStraightenPath(hashSet2);
                        this.EnterState(PathFindState.FindComplete);
                        LSingleton<ThreadManager>.Instance.RunOnMainThread(new Action(this.startMove));
                    }
                }
            }
        });
    }

    private int getClosestPoint(Point point, List<Point> points, List<int> roads, ref Point outPoint)
    {
        if (points.Count <= 0)
        {
            return -1;
        }
        float num = -1f;
        int result = 0;
        for (int i = 0; i < points.Count; i++)
        {
            float num2 = Point.Distance(point, points[i]);
            if (num == -1f || num2 < num)
            {
                num = num2;
                result = i;
            }
        }
        return result;
    }

    private bool FindPathByRoad(Point srcp, Point dstp, ref HashSet<PathFinderNode> AStarPath)
    {
        if (MapLoader.roadInfo == null)
        {
            return false;
        }
        float num = Point.Distance(srcp, dstp);
        if (num < 150f)
        {
            AStarPath = PathFindComponent.mPathFinder.FindPathByNavMesh(srcp, dstp, null);
            return null != AStarPath;
        }
        List<Point> list = new List<Point>();
        List<int> list2 = new List<int>();
        Point point = new Point(0, 0);
        Point point2 = new Point(0, 0);
        int num2 = -1;
        int num3 = -1;
        MapLoader.GetClosestRoadId(srcp, ref point, ref num2);
        if (num2 == -1)
        {
            return false;
        }
        MapLoader.GetClosestRoadId(dstp, ref point2, ref num3);
        if (num3 == -1)
        {
            return false;
        }
        AStarPath = new HashSet<PathFinderNode>();
        this.walknodes.Clear();
        List<int> list3 = new List<int>();
        if (num2 != num3)
        {
            MapLoader.GetDijkstraRoads(num2, num3, list3);
            if (list3.Count <= 0)
            {
                return false;
            }
            for (int i = 0; i < list3.Count; i++)
            {
                if (i == 0)
                {
                    this.getRoadNodes(point, MapLoader.GetLinkPoint(list3[i], list3[i + 1]), list3[i]);
                }
                else if (list3.Count - 1 == i)
                {
                    this.getRoadNodes(MapLoader.GetLinkPoint(list3[i - 1], list3[i]), point2, list3[i]);
                }
                else
                {
                    this.getRoadNodes(MapLoader.GetLinkPoint(list3[i - 1], list3[i]), MapLoader.GetLinkPoint(list3[i], list3[i + 1]), list3[i]);
                }
            }
        }
        else
        {
            this.getRoadNodes(point, point2, num2);
        }
        int num4 = 0;
        if (this.walknodes.Count > 6)
        {
            num4 = 2;
        }
        point.X = this.walknodes[num4].X;
        point.Y = this.walknodes[num4].Y;
        int num5 = this.walknodes.Count - 1;
        if (this.walknodes.Count > 9)
        {
            num5 = this.walknodes.Count - 3;
        }
        point2.X = this.walknodes[num5].X;
        point2.Y = this.walknodes[num5].Y;
        HashSet<PathFinderNode> hashSet = PathFindComponent.mPathFinder.FindPathByNavMesh(srcp, point, null);
        if (hashSet == null)
        {
            return false;
        }
        foreach (PathFinderNode item in hashSet)
        {
            AStarPath.Add(item);
        }
        for (int j = num4; j < num5; j++)
        {
            AStarPath.Add(this.walknodes[j]);
        }
        HashSet<PathFinderNode> hashSet2 = PathFindComponent.mPathFinder.FindPathByNavMesh(point2, dstp, null);
        if (hashSet2 == null)
        {
            return false;
        }
        foreach (PathFinderNode item2 in hashSet2)
        {
            AStarPath.Add(item2);
        }
        return true;
    }

    private void getRoadNodes(Point startpoint, Point endpoint, int roadid)
    {
        List<Point> list = new List<Point>();
        MapLoader.GetPointsInRoad(startpoint, endpoint, roadid, ref list);
        for (int i = 1; i < list.Count; i++)
        {
            Vector3 v = new Vector3((float)list[i].X, (float)list[i].Y, 0f);
            this.walknodes.Add(this.vector32node(v));
        }
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

    private void startMove()
    {
        if (!this.Owner.IsMoving && this.Owner.AStarPathDataQueue.Count > 0)
        {
            cs_MoveData cs_MoveData = this.Owner.AStarPathDataQueue.Dequeue();
            if (GraphUtils.IsBlockPointForMove(cs_MoveData.pos.fx, cs_MoveData.pos.fy))
            {
                this.Owner.AStarPathDataQueue.Clear();
                return;
            }
            ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqMove(cs_MoveData, false, false);
            this.Owner.MoveTo(cs_MoveData);
        }
        this.SetPathToUImap();
        this.SetPathToRader();
        this.Owner.OnAStarPathDataQueueDequeue = new Action(this.SetPathToUImap);
    }

    private void EnterState(PathFindState _type)
    {
        this.FindState = _type;
    }

    public HashSet<PathFinderNode> CastAstarPath(Vector2 destination)
    {
        if (PathFindComponent.mPathFinder == null)
        {
            FFDebug.LogWarning(this, "PathFinder == null");
            return null;
        }
        Point start = new Point(Mathf.FloorToInt(this.Owner.NextPosition2D.x), Mathf.FloorToInt(this.Owner.NextPosition2D.y));
        Point end = new Point(Mathf.FloorToInt(destination.x), Mathf.FloorToInt(destination.y));
        PathFindComponent.mPathFinder.FindPathStop();
        if (!this.IsFreePoint(destination.x, destination.y))
        {
            Point bestAccessiblePoint = PathFindComponent.mPathFinder.GetBestAccessiblePoint(start, end);
            if (bestAccessiblePoint == null)
            {
                return null;
            }
            end = bestAccessiblePoint;
        }
        HashSet<PathFinderNode> result = null;
        PathFinderFast obj = PathFindComponent.mPathFinder;
        lock (obj)
        {
            result = PathFindComponent.mPathFinder.FindPathByNavMesh(start, end, null);
        }
        return result;
    }

    private void GenStraightenPath(HashSet<PathFinderNode> AStarPath)
    {
        this.Owner.AStarPathDataQueue.Clear();
        List<Vector2> list = new List<Vector2>();
        foreach (PathFinderNode pathFinderNode in AStarPath)
        {
            list.Add(new Vector2((float)pathFinderNode.X, (float)pathFinderNode.Y));
        }
        List<Vector2> list2 = new List<Vector2>();
        for (int i = 0; i < list.Count - 1; i++)
        {
            float num = Vector2.Distance(list[i], list[i + 1]);
            for (float num2 = 0f; num2 < num; num2 += this.Owner.MinimunMoveUnit)
            {
                Vector2 item = Vector2.Lerp(list[i], list[i + 1], num2 / num);
                list2.Add(item);
            }
        }
        list2.Add(list[list.Count - 1]);
        this.GenMoveDatas(list2).ForEach(delegate (cs_MoveData x)
        {
            this.Owner.AStarPathDataQueue.Enqueue(x);
        });
    }

    private bool GenStraightenPath(Point start, Point end)
    {
        if (start == end)
        {
            return false;
        }
        this.Owner.AStarPathDataQueue.Clear();
        List<Vector2> list = new List<Vector2>();
        list.Add(new Vector2((float)start.X, (float)start.Y));
        list.Add(new Vector2((float)end.X, (float)end.Y));
        List<Vector2> list2 = new List<Vector2>();
        for (int i = 0; i < list.Count - 1; i++)
        {
            float num = Vector2.Distance(list[i], list[i + 1]);
            for (float num2 = 0f; num2 < num; num2 += this.Owner.MinimunMoveUnit)
            {
                Vector2 item = Vector2.Lerp(list[i], list[i + 1], num2 / num);
                float f = GraphUtils.Keep2DecimalPlaces(item.x);
                float f2 = GraphUtils.Keep2DecimalPlaces(item.y);
                if (GraphUtils.IsBlockPointForMove((int)CellInfos.MapInfos[(int)((UIntPtr)Mathf.FloorToInt(f2)), (int)((UIntPtr)Mathf.FloorToInt(f))]))
                {
                    return false;
                }
                list2.Add(item);
            }
        }
        list2.Add(list[list.Count - 1]);
        this.GenMoveDatas(list2).ForEach(delegate (cs_MoveData x)
        {
            this.Owner.AStarPathDataQueue.Enqueue(x);
        });
        return true;
    }

    public bool CheckBlockBetweenPoint(Point start, Point end)
    {
        if (start == end)
        {
            return false;
        }
        Vector2 a = new Vector2((float)start.X, (float)start.Y);
        Vector2 b = new Vector2((float)end.X, (float)end.Y);
        float num = Vector2.Distance(a, b);
        float cellSizeX = LSingleton<CurrentMapAccesser>.Instance.CellSizeX;
        for (float num2 = 0f; num2 < num; num2 += cellSizeX)
        {
            Vector2 vector = Vector2.Lerp(a, b, num2 / num);
            if (GraphUtils.IsBlockPointForMove(vector.x, vector.y))
            {
                return true;
            }
        }
        return false;
    }

    private List<cs_MoveData> GenMoveDatas(List<Vector2> vs)
    {
        PathStraighten pathStraighten = new PathStraighten();
        List<cs_MoveData> list = new List<cs_MoveData>();
        if (vs.Count >= 2)
        {
            list.Add(pathStraighten.GenMoveData(vs[1], this.Owner.CurrentPosition2D));
        }
        for (int i = 2; i < vs.Count; i++)
        {
            list.Add(pathStraighten.GenMoveData(vs[i], vs[i - 1]));
        }
        return list;
    }

    private void GenStraightenPath(HashSet<PathFinderNode> AStarPath, Vector2 end, PathFindComponent.AutoMoveState autoMoveState)
    {
        this.lastMoveData = null;
        this.findPathEndPos = null;
        this.Owner.AStarPathDataQueue.Clear();
        PathStraighten pathStraighten = new PathStraighten();
        List<cs_MoveData> list = pathStraighten.GenPath(AStarPath, new Vector2(this.Owner.CurrentPosition2D.x, this.Owner.CurrentPosition2D.y));
        if (list != null)
        {
            list.ForEach(delegate (cs_MoveData x)
            {
                this.lastMoveData = x;
                this.Owner.AStarPathDataQueue.Enqueue(x);
            });
        }
        cs_MoveData cs_MoveData = null;
        if (list != null && list.Count > 1)
        {
            cs_MoveData = list[list.Count - 1];
        }
        if (cs_MoveData == null || cs_MoveData.pos.fx != end.x || cs_MoveData.pos.fx != end.y)
        {
            this.lastMoveData = pathStraighten.GenMoveData(end, cs_MoveData);
            this.Owner.AStarPathDataQueue.Enqueue(this.lastMoveData);
        }
        if (this.lastMoveData != null)
        {
        }
    }

    private bool IsFreePoint(float x, float y)
    {
        return x < (float)CellInfos.MapInfos.GetLength(0) && y < (float)CellInfos.MapInfos.GetLength(1) && x >= 0f && y >= 0f && !GraphUtils.IsBlockPointForPathfind(x, y);
    }

    private void GenPathQueue(List<PathFinderNode> AStarPath)
    {
        for (int i = 1; i < AStarPath.Count; i++)
        {
            cs_MoveData cs_MoveData = new cs_MoveData();
            if (i == 1)
            {
                cs_MoveData.dir = CommonTools.GetServerDirByClientDir(new Vector2((float)AStarPath[i].X - this.Owner.NextPosition2D.x, -((float)AStarPath[i].Y - this.Owner.NextPosition2D.y)));
            }
            else
            {
                cs_MoveData.dir = CommonTools.GetServerDirByClientDir(new Vector2((float)(AStarPath[i].X - AStarPath[i - 1].X), (float)(-(float)(AStarPath[i].Y - AStarPath[i - 1].Y))));
            }
            cs_MoveData.pos = default(cs_FloatMovePos);
            cs_MoveData.pos.fx = AStarPath[i].X;
            cs_MoveData.pos.fy = AStarPath[i].Y;
            cs_MoveData.pos.fx = GraphUtils.Keep2DecimalPlaces(cs_MoveData.pos.fx);
            cs_MoveData.pos.fy = GraphUtils.Keep2DecimalPlaces(cs_MoveData.pos.fy);
            this.Owner.AStarPathDataQueue.Enqueue(cs_MoveData);
        }
    }

    private void SetPathToUImap()
    {
        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
        if (controller == null)
        {
            return;
        }
        this.WayPathTmpList.Clear();
        if ((this.CurrAutoMoveState == PathFindComponent.AutoMoveState.MoveToTaskNpc || this.CurrAutoMoveState == PathFindComponent.AutoMoveState.MoveToPointWithoutSign) && this.Owner.AStarPathDataQueue != null && this.Owner.AStarPathDataQueue.Count > 0)
        {
            Queue<cs_MoveData>.Enumerator enumerator = this.Owner.AStarPathDataQueue.GetEnumerator();
            while (enumerator.MoveNext())
            {
                cs_MoveData cs_MoveData = enumerator.Current;
                this.WayPathTmpList.Add(new Vector2(cs_MoveData.pos.fx, cs_MoveData.pos.fy));
            }
            enumerator.Dispose();
        }
        controller.SetWayPath(this.WayPathTmpList);
    }

    private void SetPathToRader()
    {
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("TreasureHuntCtrl.IsNeedPathwayPoint", new object[]
        {
            Util.GetLuaTable("TreasureHuntCtrl")
        });
        if (array == null || array.Length < 1)
        {
            return;
        }
        if (!bool.Parse(array[0].ToString()))
        {
            return;
        }
        List<Vector2> list = new List<Vector2>();
        if (this.Owner.AStarPathDataQueue != null && this.Owner.AStarPathDataQueue.Count > 0)
        {
            Queue<cs_MoveData>.Enumerator enumerator = this.Owner.AStarPathDataQueue.GetEnumerator();
            while (enumerator.MoveNext())
            {
                cs_MoveData cs_MoveData = enumerator.Current;
                list.Add(new Vector2(cs_MoveData.pos.fx, cs_MoveData.pos.fy));
            }
            enumerator.Dispose();
        }
        LuaScriptMgr.Instance.CallLuaFunction("TreasureHuntCtrl.CreatePathFindPoint", new object[]
        {
            Util.GetLuaTable("TreasureHuntCtrl"),
            list
        });
    }

    public void EndPathFind(PathFindState _type = PathFindState.Break, bool needcallback = true)
    {
        this.EnterState(_type);
        this.Offset = Vector2.zero;
        this.CurrAutoMoveState = PathFindComponent.AutoMoveState.Off;
        this.Owner.AStarPathDataQueue.Clear();
        if (this.mouseMoveEffect != null)
        {
            this.mouseMoveEffect.SetActive(false);
        }
        this.SetPathToUImap();
        MainPlayer.Self.OnMoveStateChange(true);
        if (needcallback)
        {
            if (this.OnPathFindEnd != null)
            {
                this.OnPathFindEnd();
            }
            this.OnPathFindEnd = null;
        }
    }

    private void InitMouseMoveEffect(Vector3 pos, PathFindComponent.AutoMoveState autoMoveState, Action callback)
    {
        if (autoMoveState != PathFindComponent.AutoMoveState.MoveToPoint)
        {
            if (this.mouseMoveEffect != null)
            {
                this.mouseMoveEffect.SetActive(false);
            }
            callback();
            return;
        }
        float mapHeight = MapHightDataHolder.GetMapHeight(pos.x, pos.z);
        pos = new Vector3(pos.x, mapHeight, pos.z);
        if (this.mouseMoveEffect != null && this.mouseMoveEffect.transform.Find("guadian/light").GetComponent<Projector>().material != null)
        {
            this.mouseMoveEffect.transform.position = pos;
            this.mouseMoveEffect.SetActive(true);
            callback();
            return;
        }
        ManagerCenter.Instance.GetManager<FFEffectManager>().LoadEffobj("gn_djyd_jiantou", delegate
        {
            ObjectPool<EffectObjInPool> effobj = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj("gn_djyd_jiantou");
            if (effobj != null)
            {
                effobj.GetItemFromPool(delegate (EffectObjInPool OIP)
                {
                    ManagerCenter.Instance.GetManager<FFEffectManager>().SetObjectPoolUnit(10UL, "gn_djyd_jiantou", OIP);
                    foreach (object obj in GameObject.Find("ObjectPoolRoot").transform)
                    {
                        Transform transform = (Transform)obj;
                        if (transform.name == "dianji")
                        {
                            UnityEngine.Object.Destroy(transform.gameObject);
                        }
                    }
                    this.mouseMoveEffect = OIP.ItemObj;
                    this.mouseMoveEffect.name = "dianji";
                    this.mouseMoveEffect.SetLayer(Const.Layer.Effect, true);
                    this.mouseMoveEffect.transform.SetParent(GameObject.Find("ObjectPoolRoot").transform);
                    this.mouseMoveEffect.transform.position = pos;
                });
            }
            callback();
        });
    }

    public bool isPathfinding
    {
        get
        {
            return this._autoMoveState != PathFindComponent.AutoMoveState.Off;
        }
    }

    public PathFindComponent.AutoMoveState CurrAutoMoveState
    {
        get
        {
            return this._autoMoveState;
        }
        set
        {
            this._autoMoveState = value;
            if (this.MoveStateChangedEvent != null)
            {
                this.MoveStateChangedEvent();
            }
        }
    }

    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = (Mgr.Owner as MainPlayer);
    }

    public void CompUpdate()
    {
    }

    public void CompDispose()
    {
    }

    public void ResetComp()
    {
    }

    public bool CheckPath(Vector2 scrp, Vector2 dstp, ref List<PathFinderNode> tempList)
    {
        if (tempList == null)
        {
            return false;
        }
        tempList.Clear();
        if (scrp.x == dstp.x && scrp.y == dstp.y)
        {
            return true;
        }
        bool getUseNavMesh = ManagerCenter.Instance.GetManager<ResourceManager>().getUseNavMesh;
        if (getUseNavMesh)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(scrp);
            Vector3 worldPosByServerPos2 = GraphUtils.GetWorldPosByServerPos(dstp);
            bool flag = NavMesh.CalculatePath(worldPosByServerPos, worldPosByServerPos2, 9, navMeshPath);
            if (flag && navMeshPath.corners.Length > 0)
            {
                Vector3 a = navMeshPath.corners[navMeshPath.corners.Length - 1];
                if ((a - worldPosByServerPos2).x * (a - worldPosByServerPos2).x + (a - worldPosByServerPos2).z * (a - worldPosByServerPos2).z < 0.01f)
                {
                    for (int i = 0; i < navMeshPath.corners.Length; i++)
                    {
                        Vector3 pos = navMeshPath.corners[i];
                        Vector2 serverPosByWorldPos = GraphUtils.GetServerPosByWorldPos(pos, true);
                        tempList.Add(this.vector32node(serverPosByWorldPos));
                    }
                    return true;
                }
            }
        }
        return this.FindPathBySelfNavMesh(new Point(scrp.x.ToInt(), scrp.y.ToInt()), new Point(dstp.x.ToInt(), dstp.y.ToInt()), ref tempList);
    }

    public bool FindPathBySelfNavMesh(Point start, Point end, ref List<PathFinderNode> tempList)
    {
        tempList.Clear();
        if (MapLoader.info == null)
        {
            return false;
        }
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
            return false;
        }
        this.srcPoly = pointsInPoly[0];
        this.dstPoly = pointsInPoly[1];
        navVec3 navVec = this.srcVec.clone();
        navVec3 navVec2 = this.dstVec.clone();
        if (this.srcPoly < 0)
        {
            navVec = PathFindComponent.mPathFinder.GetNavOKPoint(this.srcVec, 10);
            if (navVec == null)
            {
                return false;
            }
            this.srcPoly = navVec.polyId;
        }
        if (this.dstPoly < 0)
        {
            navVec2 = PathFindComponent.mPathFinder.GetNavOKPoint(this.dstVec, 10);
            if (navVec2 == null)
            {
                return false;
            }
            this.dstPoly = navVec2.polyId;
        }
        if (this.srcPoly < 0 || this.dstPoly < 0)
        {
            return false;
        }
        int[] array = pathFinding.calcAStarPolyPath(MapLoader.info, this.srcPoly, this.dstPoly, null, 0.1f);
        if (array == null)
        {
            return false;
        }
        List<navVec3> list = new List<navVec3>(pathFinding.calcWayPoints(MapLoader.info, navVec, navVec2, array, 0.1f));
        list.Insert(0, this.srcVec);
        list.Add(this.dstVec);
        if (list != null)
        {
            Vector3 pos = new Vector3((float)list[0].x, (float)list[0].y, (float)list[0].z);
            Vector3 vector = GraphUtils.GetServerPosByWorldPos_new(pos);
            tempList.Add(this.vector32node(vector));
            Vector3 vector2 = vector;
            for (int i = 1; i < list.Count; i++)
            {
                Vector3 pos2 = new Vector3((float)list[i].x, (float)list[i].y, (float)list[i].z);
                Vector3 v = GraphUtils.GetServerPosByWorldPos(pos2, true);
                if ((int)vector2.x != (int)v.x || (int)vector2.y != (int)v.y)
                {
                    tempList.Add(this.vector32node(v));
                }
            }
            return true;
        }
        return false;
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

    public MainPlayer Owner;

    private Action OnPathFindEnd_;

    public PathFindState FindState;

    public Point CurDestPoint = new Point(0, 0);

    public Vector2 Offset = Vector2.zero;

    private static PathFinderFast mPathFinder;

    private static float ConfigNearDistance;

    public static uint CurrentPathFindNpc;

    private List<PathFinderNode> walknodes = new List<PathFinderNode>();

    private cs_MoveData lastMoveData;

    public Vector3? findPathEndPos;

    public float visitNpcRadius;

    private List<Vector2> WayPathTmpList = new List<Vector2>();

    public GameObject mouseMoveEffect;

    private PathFindComponent.AutoMoveState _autoMoveState;

    public Action MoveStateChangedEvent;

    private int srcPoly = -1;

    private int dstPoly = -1;

    private navVec3 srcVec = new navVec3();

    private navVec3 dstVec = new navVec3();

    public enum AutoMoveState
    {
        Off,
        MoveToAttackNpc,
        MoveToFollowTarget,
        MoveToTaskNpc,
        MoveToPointByServerPos,
        MoveToPoint,
        MoveToPointWithoutSign
    }
}
