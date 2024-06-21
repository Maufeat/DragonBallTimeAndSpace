using System;
using Net;

namespace map
{
    public class RoadTile : StructData
    {
        public RoadTile()
        {
        }

        public RoadTile(ushort posx, ushort posy)
        {
            this.x = posx;
            this.y = posy;
        }

        public override OctetsStream ReadData(OctetsStream os)
        {
            this.x = os.unmarshal_ushort();
            this.y = os.unmarshal_ushort();
            return os;
        }

        public override OctetsStream WriteData(OctetsStream os)
        {
            os.marshal_ushort(this.x);
            os.marshal_ushort(this.y);
            return os;
        }

        public void UpdateTileInfo(object typeVaule)
        {
        }

        public ushort x;

        public ushort y;
    }
}
