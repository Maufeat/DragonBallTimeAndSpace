using System;
using System.Collections.Generic;
using map;

public sealed class CellInfos
{
    public static byte[,] MapInfos
    {
        get
        {
            return CellInfos.infos;
        }
    }

    public static int TerrainWdith { get; set; }

    public static int TerrainHeight { get; set; }

    public static void FillCellInfos(CopyMapNewTile[] tiles, bool ignoreBlockBilding = false)
    {
        if (tiles != null)
        {
            int num = tiles.Length;
            for (int i = 0; i < num; i++)
            {
                CopyMapNewTile copyMapNewTile = tiles[i];
                if (copyMapNewTile.uFlag == 1 && ignoreBlockBilding)
                {
                    CellInfos.infos[(int)copyMapNewTile.y, (int)copyMapNewTile.x] = 0;
                }
                else
                {
                    CellInfos.infos[(int)copyMapNewTile.y, (int)copyMapNewTile.x] = (byte)copyMapNewTile.uFlag;
                }
            }
        }
    }

    public static void FillCellInfosTile(byte[] tiles, bool ignoreBlockBilding = false)
    {
        if (tiles != null)
        {
            int num = tiles.Length;
            int max = CellInfos.GetMax(LSingleton<CurrentMapAccesser>.Instance.CellNumX);
            int max2 = CellInfos.GetMax(LSingleton<CurrentMapAccesser>.Instance.CellNumY);
            for (int i = 0; i < max2; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    CellInfos.infos[i, j] = 0;
                    if (j >= LSingleton<CurrentMapAccesser>.Instance.CellNumX || i >= LSingleton<CurrentMapAccesser>.Instance.CellNumY)
                    {
                        CellInfos.infos[i, j] = byte.MaxValue;
                    }
                    else
                    {
                        byte b = tiles[i * LSingleton<CurrentMapAccesser>.Instance.CellNumX + j];
                        if (ignoreBlockBilding && b == 1)
                        {
                            CellInfos.infos[i, j] = 0;
                        }
                        else
                        {
                            CellInfos.infos[i, j] = b;
                        }
                    }
                }
            }
        }
    }

    public static int GetInfoUpperBound(int dimension)
    {
        if (dimension == 0)
        {
            return LSingleton<CurrentMapAccesser>.Instance.CellNumY - 1;
        }
        return LSingleton<CurrentMapAccesser>.Instance.CellNumX - 1;
    }

    public static int GetInfoLength(int dimension)
    {
        if (dimension == 0)
        {
            return LSingleton<CurrentMapAccesser>.Instance.CellNumY;
        }
        return LSingleton<CurrentMapAccesser>.Instance.CellNumX;
    }

    private static int GetMax(int v)
    {
        for (int i = 0; i < CellInfos.maxs.Length; i++)
        {
            if (CellInfos.maxs[i] >= v)
            {
                return CellInfos.maxs[i];
            }
        }
        return CellInfos.maxs[0];
    }

    public static void CombineCellInfos(List<NewTile> buildingtiles)
    {
        if (buildingtiles != null)
        {
            for (int i = 0; i < LSingleton<CurrentMapAccesser>.Instance.CellNumY; i++)
            {
                for (int j = 0; j < LSingleton<CurrentMapAccesser>.Instance.CellNumX; j++)
                {
                    NewTile newTile = buildingtiles[j * LSingleton<CurrentMapAccesser>.Instance.CellNumY + i];
                    CellInfos.infos[i, j] = ((byte)(infos[i, j] | newTile.uFlag));
                }
            }
        }
    }

    public static void CombineCellInfos(GameMapInfo buildingInfo)
    {
        int uBuildingLeftTopX = (int)buildingInfo.uBuildingLeftTopX;
        int uBuildingLeftTopY = (int)buildingInfo.uBuildingLeftTopY;
        int uBuildingRightBottomX = (int)buildingInfo.uBuildingRightBottomX;
        int uBuildingRightBottomY = (int)buildingInfo.uBuildingRightBottomY;
        int num = (int)(buildingInfo.uBuildingRightBottomY - buildingInfo.uBuildingLeftTopY + 1U);
        for (int i = 0; i <= uBuildingRightBottomY - uBuildingLeftTopY; i++)
        {
            for (int j = 0; j <= uBuildingRightBottomX - uBuildingLeftTopX; j++)
            {
                int num2 = j + uBuildingLeftTopX;
                int num3 = i + uBuildingLeftTopY;
                int num4 = i * (uBuildingRightBottomX - uBuildingLeftTopX + 1) + j;
                CellInfos.infos[num3, num2] = ((byte)(infos[num3, num2] | buildingInfo.nodes[num4]));
            }
        }
    }

    public static void SeperateCellInfos(GameMapInfo buildingInfo)
    {
        int uBuildingLeftTopX = (int)buildingInfo.uBuildingLeftTopX;
        int uBuildingLeftTopY = (int)buildingInfo.uBuildingLeftTopY;
        int uBuildingRightBottomX = (int)buildingInfo.uBuildingRightBottomX;
        int uBuildingRightBottomY = (int)buildingInfo.uBuildingRightBottomY;
        int num = (int)(buildingInfo.uBuildingRightBottomY - buildingInfo.uBuildingLeftTopY + 1U);
        for (int i = 0; i <= uBuildingRightBottomY - uBuildingLeftTopY; i++)
        {
            for (int j = 0; j <= uBuildingRightBottomX - uBuildingLeftTopX; j++)
            {
                int num2 = j + uBuildingLeftTopX;
                int num3 = i + uBuildingLeftTopY;
                int num4 = i * (uBuildingRightBottomX - uBuildingLeftTopX + 1) + j;
                CellInfos.infos[num3, num2] = ((byte)(infos[num3, num2] & ~buildingInfo.nodes[num4]));
            }
        }
    }

    public static byte GetFlagByRowAndColumn(uint row, uint column)
    {
        if (CellInfos.infos == null)
        {
            throw new NullReferenceException();
        }
        if ((ulong)row < (ulong)((long)LSingleton<CurrentMapAccesser>.Instance.CellNumX) && (ulong)column < (ulong)((long)LSingleton<CurrentMapAccesser>.Instance.CellNumY))
        {
            return CellInfos.infos[(int)((UIntPtr)column), (int)((UIntPtr)row)];
        }
        throw new IndexOutOfRangeException();
    }

    public static void SetCellInfoDynamicObstacleFlag(uint row, uint column, bool dynamicFlag)
    {
        byte flagByRowAndColumn = CellInfos.GetFlagByRowAndColumn(row, column);
        byte b = flagByRowAndColumn;
        byte b2 = 1;
        if (dynamicFlag)
        {
            if ((b & b2) == 0)
            {
                b |= b2;
            }
        }
        else if ((b & b2) != 0)
        {
            b &= (byte)~b2;
        }
        CellInfos.infos[(int)((UIntPtr)row), (int)((UIntPtr)column)] = b;
    }

    private static byte[,] infos = new byte[5000, 5000];

    private static int[] maxs = new int[]
    {
        64,
        128,
        256,
        512,
        1024,
        2048,
        3000,
        4000,
        5000
    };
}
