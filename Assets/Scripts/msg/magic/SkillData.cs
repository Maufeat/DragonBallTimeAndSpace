using System;
using System.ComponentModel;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "SkillData")]
    [Serializable]
    public class SkillData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "skillid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(2, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0f)]
        [ProtoMember(3, IsRequired = false, Name = "lastusetime", DataFormat = DataFormat.TwosComplement)]
        public ulong lastusetime
        {
            get
            {
                return this._lastusetime;
            }
            set
            {
                this._lastusetime = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "onoff", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint onoff
        {
            get
            {
                return this._onoff;
            }
            set
            {
                this._onoff = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "lastupdatetime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong lastupdatetime
        {
            get
            {
                return this._lastupdatetime;
            }
            set
            {
                this._lastupdatetime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "overlaytimes", DataFormat = DataFormat.TwosComplement)]
        public uint overlaytimes
        {
            get
            {
                return this._overlaytimes;
            }
            set
            {
                this._overlaytimes = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "active_stages", DataFormat = DataFormat.TwosComplement)]
        public uint active_stages
        {
            get
            {
                return this._active_stages;
            }
            set
            {
                this._active_stages = value;
            }
        }

        [ProtoMember(8, IsRequired = false, Name = "maxmultitimes", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint maxmultitimes
        {
            get
            {
                return this._maxmultitimes;
            }
            set
            {
                this._maxmultitimes = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(9, IsRequired = false, Name = "skillcd", DataFormat = DataFormat.TwosComplement)]
        public uint skillcd
        {
            get
            {
                return this._skillcd;
            }
            set
            {
                this._skillcd = value;
            }
        }

        private uint _skillid;

        private uint _level;

        private ulong _lastusetime;

        private uint _onoff;

        private ulong _lastupdatetime;

        private uint _overlaytimes;

        private uint _active_stages;

        private uint _maxmultitimes;

        private uint _skillcd;

        private IExtension extensionObject;
    }
}
