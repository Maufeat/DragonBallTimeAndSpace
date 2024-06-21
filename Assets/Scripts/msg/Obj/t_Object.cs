using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "t_Object")]
    [Serializable]
    public class t_Object : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "baseid", DataFormat = DataFormat.TwosComplement)]
        public uint baseid
        {
            get
            {
                return this._baseid;
            }
            set
            {
                this._baseid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        [DefaultValue(ObjectType.OBJTYPE_WEAPON_NPC)]
        [ProtoMember(3, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public ObjectType type
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

        [DefaultValue(PackType.PACK_TYPE_NONE)]
        [ProtoMember(4, IsRequired = false, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
        public PackType packtype
        {
            get
            {
                return this._packtype;
            }
            set
            {
                this._packtype = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(5, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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
        [ProtoMember(6, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
        public uint num
        {
            get
            {
                return this._num;
            }
            set
            {
                this._num = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "bind", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(8, IsRequired = false, Name = "grid_x", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint grid_x
        {
            get
            {
                return this._grid_x;
            }
            set
            {
                this._grid_x = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(9, IsRequired = false, Name = "grid_y", DataFormat = DataFormat.TwosComplement)]
        public uint grid_y
        {
            get
            {
                return this._grid_y;
            }
            set
            {
                this._grid_y = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(10, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
        public uint quality
        {
            get
            {
                return this._quality;
            }
            set
            {
                this._quality = value;
            }
        }

        [ProtoMember(11, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(12, IsRequired = false, Name = "timer", DataFormat = DataFormat.TwosComplement)]
        public uint timer
        {
            get
            {
                return this._timer;
            }
            set
            {
                this._timer = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(13, IsRequired = false, Name = "equipprop", DataFormat = DataFormat.Default)]
        public EquipData equipprop
        {
            get
            {
                return this._equipprop;
            }
            set
            {
                this._equipprop = value;
            }
        }

        [ProtoMember(14, Name = "equiprand", DataFormat = DataFormat.Default)]
        public List<EquipRandInfo> equiprand
        {
            get
            {
                return this._equiprand;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(15, IsRequired = false, Name = "nextusetime", DataFormat = DataFormat.TwosComplement)]
        public uint nextusetime
        {
            get
            {
                return this._nextusetime;
            }
            set
            {
                this._nextusetime = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(16, IsRequired = false, Name = "card_data", DataFormat = DataFormat.Default)]
        public CardData card_data
        {
            get
            {
                return this._card_data;
            }
            set
            {
                this._card_data = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(17, IsRequired = false, Name = "lock_end_time", DataFormat = DataFormat.TwosComplement)]
        public uint lock_end_time
        {
            get
            {
                return this._lock_end_time;
            }
            set
            {
                this._lock_end_time = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(18, IsRequired = false, Name = "tradetime", DataFormat = DataFormat.TwosComplement)]
        public uint tradetime
        {
            get
            {
                return this._tradetime;
            }
            set
            {
                this._tradetime = value;
            }
        }

        private uint _baseid;

        private string _thisid = string.Empty;

        private ObjectType _type;

        private PackType _packtype;

        private string _name = string.Empty;

        private uint _num;

        private uint _bind;

        private uint _grid_x;

        private uint _grid_y;

        private uint _quality;

        private uint _level;

        private uint _timer;

        private EquipData _equipprop;

        private readonly List<EquipRandInfo> _equiprand = new List<EquipRandInfo>();

        private uint _nextusetime;

        private CardData _card_data;

        private uint _lock_end_time;

        private uint _tradetime;

        private IExtension extensionObject;
    }
}
