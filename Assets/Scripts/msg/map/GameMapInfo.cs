using System;
using Net;

namespace map
{
    public class GameMapInfo : StructData
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
            if (this.nodes != null)
            {
                for (int i = 0; i < this.nodes.Length; i++)
                {
                    ot.marshal_short((short)this.nodes[i]);
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
            uint num = this.uWidth;
            uint num2 = this.uHeight;
            if (this.bBuilding)
            {
                num = this.uBuildingRightBottomX - this.uBuildingLeftTopX + 1U;
                num2 = this.uBuildingRightBottomY - this.uBuildingLeftTopY + 1U;
            }
            this.nodes = new byte[num * num2];
            int num3 = 0;
            while ((long)num3 < (long)((ulong)(num * num2)))
            {
                this.nodes[num3] = (byte)ot.unmarshal_short();
                num3++;
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

        public byte[] nodes;
    }
}
