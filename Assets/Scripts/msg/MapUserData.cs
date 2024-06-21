using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MapUserData")]
    [Serializable]
    public class MapUserData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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

        [ProtoMember(3, IsRequired = false, Name = "mapshow", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public CharacterMapShow mapshow
        {
            get
            {
                return this._mapshow;
            }
            set
            {
                this._mapshow = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(4, IsRequired = false, Name = "bakhero", DataFormat = DataFormat.Default)]
        public CharacterMapShow bakhero
        {
            get
            {
                return this._bakhero;
            }
            set
            {
                this._bakhero = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(5, IsRequired = false, Name = "mapdata", DataFormat = DataFormat.Default)]
        public CharacterMapData mapdata
        {
            get
            {
                return this._mapdata;
            }
            set
            {
                this._mapdata = value;
            }
        }

        private ulong _charid;

        private string _name = string.Empty;

        private CharacterMapShow _mapshow;

        private CharacterMapShow _bakhero;

        private CharacterMapData _mapdata;

        private IExtension extensionObject;
    }
}
