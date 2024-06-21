using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "CardPackInfo")]
    [Serializable]
    public class CardPackInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "gold_opened_num", DataFormat = DataFormat.TwosComplement)]
        public uint gold_opened_num
        {
            get
            {
                return this._gold_opened_num;
            }
            set
            {
                this._gold_opened_num = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "wood_opened_num", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint wood_opened_num
        {
            get
            {
                return this._wood_opened_num;
            }
            set
            {
                this._wood_opened_num = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "water_opened_num", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint water_opened_num
        {
            get
            {
                return this._water_opened_num;
            }
            set
            {
                this._water_opened_num = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "fire_opened_num", DataFormat = DataFormat.TwosComplement)]
        public uint fire_opened_num
        {
            get
            {
                return this._fire_opened_num;
            }
            set
            {
                this._fire_opened_num = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "earth_opened_num", DataFormat = DataFormat.TwosComplement)]
        public uint earth_opened_num
        {
            get
            {
                return this._earth_opened_num;
            }
            set
            {
                this._earth_opened_num = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "hero_baseid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint hero_baseid
        {
            get
            {
                return this._hero_baseid;
            }
            set
            {
                this._hero_baseid = value;
            }
        }

        [ProtoMember(7, Name = "objs", DataFormat = DataFormat.Default)]
        public List<t_Object> objs
        {
            get
            {
                return this._objs;
            }
        }

        private uint _gold_opened_num;

        private uint _wood_opened_num;

        private uint _water_opened_num;

        private uint _fire_opened_num;

        private uint _earth_opened_num;

        private uint _hero_baseid;

        private readonly List<t_Object> _objs = new List<t_Object>();

        private IExtension extensionObject;
    }
}
