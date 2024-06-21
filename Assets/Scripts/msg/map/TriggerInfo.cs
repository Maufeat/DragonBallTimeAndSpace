using System;
using Net;
using UnityEngine;

namespace map
{
    public class TriggerInfo : StructData
    {
        public TriggerInfo()
        {
        }

        public TriggerInfo(int type)
        {
            this.type = type;
        }

        public override OctetsStream WriteData(OctetsStream ot)
        {
            ot.marshal_int(this.type);
            ot.marshal_float(this.pos.x);
            ot.marshal_float(this.pos.y);
            ot.marshal_float(this.pos.z);
            ot.marshal_float(this.euler.x);
            ot.marshal_float(this.euler.y);
            ot.marshal_float(this.euler.z);
            ot.marshal_float(this.size.x);
            ot.marshal_float(this.size.y);
            ot.marshal_float(this.size.z);
            return ot;
        }

        public override OctetsStream ReadData(OctetsStream ot)
        {
            this.type = ot.unmarshal_int();
            this.pos.x = ot.unmarshal_float();
            this.pos.y = ot.unmarshal_float();
            this.pos.z = ot.unmarshal_float();
            this.euler.x = ot.unmarshal_float();
            this.euler.y = ot.unmarshal_float();
            this.euler.z = ot.unmarshal_float();
            this.size.x = ot.unmarshal_float();
            this.size.y = ot.unmarshal_float();
            this.size.z = ot.unmarshal_float();
            return ot;
        }

        public int type;

        public Vector3 pos = Vector3.zero;

        public Vector3 euler = Vector3.zero;

        public Vector3 size = Vector3.one;
    }
}
