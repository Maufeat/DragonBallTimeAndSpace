using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "GetBagInfo")]
    [Serializable]
    public class GetBagInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "idx", DataFormat = DataFormat.TwosComplement)]
        public uint idx
        {
            get
            {
                return this._idx;
            }
            set
            {
                this._idx = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "uid", DataFormat = DataFormat.TwosComplement)]
        public ulong uid
        {
            get
            {
                return this._uid;
            }
            set
            {
                this._uid = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "objectid", DataFormat = DataFormat.TwosComplement)]
        public uint objectid
        {
            get
            {
                return this._objectid;
            }
            set
            {
                this._objectid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "count", DataFormat = DataFormat.TwosComplement)]
        public uint count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
            }
        }

        private uint _idx;

        private ulong _uid;

        private string _name = string.Empty;

        private uint _objectid;

        private uint _count;

        private IExtension extensionObject;
    }
}
