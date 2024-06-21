using System;
using System.Collections.Generic;
using Framework.Managers;
using GraphicsClient;
using map;
using UnityEngine;

public static class GraphUtils
{
    public static Vector2 GetServerPosByWorldPos(Vector3 pos, bool safety = true)
    {
        Vector2 result = new Vector2(-1f, -1f);
        float x = pos.x;
        float z = pos.z;
        bool flag = LSingleton<CurrentMapAccesser>.Instance.WorldOriginPoint.x < x && LSingleton<CurrentMapAccesser>.Instance.WorldOriginPoint.x + LSingleton<CurrentMapAccesser>.Instance.realWidth > x && LSingleton<CurrentMapAccesser>.Instance.WorldOriginPoint.z > z && LSingleton<CurrentMapAccesser>.Instance.WorldOriginPoint.z - LSingleton<CurrentMapAccesser>.Instance.realHeight < z;
        if (!safety || flag)
        {
            pos -= LSingleton<CurrentMapAccesser>.Instance.WorldOriginPoint;
            pos.z = -pos.z;
            result.x = pos.x * (float)LSingleton<CurrentMapAccesser>.Instance.CellNumX / LSingleton<CurrentMapAccesser>.Instance.realWidth;
            if (result.x >= (float)LSingleton<CurrentMapAccesser>.Instance.CellNumX)
            {
                result.x = (float)(LSingleton<CurrentMapAccesser>.Instance.CellNumX - 1);
            }
            result.y = pos.z * (float)LSingleton<CurrentMapAccesser>.Instance.CellNumY / LSingleton<CurrentMapAccesser>.Instance.realHeight;
            if (result.y >= (float)LSingleton<CurrentMapAccesser>.Instance.CellNumY)
            {
                result.y = (float)(LSingleton<CurrentMapAccesser>.Instance.CellNumY - 1);
            }
        }
        result.x = GraphUtils.Keep2DecimalPlaces(result.x);
        result.y = GraphUtils.Keep2DecimalPlaces(result.y);
        return result;
    }

    public static Vector2 GetServerPosByWorldPos_new(Vector3 pos)
    {
        pos -= LSingleton<CurrentMapAccesser>.Instance.WorldOriginPoint;
        pos.z = -pos.z;
        float x = pos.x / LSingleton<CurrentMapAccesser>.Instance.CellSizeX;
        float y = pos.z / LSingleton<CurrentMapAccesser>.Instance.CellSizeY;
        return new Vector2(x, y);
    }

    public static Vector3 GetWorldPosByServerPos(Vector2 v)
    {
        return GraphUtils.GetWorldPosByServerPos(v.x, v.y);
    }

    public static float GetWorldDistance(Vector3 v1, Vector3 v2)
    {
        return Vector3.Distance(v1, v2);
    }

    public static float GetRealDistanceRate()
    {
        return LSingleton<CurrentMapAccesser>.Instance.realWidth / (float)LSingleton<CurrentMapAccesser>.Instance.CellNumX;
    }

    public static Vector3 GetWorldPosByServerPos(float x, float y)
    {
        Vector3 vector = new Vector3(99999f, 0f, 99999f);
        if (x < (float)LSingleton<CurrentMapAccesser>.Instance.CellNumX && y < (float)LSingleton<CurrentMapAccesser>.Instance.CellNumY)
        {
            float cellSizeX = LSingleton<CurrentMapAccesser>.Instance.CellSizeX;
            float cellSizeY = LSingleton<CurrentMapAccesser>.Instance.CellSizeY;
            vector.x = x * cellSizeX + LSingleton<CurrentMapAccesser>.Instance.CellSizeX / 2f;
            vector.z = y * cellSizeY;
            vector.z = -vector.z;
            vector.z -= LSingleton<CurrentMapAccesser>.Instance.CellSizeY / 2f;
            vector = LSingleton<CurrentMapAccesser>.Instance.WorldOriginPoint + vector;
        }
        return vector;
    }

