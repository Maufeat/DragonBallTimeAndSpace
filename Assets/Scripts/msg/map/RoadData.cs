using System;
using System.Collections.Generic;
using Net;

namespace map
{
    public class RoadData : StructData
    {
        public OctetsStream ReadOldData(OctetsStream ot)
        {
            int num = ot.unmarshal_int();
            for (int i = 0; i < num; i++)
            {
                RoadTile roadTile = new RoadTile();
                ot.unmarshal_struct(roadTile);
                this.nodes.Add(roadTile);
            }
            return ot;
        }

        public override OctetsStream ReadData(OctetsStream ot)
        {
            this.ParentId = ot.unmarshal_short();
            int num = ot.unmarshal_int();
            for (int i = 0; i < num; i++)
            {
                RoadTile roadTile = new RoadTile();
                ot.unmarshal_struct(roadTile);
                this.nodes.Add(roadTile);
            }
            return ot;
        }

        public override OctetsStream WriteData(OctetsStream os)
        {
            os.marshal_short(this.ParentId);
            os.marshal_int(this.nodes.Count);
            for (int i = 0; i < this.nodes.Count; i++)
            {
                this.nodes[i].WriteData(os);
            }
            return os;
        }

        public short ParentId = -1;

        public List<RoadTile> nodes;
    }
}
