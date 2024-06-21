using System;
using System.Collections.Generic;
using System.IO;
using Algorithms;
using Framework.Managers;
using lighttool.NavMesh;
using map;
using Net;
using ProtoBuf;
using ResoureManager;
using UnityEngine;

public sealed class MapLoader
{
    public static void LoadScene(string name, Action callback)
    {
        string text = string.Concat(new string[]
        {
            "Scenes/",
            name,
            "/scene/",
            name,
            ".u"
        });
        if (FileLocalVersion.IsOpenLocalVersion())
        {
            text = FileLocalVersion.GetLoaclFile(text);
        }
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadScene(text, name, delegate (AseetItem asset1)
        {
            string str = name + ".navmesh";
            string name2 = "CubeToNavmesh";
            string path = "Scenes/MapData/" + str + ".u";
            ManagerCenter.Instance.GetManager<ResourceManager>().LoadNavMeshScene(path, name2, delegate (AseetItem asset2)
            {
                callback();
            });
        });
    }

    private static navMeshInfo LoadMeshInfo(string s)
    {
        MyJson.IJsonNode jsonNode = MyJson.Parse(s);
        navMeshInfo navMeshInfo = new navMeshInfo();
        IDictionary<string, MyJson.IJsonNode> dictionary = jsonNode.asDict();
        IList<MyJson.IJsonNode> list = dictionary["v"].AsList();
        navVec3[] array = new navVec3[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            IList<MyJson.IJsonNode> list2 = list[i].AsList();
            array[i] = new navVec3
            {
                x = (double)((float)list2[0].AsDouble()),
                y = (double)((float)list2[1].AsDouble()),
                z = (double)((float)list2[2].AsDouble())
            };
        }
        navMeshInfo.vecs = array;
        IList<MyJson.IJsonNode> list3 = dictionary["p"].AsList();
        navNode[] array2 = new navNode[list3.Count];
        for (int j = 0; j < list3.Count; j++)
        {
            IList<MyJson.IJsonNode> list4 = list3[j].AsList();
            navNode navNode = new navNode();
            navNode.nodeID = j;
            int[] array3 = new int[list4.Count];
            for (int k = 0; k < list4.Count; k++)
            {
                array3[k] = list4[k].AsInt();
            }
            navNode.poly = array3;
            navNode.genBorder();
            navNode.genCenter(navMeshInfo);
            array2[j] = navNode;
        }
        navMeshInfo.nodes = array2;
        navMeshInfo.calcBound();
        navMeshInfo.genBorder();
        return navMeshInfo;
    }

    private static navMeshInfo LoadMeshInfo(NavMeshData data)
    {
        navMeshInfo navMeshInfo = new navMeshInfo();
        navVec3[] array = new navVec3[data.pos.Count];
        for (int i = 0; i < data.pos.Count; i++)
        {
            array[i] = new navVec3
            {
                x = (double)data.pos[i].x,
                y = (double)data.pos[i].y,
                z = (double)data.pos[i].z
            };
        }
        navMeshInfo.vecs = array;
        int count = data.polys.Count;
        navNode[] array2 = new navNode[count];
        for (int j = 0; j < count; j++)
        {
            navNode navNode = new navNode();
            navNode.nodeID = j;
            int num = data.polys[j].poly.Length;
            int[] array3 = new int[num];
            for (int k = 0; k < num; k++)
            {
                array3[k] = data.polys[j].poly[k];
            }
            navNode.poly = array3;
            navNode.genBorder();
            navNode.genCenter(navMeshInfo);
            array2[j] = navNode;
        }
        navMeshInfo.nodes = array2;
        navMeshInfo.calcBound();
        navMeshInfo.genBorder();
        return navMeshInfo;
    }