    public static bool IsContainsFlag(float x, float y, TileFlag flag)
    {
        uint num = (uint)Mathf.FloorToInt(x);
        uint num2 = (uint)Mathf.FloorToInt(y);
        if ((ulong)num >= (ulong)((long)LSingleton<CurrentMapAccesser>.Instance.CellNumX) || (ulong)num2 >= (ulong)((long)LSingleton<CurrentMapAccesser>.Instance.CellNumY))
        {
            throw new IndexOutOfRangeException();
        }
        return GraphUtils.IsFlag((int)CellInfos.GetFlagByRowAndColumn(num, num2), (int)flag);
    }

    public static bool IsBlockPointForMove(float x, float y)
    {
        return GraphUtils.IsContainsFlag(x, y, TileFlag.TILE_BLOCK_NORMAL) || GraphUtils.IsContainsFlag(x, y, TileFlag.TILE_BLOCK_BUILDING);
    }

    public static bool IsBlockPointForMove(int code)
    {
        return (code & 1) != 0 || (code & 8) != 0;
    }

    public static bool IsBlockPointForPathfind(int code)
    {
        if (ManagerCenter.Instance.GetManager<CopyManager>().MCurrentCopyID != 0U)
        {
            if ((code & 1) != 0)
            {
                return true;
            }
        }
        else if ((code & 1) != 0 || (code & 8) != 0)
        {
            return true;
        }
        return false;
    }

    public static bool IsBlockPointForPathfind(float x, float y)
    {
        if (ManagerCenter.Instance.GetManager<CopyManager>().MCurrentCopyID != 0U)
        {
            if (GraphUtils.IsContainsFlag(x, y, TileFlag.TILE_BLOCK_NORMAL))
            {
                return true;
            }
        }
        else if (GraphUtils.IsContainsFlag(x, y, TileFlag.TILE_BLOCK_NORMAL) || GraphUtils.IsContainsFlag(x, y, TileFlag.TILE_BLOCK_BUILDING))
        {
            return true;
        }
        return false;
    }

    public static bool IsContainsFlag(Vector2 pos, TileFlag flag)
    {
        return GraphUtils.IsContainsFlag(pos.x, pos.y, flag);
    }

    public static bool IsContainsFlag(TileFlag code, TileFlag flag)
    {
        return GraphUtils.IsFlag((int)code, (int)flag);
    }

    public static bool IsContainsFlag(int code, TileFlag flag)
    {
        return GraphUtils.IsFlag(code, (int)flag);
    }

    public static bool IsFlag(int code, int flag)
    {
        return (code & flag) != 0;
    }

    public static bool PosIsInMap(Vector2 pos, bool safety = true)
    {
        if (pos.x < 0f || pos.x > (float)LSingleton<CurrentMapAccesser>.Instance.CellNumX)
        {
            if (safety)
            {
                FFDebug.LogWarning("GraphUtils", "out map :" + pos);
            }
            return false;
        }
        if (pos.y < 0f || pos.y > (float)LSingleton<CurrentMapAccesser>.Instance.CellNumY)
        {
            if (safety)
            {
                FFDebug.LogWarning("GraphUtils", "out map :" + pos);
            }
            return false;
        }
        return true;
    }

    public static bool CheckCharactorPosEqual(Vector2 pos1, Vector2 pos2)
    {
        bool result = false;
        if ((int)pos1.x == (int)pos2.x && (int)pos1.y == (int)pos2.y)
        {
            result = true;
        }
        return result;
    }

