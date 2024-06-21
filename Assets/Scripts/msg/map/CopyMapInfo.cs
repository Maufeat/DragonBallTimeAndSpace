using System;
using Net;

namespace map
{
    public class CopyMapInfo : StructData
    {
        public override OctetsStream WriteData(OctetsStream ot)
        {
            ot.marshal_uint(this.uMagic);
            ot.marshal_uint(this.uVersion);
            ot.marshal_uint(this.uWidth);
            ot.marshal_uint(this.uHeight);
            ot.marshal_uint(this.uRealWidth);
            ot.marshal_uint(this.uRealHeight);
            ot.marshal_boolean(this.bBuilding);
            ot.marshal_float(this.fBuildingPosX);
            ot.marshal_float(this.fBuildingPosY);
            ot.marshal_float(this.fBuildingHeight);
            ot.marshal_uint(this.uBuildingLeftTopX);
            ot.marshal_uint(this.uBuildingLeftTopY);
            ot.marshal_uint(this.uBuildingRightBottomX);
            ot.marshal_uint(this.uBuildingRightBottomY);
            ot.marshal_uint(this.uBuildingRotation);
            ot.marshal_float(this.fBuildingAngleX);
            ot.marshal_float(this.fBuildingAngleY);
            ot.marshal_float(this.fBuildingAngleZ);
            this.uTotalTileCount = 0U;
            for (int i = 0; i < this.nodes.Length; i++)
            {
                if (this.nodes[i].uFlag > 0)
                {
                    this.uTotalTileCount += 1U;
                }
            }
            ot.marshal_uint(this.uTotalTileCount);
            if (this.nodes != null)
            {
                for (int j = 0; j < this.nodes.Length; j++)
                {
                    if (this.nodes[j].uFlag > 0)
                    {
                        ot.marshal_struct(this.nodes[j]);
                    }
                }
            }
            return ot;
        }

        public OctetsStream WriteDataForGameMap(OctetsStream ot)
        {
            ot.marshal_uint(this.uMagic);
            ot.marshal_uint(this.uVersion);
            ot.marshal_uint(this.uWidth);
            ot.marshal_uint(this.uHeight);
            ot.marshal_uint(this.uRealWidth);
            ot.marshal_uint(this.uRealHeight);
            ot.marshal_boolean(this.bBuilding);
            ot.marshal_float(this.fBuildingPosX);
            ot.marshal_float(this.fBuildingPosY);
            ot.marshal_float(this.fBuildingHeight);
            ot.marshal_uint(this.uBuildingLeftTopX);
            ot.marshal_uint(this.uBuildingLeftTopY);
            ot.marshal_uint(this.uBuildingRightBottomX);
            ot.marshal_uint(this.uBuildingRightBottomY);
            ot.marshal_uint(this.uBuildingRotation);
            ot.marshal_float(this.fBuildingAngleX);
            ot.marshal_float(this.fBuildingAngleY);
            ot.marshal_float(this.fBuildingAngleZ);
            if (this.nodes != null)
            {
                for (int i = 0; i < this.nodes.Length; i++)
                {
                    ot.marshal_struct(this.nodes[i]);
                }
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
            this.bBuilding = ot.unmarshal_boolean();
            this.fBuildingPosX = ot.unmarshal_float();
            this.fBuildingPosY = ot.unmarshal_float();
            this.fBuildingHeight = ot.unmarshal_float();
            this.uBuildingLeftTopX = ot.unmarshal_uint();
            this.uBuildingLeftTopY = ot.unmarshal_uint();
            this.uBuildingRightBottomX = ot.unmarshal_uint();
            this.uBuildingRightBottomY = ot.unmarshal_uint();
            this.uBuildingRotation = ot.unmarshal_uint();
            this.fBuildingAngleX = ot.unmarshal_float();
            this.fBuildingAngleY = ot.unmarshal_float();
            this.fBuildingAngleZ = ot.unmarshal_float();
            this.uTotalTileCount = ot.unmarshal_uint();
            uint num = this.uWidth;
            uint num2 = this.uHeight;
            if (this.bBuilding)
            {
                num = this.uBuildingRightBottomX - this.uBuildingLeftTopX + 1U;
                num2 = this.uBuildingRightBottomY - this.uBuildingLeftTopY + 1U;
            }
            this.nodes = new CopyMapNewTile[this.uTotalTileCount];
            int num3 = 0;
            while ((long)num3 < (long)((ulong)this.uTotalTileCount))
            {
                this.nodes[num3] = new CopyMapNewTile();
                num3++;
            }
            int num4 = 0;
            while ((long)num4 < (long)((ulong)this.uTotalTileCount))
            {
                CopyMapNewTile copyMapNewTile = new CopyMapNewTile();
                ot.unmarshal_struct(copyMapNewTile);
                this.nodes[num4] = copyMapNewTile;
                num4++;
            }
            return ot;
        }

        public uint uMagic;

        public uint uVersion;

        public uint uWidth;

        public uint uHeight;

        public uint uRealWidth;

        public uint uRealHeight;

        public bool bBuilding;

        public float fBuildingPosX;

        public float fBuildingPosY;

        public float fBuildingHeight;

        public uint uBuildingLeftTopX;

        public uint uBuildingLeftTopY;

        public uint uBuildingRightBottomX;

        public uint uBuildingRightBottomY;

        public uint uBuildingRotation;

        public float fBuildingAngleX;

        public float fBuildingAngleY;

        public float fBuildingAngleZ;

        public uint uTotalTileCount;

        public CopyMapNewTile[] nodes;
    }
}
