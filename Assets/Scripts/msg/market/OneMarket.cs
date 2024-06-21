using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace market
{
    [ProtoContract(Name = "OneMarket")]
    [Serializable]
    public class OneMarket : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        [DefaultValue(MarketType.MarketType_1)]
        [ProtoMember(2, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public MarketType type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [ProtoMember(3, Name = "itemlist", DataFormat = DataFormat.Default)]
        public List<Item> itemlist
        {
            get
            {
                return this._itemlist;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "refreshtype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint refreshtype
        {
            get
            {
                return this._refreshtype;
            }
            set
            {
                this._refreshtype = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(5, IsRequired = false, Name = "refreshtime", DataFormat = DataFormat.Default)]
        public string refreshtime
        {
            get
            {
                return this._refreshtime;
            }
            set
            {
                this._refreshtime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "subtype", DataFormat = DataFormat.TwosComplement)]
        public uint subtype
        {
            get
            {
                return this._subtype;
            }
            set
            {
                this._subtype = value;
            }
        }

        private uint _id;

        private MarketType _type = MarketType.MarketType_1;

        private readonly List<Item> _itemlist = new List<Item>();

        private uint _refreshtype;

        private string _refreshtime = string.Empty;

        private uint _subtype;

        private IExtension extensionObject;
    }
}
