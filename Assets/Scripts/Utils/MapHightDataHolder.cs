using System;

public sealed class MapHightDataHolder
{
    public static MapHightData CurMapData
    {
        get
        {
            return MapHightDataHolder.m_cur_mapData;
        }
        set
        {
            MapHightDataHolder.m_cur_mapData = value;
        }
    }

    public static float GetMapHeight(float x, float z)
    {
        float result = 0f;
        if (MapHightDataHolder.m_cur_mapData != null)
        {
            result = MapHightDataHolder.m_cur_mapData.GetMapHeight(x, z);
        }
        return result;
    }

    private static MapHightData m_cur_mapData;

    public static float FlyMapHeight;
}
