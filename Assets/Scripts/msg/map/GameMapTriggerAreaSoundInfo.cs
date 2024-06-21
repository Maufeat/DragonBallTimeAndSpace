using System;
using System.Collections.Generic;
using Net;

namespace map
{
    public class GameMapTriggerAreaSoundInfo : StructData
    {
        public override OctetsStream WriteData(OctetsStream ot)
        {
            ot.marshal_int(this.list.Count);
            for (int i = 0; i < this.list.Count; i++)
            {
                this.list[i].WriteData(ot);
            }
            return ot;
        }

        public override OctetsStream ReadData(OctetsStream ot)
        {
            this.list.Clear();
            int num = ot.unmarshal_int();
            for (int i = 0; i < num; i++)
            {
                TriggerAreaInfo triggerAreaInfo = new TriggerAreaInfo();
                this.list.Add(triggerAreaInfo);
                triggerAreaInfo.ReadData(ot);
            }
            return ot;
        }

        public List<TriggerAreaInfo> list = new List<TriggerAreaInfo>();
    }
}
