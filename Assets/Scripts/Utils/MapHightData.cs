using System;
using System.IO;
using GraphicsClient;

public class MapHightData
{
    public int width
    {
        get
        {
            return this.m_width;
        }
        set
        {
            this.m_width = value;
        }
    }

    public int height
    {
        get
        {
            return this.m_height;
        }
        set
        {
            this.m_height = value;
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
            return this.m_cellSizeX;
        }
        set
        {
            this.m_cellSizeX = value;
        }
    }

    public float CellSizeZ
    {
        get
        {
            return this.m_cellSizeZ;
        }
        set
        {
            this.m_cellSizeZ = value;
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

    public int CellNumZ
    {
        get
        {
            return this.m_cellNumZ;
        }
        set
        {
            this.m_cellNumZ = value;
        }
    }

    public float maxHeightDiff
    {
        get
        {
            return this.m_maxHeightDiff;
        }
        set
        {
            this.m_maxHeightDiff = value;
        }
    }

    public float minHeight
    {
        get
        {
            return this.m_minHeight;
        }
        set
        {
            this.m_minHeight = value;
        }
    }

    public float originX
    {
        get
        {
            return this.m_originX;
        }
        set
        {
            this.m_originX = value;
        }
    }

    public float originZ
    {
        get
        {
            return this.m_originZ;
        }
        set
        {
            this.m_originZ = value;
        }
    }

    public float fOffsetX
    {
        get
        {
            return this.m_fOffsetX;
        }
        set
        {
            this.m_fOffsetX = value;
        }
    }

    public float fOffsetZ
    {
        get
        {
            return this.m_fOffsetZ;
        }
        set
        {
            this.m_fOffsetZ = value;
        }
    }

    public ushort[,] datas
    {
        get
        {
            return this.m_datas;
        }
        set
        {
            this.m_datas = value;
        }
    }

    public void Init(int width, int height, float realWidth, float realHeight, float fOffsetX, float fOffsetZ, float maxHeightDiff, float minHeight, BinaryReader br)
    {
        this.m_width = width;
        this.m_height = height;
        this.m_realWidth = realWidth;
        this.m_realHeight = realHeight;
        this.m_fOffsetX = fOffsetX;
        this.m_fOffsetZ = fOffsetZ;
        this.m_maxHeightDiff = maxHeightDiff;
        this.minHeight = minHeight;
        this.m_cellSizeX = this.m_realWidth / (float)this.m_width;
        this.m_cellSizeZ = this.m_realHeight / (float)this.m_height;
        this.m_cellNumX = this.m_width - 1;
        this.m_cellNumZ = this.m_height - 1;
        this.m_originX = -this.m_realWidth * 0.5f + this.m_fOffsetX;
        this.m_originZ = -this.m_realHeight * 0.5f + this.m_fOffsetZ;
        this.m_datas = new ushort[this.m_height, this.m_width];
        for (int i = 0; i < this.m_height; i++)
        {
            for (int j = 0; j < this.m_width; j++)
            {
                this.m_datas[i, j] = br.ReadUInt16();
            }
        }
        br.Close();
    }

    public void CombineHeightData(int top, int left, int bottom, int right, ushort[] datas)
    {
        int num = right - left;
        for (int i = top; i <= bottom; i++)
        {
            for (int j = left; j <= right; j++)
            {
                this.m_datas[i, j] = datas[(i - top) * num + (j - left)];
            }
        }
    }

    public float GetMapHeight(float x, float z)
    {
        x -= this.m_originX;
        z -= this.m_originZ;
        if (x > this.m_realWidth || z > this.m_realHeight)
        {
            return 0f;
        }
        int num = (int)(x / this.m_realWidth * (float)this.m_cellNumX);
        if (num >= this.m_width)
        {
            num = this.m_width - 1;
        }
        int num2 = (int)(z / this.m_realHeight * (float)this.m_cellNumZ);
        if (num2 >= this.m_height)
        {
            num2 = this.m_height - 1;
        }
        float num3 = this.m_realWidth * (float)num / (float)(this.m_width - 1);
        float num4 = this.m_realHeight * (float)num2 / (float)(this.m_height - 1);
        if (Math.Abs(num3 - x) < 1.401298E-45f && Math.Abs(num4 - x) < 1.401298E-45f)
        {
            return this.GetVetexHeight(num2, num);
        }
        float result = 0f;
        if (x - num3 + z - num4 <= this.CellSizeX)
        {
            Vector2D vector2D = new Vector2D(num3, num4);
            Vector2D vector2D2 = new Vector2D(num3, num4 + this.m_cellSizeZ);
            Vector2D vector2D3 = new Vector2D(num3 + this.m_cellSizeX, num4);
            Vector2D vector2D4 = new Vector2D(x, z);
            float num5 = 0f;
            Vector2D zero = Vector2D.Zero;
            if (GraphUtils.LineIntersection2D(vector2D, vector2D4, vector2D2, vector2D3, ref num5, ref zero))
            {
                float vetexHeight = this.GetVetexHeight(num2, num);
                float num6 = Vector2D.Distance(zero, vector2D2) / Vector2D.Distance(vector2D3, vector2D2);
                float vetexHeight2 = this.GetVetexHeight(num2 + 1, num);
                float vetexHeight3 = this.GetVetexHeight(num2, num + 1);
                float num7 = vetexHeight2 * (1f - num6) + num6 * vetexHeight3;
                float num8 = Vector2D.Distance(vector2D, vector2D4) / Vector2D.Distance(vector2D, zero);
                result = vetexHeight * (1f - num8) + num8 * num7;
            }
        }
        else
        {
            Vector2D vector2D5 = new Vector2D(num3 + this.m_cellSizeX, num4 + this.m_cellSizeZ);
            Vector2D vector2D6 = new Vector2D(num3, num4 + this.m_cellSizeZ);
            Vector2D vector2D7 = new Vector2D(num3 + this.m_cellSizeX, num4);
            Vector2D vector2D8 = new Vector2D(x, z);
            float num9 = 0f;
            Vector2D zero2 = Vector2D.Zero;
            if (GraphUtils.LineIntersection2D(vector2D5, vector2D8, vector2D6, vector2D7, ref num9, ref zero2))
            {
                float vetexHeight = this.GetVetexHeight(num2 + 1, num + 1);
                float num10 = Vector2D.Distance(zero2, vector2D6) / Vector2D.Distance(vector2D7, vector2D6);
                num10 = GraphUtils.Clamp01(num10);
                float vetexHeight4 = this.GetVetexHeight(num2 + 1, num);
                float vetexHeight5 = this.GetVetexHeight(num2, num + 1);
                float num11 = vetexHeight4 * (1f - num10) + num10 * vetexHeight5;
                float num12 = Vector2D.Distance(vector2D5, vector2D8) / Vector2D.Distance(vector2D5, zero2);
                num12 = GraphUtils.Clamp01(num12);
                result = vetexHeight * (1f - num12) + num12 * num11;
            }
        }
        return result;
    }

    public float GetVetexHeight(int row, int column)
    {
        float result = 0f;
        try
        {
            result = (float)this.m_datas[row, column] * this.m_maxHeightDiff / 65535f + this.minHeight;
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, string.Concat(new object[]
            {
                "GetVetexHeight Exception  row : ",
                row,
                " column : ",
                column
            }));
        }
        return result;
    }

    private int m_width = -1;

    private int m_height = -1;

    private float m_realWidth = -1f;

    private float m_realHeight = -1f;

    private float m_cellSizeX;

    private float m_cellSizeZ;

    private int m_cellNumX = -1;

    private int m_cellNumZ = -1;

    private float m_maxHeightDiff = -1f;

    private float m_minHeight = -1f;

    private float m_originX;

    private float m_originZ;

    private float m_fOffsetX;

    private float m_fOffsetZ;

    private ushort[,] m_datas;
}
