using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "ChangeMapFindWayInfo")]
    [Serializable]
    public class ChangeMapFindWayInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "errcode", DataFormat = DataFormat.TwosComplement)]
        public uint errcode
        {
            get
            {
                return this._errcode;
            }
            set
            {
                this._errcode = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "pathwayid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint pathwayid
        {
            get
            {
                return this._pathwayid;
            }
            set
            {
                this._pathwayid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "destmapid", DataFormat = DataFormat.TwosComplement)]
        public uint destmapid
        {
            get
            {
                return this._destmapid;
            }
            set
            {
                this._destmapid = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(4, IsRequired = false, Name = "findingway", DataFormat = DataFormat.Default)]
        public bool findingway
        {
            get
            {
                return this._findingway;
            }
            set
            {
                this._findingway = value;
            }
        }

        private uint _errcode;

        private uint _pathwayid;

        private uint _destmapid;

        private bool _findingway;

        private IExtension extensionObject;
    }
}
