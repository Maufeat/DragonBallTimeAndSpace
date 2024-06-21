using System;
using UnityEngine;

public class CurrentMapAccesser : LSingleton<CurrentMapAccesser>
{
    public Vector3 WorldOriginPoint
    {
        get
        {
            return this.m_WorldOriginPoint;
        }
        set
        {
            this.m_WorldOriginPoint = value;
        }
    }

    public int CellNumX
    {
        get
        {
            return this.m_cellNumX;
        }
        set
        {
            this.m_cellNumX = value;
        }
    }

    public int CellNumY
    {
        get
        {
            return this.m_cellNumY;
        }
        set
        {
            this.m_cellNumY = value;
        }
    }

    public float realWidth
    {
        get
        {
            return this.m_realWidth;
        }
        set
        {
            this.m_realWidth = value;
        }
    }

    public float realHeight
    {
        get
        {
            return this.m_realHeight;
        }
        set
        {
            this.m_realHeight = value;
        }
    }

    public float CellSizeX
    {
        get
        {
            return this.m_fCellSizeX;
        }
        set
        {
            this.m_fCellSizeX = value;
        }
    }

    public float CellSizeY
    {
        get
        {
            return this.m_fCellSizeY;
        }
        set
        {
            this.m_fCellSizeY = value;
        }
    }

    public int BlockCountWidth
    {
        get
        {
            return this.m_nBlockCountWidth;
        }
        set
        {
            this.m_nBlockCountWidth = value;
        }
    }

    public int BlockCountHeight
    {
        get
        {
            return this.m_nBlockCountHeight;
        }
        set
        {
            this.m_nBlockCountHeight = value;
        }
    }

    private Vector3 m_WorldOriginPoint;

    private int m_cellNumX = -1;

    private int m_cellNumY = -1;

    private float m_realWidth;

    private float m_realHeight;

    private float m_fCellSizeX;

    private float m_fCellSizeY;

    private int m_nBlockCountWidth;

    private int m_nBlockCountHeight;
}