    public static bool FindPosForRush(Vector2 launchPos, Vector2 targetPos, out Vector2 findedPos, TileFlag Passblock = TileFlag.TILE_BLOCK_IDK, TileFlag Stopblock = TileFlag.TILE_BLOCK_IDK)
    {
        if (Vector2.Distance(launchPos, targetPos) < 0.01f)
        {
            findedPos = launchPos;
            return true;
        }
        if (Vector2.Distance(launchPos, targetPos) >= GraphUtils.MaxRushDis)
        {
            targetPos = launchPos + (targetPos - launchPos).normalized * (GraphUtils.MaxRushDis - 0.1f);
        }
        GraphUtils.SamplePosList.Clear();
        GraphUtils.SampleRadialPassPos(launchPos, targetPos, GraphUtils.SamplePosList);
        Vector3 v = targetPos + (targetPos - launchPos).normalized * GraphUtils.blockThroughDis;
        int count = GraphUtils.SamplePosList.Count;
        GraphUtils.SampleRadialPassPos(targetPos, v, GraphUtils.SamplePosList);
        findedPos = launchPos;
        int num = 0;
        for (int i = 0; i < count; i++)
        {
            Vector2 vector = GraphUtils.SamplePosList[i];
            if (GraphUtils.IsContainsFlag(vector, Passblock))
            {
                break;
            }
            findedPos = vector;
            num = i;
        }
        if (GraphUtils.IsContainsFlag(findedPos, Stopblock))
        {
            bool flag = false;
            for (int j = num; j < GraphUtils.SamplePosList.Count; j++)
            {
                Vector2 vector2 = GraphUtils.SamplePosList[j];
                if (Vector3.Distance(vector2, findedPos) > GraphUtils.blockThroughDis)
                {
                    break;
                }
                if (GraphUtils.IsContainsFlag(vector2, Passblock))
                {
                    break;
                }
                if (!GraphUtils.IsContainsFlag(vector2, Stopblock))
                {
                    findedPos = vector2;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                for (int k = num; k > 0; k--)
                {
                    Vector2 vector3 = GraphUtils.SamplePosList[k];
                    if (!GraphUtils.IsContainsFlag(vector3, Stopblock))
                    {
                        findedPos = vector3;
                        break;
                    }
                }
            }
        }
        return Vector2.Distance(findedPos, targetPos) < 0.01f;
    }

    private static void SampleRadialPassPos(Vector2 srcPos, Vector2 targetPos, List<Vector2> SamplePosList)
    {
        if (SamplePosList == null)
        {
            return;
        }
        SamplePosList.Add(srcPos);
        if (Math.Abs(srcPos.x - targetPos.x) > Math.Abs(srcPos.y - targetPos.y))
        {
            float num = (srcPos.y - targetPos.y) / (srcPos.x - targetPos.x);
            float num2 = srcPos.y - num * srcPos.x;
            if (srcPos.x > targetPos.x)
            {
                for (uint num3 = (uint)srcPos.x; num3 >= targetPos.x; num3 -= 1U)
                {
                    Vector2 vector;
                    vector.x = num3;
                    vector.y = (uint)(num * vector.x + num2);
                    if (!GraphUtils.PosIsInMap(vector, false))
                    {
                        break;
                    }
                    SamplePosList.Add(vector);
                }
            }
            else
            {
                for (uint num4 = (uint)srcPos.x; num4 <= targetPos.x; num4 += 1U)
                {
                    Vector2 vector;
                    vector.x = num4;
                    vector.y = (uint)(num * vector.x + num2);
                    if (!GraphUtils.PosIsInMap(vector, false))
                    {
                        break;
                    }
                    SamplePosList.Add(vector);
                }
            }
        }
        else
        {
            float num5 = (srcPos.x - targetPos.x) / (srcPos.y - targetPos.y);
            float num6 = srcPos.x - num5 * srcPos.y;
            if (srcPos.y > targetPos.y)
            {
                for (uint num7 = (uint)srcPos.y; num7 >= targetPos.y; num7 -= 1U)
                {
                    Vector2 vector2;
                    vector2.y = num7;
                    vector2.x = (uint)(num5 * vector2.y + num6);
                    if (!GraphUtils.PosIsInMap(vector2, false))
                    {
                        break;
                    }
                    SamplePosList.Add(vector2);
                }
            }
            else
            {
                for (uint num8 = (uint)srcPos.y; num8 <= targetPos.y; num8 += 1U)
                {
                    Vector2 vector2;
                    vector2.y = num8;
                    vector2.x = (uint)(num5 * vector2.y + num6);
                    if (!GraphUtils.PosIsInMap(vector2, false))
                    {
                        break;
                    }
                    SamplePosList.Add(vector2);
                }
            }
        }
        SamplePosList.Add(targetPos);
    }

    public static List<TileFlag> GetFlagsByXY(uint x, uint y)
    {
        List<TileFlag> list = null;
        if ((ulong)x >= (ulong)((long)LSingleton<CurrentMapAccesser>.Instance.CellNumX) || (ulong)y >= (ulong)((long)LSingleton<CurrentMapAccesser>.Instance.CellNumY))
        {
            throw new IndexOutOfRangeException();
        }
        byte flagByRowAndColumn = CellInfos.GetFlagByRowAndColumn(x, y);
        string[] names = Enum.GetNames(typeof(TileFlag));
        for (int i = 0; i < names.Length; i++)
        {
            TileFlag tileFlag = (TileFlag)((int)Enum.Parse(typeof(TileFlag), names[i], true));
            int num = (int)tileFlag;
            if ((num & (int)flagByRowAndColumn) != 0)
            {
                if (list == null)
                {
                    list = new List<TileFlag>();
                }
                list.Add(tileFlag);
            }
        }
        return list;
    }

    public static bool LineIntersection2D(Vector2D A, Vector2D B, Vector2D C, Vector2D D, ref float dist, ref Vector2D point)
    {
        float num = (A.y - C.y) * (D.x - C.x) - (A.x - C.x) * (D.y - C.y);
        float num2 = (B.x - A.x) * (D.y - C.y) - (B.y - A.y) * (D.x - C.x);
        if (num2 == 0f)
        {
            return false;
        }
        float num3 = num / num2;
        dist = Vector2D.Distance(A, B) * num3;
        point = A + num3 * (B - A);
        return true;
    }

    public static float Clamp(float value, float min, float max)
    {
        if (value < min)
        {
            value = min;
            return value;
        }
        if (value > max)
        {
            value = max;
        }
        return value;
    }

    public static int Clamp(int value, int min, int max)
    {
        if (value < min)
        {
            value = min;
            return value;
        }
        if (value > max)
        {
            value = max;
        }
        return value;
    }

    public static float Clamp01(float value)
    {
        if (value < 0f)
        {
            return 0f;
        }
        if (value > 1f)
        {
            return 1f;
        }
        return value;
    }

    public static float Keep2DecimalPlaces(float f)
    {
        f *= 100f;
        int num = (int)f;
        return (float)num / 100f;
    }

    public static Vector2 Keep2DecimalPlacesVector2(Vector2 v)
    {
        v.x = GraphUtils.Keep2DecimalPlaces(v.x);
        v.y = GraphUtils.Keep2DecimalPlaces(v.y);
        return v;
    }

    public static Vector2[] GetSegmentThroughCell(Vector2 from, Vector2 to)
    {
        float num = (to.x - from.x) / (to.y - from.x);
        Vector2 vector = new Vector2((float)Mathf.FloorToInt(from.x), (float)Mathf.FloorToInt(from.y));
        Vector2 vector2 = new Vector2((float)Mathf.FloorToInt(to.x), (float)Mathf.FloorToInt(to.y));
        List<Vector2> list = new List<Vector2>();
        int num2 = (int)vector.x;
        while ((float)num2 < vector2.x)
        {
            float f = ((float)num2 - from.x) / num + from.y;
            int num3 = Mathf.FloorToInt(f);
            Vector2 item = new Vector2((float)num2, (float)num3);
            Vector2 item2 = new Vector2((float)num2, (float)(num3 + 1));
            if (!list.Contains(item))
            {
                list.Add(item);
            }
            if (!list.Contains(item2))
            {
                list.Add(item2);
            }
            num2++;
        }
        int num4 = (int)vector.y;
        while ((float)num4 < vector2.y)
        {
            float f2 = num * ((float)num4 - vector.y) + vector.x;
            int num5 = Mathf.FloorToInt(f2);
            Vector2 item3 = new Vector2((float)num5, (float)num4);
            Vector2 item4 = new Vector2((float)(num5 + 1), (float)num4);
            if (!list.Contains(item3))
            {
                list.Add(item3);
            }
            if (!list.Contains(item4))
            {
                list.Add(item4);
            }
            num4++;
        }
        return list.ToArray();
    }

    public const TileFlag MagicBlock = (TileFlag)10;

    public const TileFlag PhysicsBlock = (TileFlag)9;

    private static float blockThroughDis = 2f;

    private static float MaxRushDis = 120f;

    private static List<Vector2> SamplePosList = new List<Vector2>();
}
