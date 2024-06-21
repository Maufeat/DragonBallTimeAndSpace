using System;
using Net;

namespace map
{
    public class NewTile : StructData
    {
        public override OctetsStream ReadData(OctetsStream os)
        {
            this.uFlag = (byte)os.unmarshal_short();
            return os;
        }

        public override OctetsStream WriteData(OctetsStream os)
        {
            os.marshal_short((short)this.uFlag);
            return os;
        }

        public void UpdateTileInfo(object typeVaule)
        {
            int num = (int)typeVaule;
            this.uFlag = (byte)num;
        }

        public byte uFlag;
    }
}
