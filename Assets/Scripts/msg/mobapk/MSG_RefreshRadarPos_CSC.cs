using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_RefreshRadarPos_CSC")]
    [Serializable]
    public class MSG_RefreshRadarPos_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "pos", DataFormat = DataFormat.Default)]
        public List<RadarPos> pos
        {
            get
            {
                return this._pos;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "radius", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint radius
        {
            get
            {
                return this._radius;
            }
            set
            {
                this._radius = value;
            }
        }

        private readonly List<RadarPos> _pos = new List<RadarPos>();

        private uint _radius;

        private IExtension extensionObject;
    }
}
