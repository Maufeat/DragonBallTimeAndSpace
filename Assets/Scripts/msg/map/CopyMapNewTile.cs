using System;
using Net;

namespace map
{
    public class CopyMapNewTile : StructData
    {
        public override OctetsStream ReadData(OctetsStream os)
        {
            this.x = os.unmarshal_ushort();
            this.y = os.unmarshal_ushort();
            this.uFlag = os.unmarshal_ushort();
            return os;
        }

        public override OctetsStream WriteData(OctetsStream os)
        {
            os.marshal_ushort(this.x);
            os.marshal_ushort(this.y);
            os.marshal_ushort(this.uFlag);
            return os;
        }

        public void UpdateTileInfo(object typeVaule)
        {
            NodeData nodeData = (NodeData)typeVaule;
            this.x = nodeData.x;
            this.y = nodeData.y;
            this.uFlag = nodeData.flag;
        }

        public ushort x;

        public ushort y;

        public ushort uFlag;
    }
}
