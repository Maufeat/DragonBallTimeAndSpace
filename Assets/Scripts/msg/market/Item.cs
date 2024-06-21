using System;
using System.ComponentModel;
using ProtoBuf;

namespace market
{
    [ProtoContract(Name = "Item")]
    [Serializable]
    public class Item : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "itemid", DataFormat = DataFormat.TwosComplement)]
        public uint itemid
        {
            get
            {
                return this._itemid;
            }
            set
            {
                this._itemid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "discount", DataFormat = DataFormat.TwosComplement)]
        public uint discount
        {
            get
            {
                return this._discount;
            }
            set
            {
                this._discount = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "curnum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint curnum
        {
            get
            {
                return this._curnum;
            }
            set
            {
                this._curnum = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "maxnum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint maxnum
        {
            get
            {
                return this._maxnum;
            }
            set
            {
                this._maxnum = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "costid", DataFormat = DataFormat.TwosComplement)]
        public uint costid
        {
            get
            {
                return this._costid;
            }
            set
            {
                this._costid = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "costnum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint costnum
        {
            get
            {
                return this._costnum;
            }
            set
            {
                this._costnum = value;
            }
        }

        [ProtoMember(8, IsRequired = false, Name = "bind", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint bind
        {
            get
            {
                return this._bind;
            }
            set
            {
                this._bind = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(9, IsRequired = false, Name = "skillid", DataFormat = DataFormat.TwosComplement)]
        public uint skillid
        {
            get
            {
                return this._skillid;
            }
            set
            {
                this._skillid = value;
            }
        }

        [ProtoMember(10, IsRequired = false, Name = "skilllv", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint skilllv
        {
            get
            {
                return this._skilllv;
            }
            set
            {
                this._skilllv = value;
            }
        }

        private uint _id;

        private uint _itemid;

        private uint _discount;

        private uint _curnum;

        private uint _maxnum;

        private uint _costid;

        private uint _costnum;

        private uint _bind;

        private uint _skillid;

        private uint _skilllv;

        private IExtension extensionObject;
    }
}
