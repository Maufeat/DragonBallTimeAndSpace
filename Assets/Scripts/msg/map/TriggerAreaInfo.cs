using System;
using System.Collections.Generic;
using Net;
using UnityEngine;

namespace map
{
    public class TriggerAreaInfo : StringableStructData
    {
        public override OctetsStream WriteData(OctetsStream ot)
        {
            base.MarshalString(ot, this.sound, null);
            ot.marshal_int(this.path.Count);
            for (int i = 0; i < this.path.Count; i++)
            {
                ot.marshal_float(this.path[i].x);
                ot.marshal_float(this.path[i].y);
                ot.marshal_float(this.path[i].z);
            }
            ot.marshal_int(this.triggers.Count);
            for (int j = 0; j < this.triggers.Count; j++)
            {
                this.triggers[j].WriteData(ot);
            }
            return ot;
        }

        public override OctetsStream ReadData(OctetsStream ot)
        {
            this.sound = base.UnmarshalString(ot, null);
            this.path.Clear();
            int num = ot.unmarshal_int();
            for (int i = 0; i < num; i++)
            {
                Vector3 item = default(Vector3);
                item.x = ot.unmarshal_float();
                item.y = ot.unmarshal_float();
                item.z = ot.unmarshal_float();
                this.path.Add(item);
            }
            this.triggers.Clear();
            num = ot.unmarshal_int();
            for (int j = 0; j < num; j++)
            {
                TriggerInfo triggerInfo = new TriggerInfo();
                this.triggers.Add(triggerInfo);
                triggerInfo.ReadData(ot);
            }
            return ot;
        }

        public string sound = string.Empty;

        public List<Vector3> path = new List<Vector3>();

        public List<TriggerInfo> triggers = new List<TriggerInfo>();
    }
}