    public static void LoadMapConfigData(string name, string copymapblockname, string copymapmeshfindname, Action callback)
    {
        MapLoader.info = null;
        MapLoader.roadInfo = null;
        string strPath = string.Empty;
        if (string.IsNullOrEmpty(copymapmeshfindname))
        {
            strPath = "Scenes/MapData/" + name + ".walkmesh.txt";
        }
        else
        {
            strPath = "Scenes/MapData/" + copymapmeshfindname + ".walkmesh.txt";
        }
        string strPath2 = "Scenes/MapData/" + name + "_road.bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(strPath2, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                MapLoader.roadInfo = new GameMapRoadInfo();
                MapLoader.os.clear();
                MapLoader.os.position(0);
                MapLoader.os.insert(0, bytes);
                MapLoader.roadInfo.ReadData(MapLoader.os);
                MapLoader.dijkstra.InitData(MapLoader.roadInfo.noderelation, MapLoader.roadInfo.nodes.Count);
            }
        });
        MapLoader.soundInfo = null;
        string strPath3 = "Scenes/MapData/" + name + "_sound.bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(strPath3, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                MapLoader.soundInfo = new GameMapSoundInfo();
                MapLoader.os.clear();
                MapLoader.os.position(0);
                MapLoader.os.insert(0, bytes);
                MapLoader.soundInfo.ReadData(MapLoader.os);
            }
        });
        MapLoader.areaMusicInfo = null;
        string strPath4 = "Scenes/MapData/" + name + "_areamusic.bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(strPath4, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                MapLoader.areaMusicInfo = new GameMapAreaMusicInfo();
                MapLoader.os.clear();
                MapLoader.os.position(0);
                MapLoader.os.insert(0, bytes);
                MapLoader.areaMusicInfo.ReadData(MapLoader.os);
            }
        });
        MapLoader.triggerAreaSoundInfo = null;
        string strPath5 = "Scenes/MapData/" + name + "_triggerareasound.bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(strPath5, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                MapLoader.triggerAreaSoundInfo = new GameMapTriggerAreaSoundInfo();
                MapLoader.os.clear();
                MapLoader.os.position(0);
                MapLoader.os.insert(0, bytes);
                MapLoader.triggerAreaSoundInfo.ReadData(MapLoader.os);
            }
            AreaSoundTriggerTool.Instance.ResetTriggers();
        });
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(strPath, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    try
                    {
                        NavMeshData data = Serializer.Deserialize<NavMeshData>(memoryStream);
                        MapLoader.info = MapLoader.LoadMeshInfo(data);
                    }
                    catch (Exception ex)
                    {
                        FFDebug.LogWarning("LoadMeshdata", "Message : " + ex.Message);
                    }
                }
            }
        });
        string path = "Scenes/MapData/" + name + ".bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(path, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                GameMapInfo gameMapInfo = new GameMapInfo();
                MapLoader.os.clear();
                MapLoader.os.position(0);
                MapLoader.os.insert(0, bytes);
                gameMapInfo.ReadData(MapLoader.os);
                int uWidth = (int)gameMapInfo.uWidth;
                int uHeight = (int)gameMapInfo.uHeight;
                CellInfos.TerrainWdith = (int)gameMapInfo.uWidth;
                CellInfos.TerrainHeight = (int)gameMapInfo.uHeight;
                LSingleton<CurrentMapAccesser>.Instance.CellNumX = uWidth;
                LSingleton<CurrentMapAccesser>.Instance.CellNumY = uHeight;
                LSingleton<CurrentMapAccesser>.Instance.BlockCountWidth = 1;
                LSingleton<CurrentMapAccesser>.Instance.BlockCountHeight = 1;
                float num = gameMapInfo.uRealWidth;
                float num2 = gameMapInfo.uRealHeight;
                LSingleton<CurrentMapAccesser>.Instance.realWidth = num;
                LSingleton<CurrentMapAccesser>.Instance.realHeight = num2;
                LSingleton<CurrentMapAccesser>.Instance.CellSizeX = num / (float)uWidth;
                LSingleton<CurrentMapAccesser>.Instance.CellSizeY = num2 / (float)uHeight;
                CellInfos.FillCellInfosTile(gameMapInfo.nodes, name == "Pc_emcs");
                float num3 = 0f;
                float num4 = 0f;
                if (MapHightDataHolder.CurMapData != null)
                {
                    num3 = MapHightDataHolder.CurMapData.fOffsetX;
                    num4 = MapHightDataHolder.CurMapData.fOffsetZ;
                }
                LSingleton<CurrentMapAccesser>.Instance.WorldOriginPoint = new Vector3(-num * 0.5f + num3, 0f, num2 * 0.5f + num4);
                PathFindComponent.InitPathFind();
            }
            else
            {
                FFDebug.LogWarning("MapLoader", "Load Map Block Data Failed! Path: " + path);
            }
            MapLoader.LoadCopyMapBlockConfigData(copymapblockname);
            if (callback != null)
            {
                callback();
            }
        });
    }

    public static void GetDijkstraRoads(int roadstart, int roadend, List<int> roads)
    {
        if (MapLoader.dijkstra == null)
        {
            return;
        }
        MapLoader.dijkstra.FindDijkstra(roadstart, roadend, roads);
    }

    public static void GetPointsInRoad(Point from, Point to, int roadid, ref List<Point> points)
    {
        if (MapLoader.roadInfo == null)
        {
            return;
        }
        int pointIndex = MapLoader.GetPointIndex(from, roadid);
        int pointIndex2 = MapLoader.GetPointIndex(to, roadid);
        int num = (pointIndex - pointIndex2 >= 0) ? -1 : 1;
        points.Add(from);
        for (int num2 = pointIndex; num2 != pointIndex2; num2 += num)
        {
            points.Add(new Point((int)MapLoader.roadInfo.nodes[roadid].nodes[num2].x, (int)MapLoader.roadInfo.nodes[roadid].nodes[num2].y));
        }
        points.Add(to);
    }

    public static int GetPointIndex(Point point, int roadid)
    {
        if (MapLoader.roadInfo == null || roadid >= MapLoader.roadInfo.nodes.Count)
        {
            return -1;
        }
        int result = 0;
        int num = -1;
        for (int i = 0; i < MapLoader.roadInfo.nodes[roadid].nodes.Count; i++)
        {
            int num2 = (int)MapLoader.roadInfo.nodes[roadid].nodes[i].x - point.X;
            int num3 = (int)MapLoader.roadInfo.nodes[roadid].nodes[i].y - point.Y;
            int num4 = num2 * num2 + num3 * num3;
            if (num == -1 || num4 < num)
            {
                result = i;
                num = num4;
            }
        }
        return result;
    }

    public static Point GetLinkPoint(int fromroadid, int toroadid)
    {
        if (MapLoader.roadInfo == null)
        {
            return null;
        }
        return new Point((int)MapLoader.roadInfo.noderelationTile[fromroadid, toroadid].x, (int)MapLoader.roadInfo.noderelationTile[fromroadid, toroadid].y);
    }

    public static void GetClosestRoadId(Point from, ref Point dstPoint, ref int roadid)
    {
        float num = -1f;
        for (int i = 0; i < MapLoader.roadInfo.nodes.Count; i++)
        {
            float num2 = 0f;
            if (MapLoader.GetClosestPoint(from, i, ref MapLoader.closestPoint, ref num2) && num2 != -1f && (num == -1f || num > num2))
            {
                dstPoint.X = MapLoader.closestPoint.X;
                dstPoint.Y = MapLoader.closestPoint.Y;
                roadid = i;
                num = num2;
            }
        }
        if (roadid > 0 && roadid < MapLoader.roadInfo.nodes.Count && MapLoader.roadInfo.nodes[roadid].ParentId >= 0)
        {
            MapLoader.GetClosestPoint(from, (int)MapLoader.roadInfo.nodes[roadid].ParentId, ref dstPoint, ref num);
            roadid = (int)MapLoader.roadInfo.nodes[roadid].ParentId;
        }
    }

    public static bool GetClosestPoint(Point from, int roadid, ref Point point, ref float distance)
    {
        if (MapLoader.roadInfo == null || MapLoader.roadInfo.nodes.Count <= 0 || roadid >= MapLoader.roadInfo.nodes.Count)
        {
            return false;
        }
        distance = -1f;
        for (int i = 0; i < MapLoader.roadInfo.nodes[roadid].nodes.Count; i++)
        {
            if (i + 1 >= MapLoader.roadInfo.nodes[roadid].nodes.Count)
            {
                break;
            }
            MapLoader.verticalRoadStart.X = (int)MapLoader.roadInfo.nodes[roadid].nodes[i].x;
            MapLoader.verticalRoadStart.Y = (int)MapLoader.roadInfo.nodes[roadid].nodes[i].y;
            float num = Point.Distance(from, MapLoader.verticalRoadStart);
            if (distance == -1f || distance > num)
            {
                point.X = MapLoader.verticalRoadStart.X;
                point.Y = MapLoader.verticalRoadStart.Y;
                distance = num;
            }
        }
        return true;
    }

    private static void getClosePoint(int roadid, int startindex, int endindex, Point fromPoint, ref Point outPoint)
    {
        if (startindex == endindex)
        {
            outPoint.X = (int)MapLoader.roadInfo.nodes[roadid].nodes[startindex].x;
            outPoint.Y = (int)MapLoader.roadInfo.nodes[roadid].nodes[startindex].y;
            return;
        }
        int num = 1;
        if (startindex > endindex)
        {
            num = -1;
        }
        float num2 = -1f;
        for (int num3 = startindex; num3 != endindex; num3 += num)
        {
            MapLoader.tmpPointClose.X = (int)MapLoader.roadInfo.nodes[roadid].nodes[num3].x;
            MapLoader.tmpPointClose.Y = (int)MapLoader.roadInfo.nodes[roadid].nodes[num3].y;
            float num4 = Point.Distance(fromPoint, MapLoader.tmpPointClose);
            if (num2 == -1f || num4 < num2)
            {
                outPoint.X = (int)MapLoader.roadInfo.nodes[roadid].nodes[num3].x;
                outPoint.Y = (int)MapLoader.roadInfo.nodes[roadid].nodes[num3].y;
            }
        }
    }

    public static void LoadCopyMapBlockConfigData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        string path = "Scenes/MapData/" + name + ".bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(path, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                CopyMapInfo copyMapInfo = new CopyMapInfo();
                MapLoader.os.clear();
                MapLoader.os.position(0);
                MapLoader.os.insert(0, bytes);
                copyMapInfo.ReadData(MapLoader.os);
                int uWidth = (int)copyMapInfo.uWidth;
                int uHeight = (int)copyMapInfo.uHeight;
                CellInfos.TerrainWdith = (int)copyMapInfo.uWidth;
                CellInfos.TerrainHeight = (int)copyMapInfo.uHeight;
                LSingleton<CurrentMapAccesser>.Instance.CellNumX = uWidth;
                LSingleton<CurrentMapAccesser>.Instance.CellNumY = uHeight;
                LSingleton<CurrentMapAccesser>.Instance.BlockCountWidth = 1;
                LSingleton<CurrentMapAccesser>.Instance.BlockCountHeight = 1;
                float num = copyMapInfo.uRealWidth;
                float num2 = copyMapInfo.uRealHeight;
                LSingleton<CurrentMapAccesser>.Instance.realWidth = num;
                LSingleton<CurrentMapAccesser>.Instance.realHeight = num2;
                LSingleton<CurrentMapAccesser>.Instance.CellSizeX = num / (float)uWidth;
                LSingleton<CurrentMapAccesser>.Instance.CellSizeY = num2 / (float)uHeight;
                CellInfos.FillCellInfos(copyMapInfo.nodes, name == "Pc_emcs");
            }
            else
            {
                FFDebug.LogWarning("MapLoader", "Load Copy Map Block Data Failed! Path: " + path);
            }
        });
    }

    public static void LoadMapConfigData1(string name, Action<GameMapInfo> callback)
    {
        string strPath = "Scenes/MapData/" + name + ".bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(strPath, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                FFDebug.Log("MapLoader", FFLogType.Scene, "LoadMapConfigData complete " + "Scenes/MapData/" + name + ".bytes");
                GameMapInfo gameMapInfo = new GameMapInfo();
                MapLoader.os.clear();
                MapLoader.os.position(0);
                MapLoader.os.insert(0, bytes);
                gameMapInfo.ReadData(MapLoader.os);
                if (callback != null)
                {
                    callback(gameMapInfo);
                }
            }
            else
            {
                callback(null);
            }
        });
    }

    public static void LoadMapHightDataByName(string name, Action callback)
    {
        string path = "Scenes/MapData/" + name + ".bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(path, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    BinaryReader binaryReader = new BinaryReader(memoryStream);
                    int width = binaryReader.ReadInt32();
                    int height = binaryReader.ReadInt32();
                    float realWidth = binaryReader.ReadSingle();
                    float realHeight = binaryReader.ReadSingle();
                    float fOffsetX = binaryReader.ReadSingle();
                    float fOffsetZ = binaryReader.ReadSingle();
                    float maxHeightDiff = binaryReader.ReadSingle();
                    float minHeight = binaryReader.ReadSingle();
                    MapHightData mapHightData = new MapHightData();
                    mapHightData.Init(width, height, realWidth, realHeight, fOffsetX, fOffsetZ, maxHeightDiff, minHeight, binaryReader);
                    MapHightDataHolder.CurMapData = mapHightData;
                }
            }
            else
            {
                FFDebug.LogWarning("MapLoader", "Load Map High Data Failed! Path: " + path);
            }
            if (callback != null)
            {
                callback();
            }
        });
    }

    public static void LoadDisAreaDataByName(string name, Action<MapDisAreaFile> callback)
    {
        string strPath = "Scenes/MapData/" + name + "_disArea.bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(strPath, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    MapDisAreaFile mapDisAreaFile = Serializer.Deserialize<MapDisAreaFile>(memoryStream);
                    if (mapDisAreaFile != null)
                    {
                        callback(mapDisAreaFile);
                    }
                    else
                    {
                        callback(null);
                    }
                }
            }
            else
            {
                callback(null);
            }
        });
    }

    public static void LoadBuildingHightDataByName(string name, Action callback)
    {
        string path = "Scenes/MapData/npc_block/" + name + ".bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(path, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    BinaryReader binaryReader = new BinaryReader(memoryStream);
                    int num = binaryReader.ReadInt32();
                    int num2 = binaryReader.ReadInt32();
                    float num3 = binaryReader.ReadSingle();
                    float num4 = binaryReader.ReadSingle();
                    float num5 = binaryReader.ReadSingle();
                    float num6 = binaryReader.ReadSingle();
                    float num7 = binaryReader.ReadSingle();
                    float num8 = binaryReader.ReadSingle();
                    int num9 = binaryReader.ReadInt32();
                    int num10 = binaryReader.ReadInt32();
                    int num11 = binaryReader.ReadInt32();
                    int num12 = binaryReader.ReadInt32();
                    FFDebug.Log("MapLoader", FFLogType.Scene, "LoadBuildingHightDataByName  " + name);
                    ushort[] array = new ushort[(num11 - num9 + 1) * (num12 - num10 + 1)];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = binaryReader.ReadUInt16();
                    }
                    MapHightDataHolder.CurMapData.CombineHeightData(num10, num9, num12, num11, array);
                    binaryReader.Close();
                }
            }
            else
            {
                FFDebug.LogWarning("MapLoader", "Load Map Building High Data Failed! Path: " + path);
            }
            if (callback != null)
            {
                callback();
            }
        });
    }

    public static void ConbineMapData(string buildingname, Action<GameMapInfo> callback)
    {
        string strPath = "Scenes/MapData/npc_block/" + buildingname + ".bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(strPath, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                GameMapInfo gameMapInfo = new GameMapInfo();
                MapLoader.os.clear();
                MapLoader.os.position(0);
                MapLoader.os.insert(0, bytes);
                gameMapInfo.ReadData(MapLoader.os);
                CellInfos.CombineCellInfos(gameMapInfo);
                FFDebug.Log("MapLoader", FFLogType.Scene, "ConbineMapData  " + buildingname);
                if (callback != null)
                {
                    callback(gameMapInfo);
                }
            }
            else
            {
                callback(null);
            }
        });
    }

    public static void SeperateMapData(string buildingname, Action<GameMapInfo> callback)
    {
        string strPath = "Scenes/MapData/npc_block/" + buildingname + ".bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(strPath, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                GameMapInfo gameMapInfo = new GameMapInfo();
                MapLoader.os.clear();
                MapLoader.os.position(0);
                MapLoader.os.insert(0, bytes);
                gameMapInfo.ReadData(MapLoader.os);
                CellInfos.SeperateCellInfos(gameMapInfo);
                FFDebug.Log("MapLoader", FFLogType.Scene, "SeperateMapData  " + buildingname);
                if (callback != null)
                {
                    callback(gameMapInfo);
                }
            }
            else if (callback != null)
            {
                callback(null);
            }
        });
    }

    public static OctetsStream os = new OctetsStream();

    public static navMeshInfo info;

    public static GameMapRoadInfo roadInfo;

    public static GameMapSoundInfo soundInfo;

    public static GameMapAreaMusicInfo areaMusicInfo;

    public static GameMapTriggerAreaSoundInfo triggerAreaSoundInfo;

    private static Dijkstra dijkstra = new Dijkstra();

    private static Point closestPoint = new Point(0, 0);

    private static Point verticalRoadStart = new Point(0, 0);

    private static Point verticalRoadEnd = new Point(0, 0);

    private static Point verticalpoint = new Point(0, 0);

    private static Point tmpPointClose = new Point(0, 0);
}
