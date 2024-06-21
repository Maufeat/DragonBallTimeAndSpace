using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "GuildPositionInfo")]
    [Serializable]
    public class GuildPositionInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "positionid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint positionid
        {
            get
            {
                return this._positionid;
            }
            set
            {
                this._positionid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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
        [ProtoMember(3, IsRequired = false, Name = "orderid", DataFormat = DataFormat.TwosComplement)]
        public uint orderid
        {
            get
            {
                return this._orderid;
            }
            set
            {
                this._orderid = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "privilege", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint privilege
        {
            get
            {
                return this._privilege;
            }
            set
            {
                this._privilege = value;
            }
        }

        private uint _positionid;

        private string _name = string.Empty;

        private uint _orderid;

        private uint _privilege;

        private IExtension extensionObject;
    }
}
