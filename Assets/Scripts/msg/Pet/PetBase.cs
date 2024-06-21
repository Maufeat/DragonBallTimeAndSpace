using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Pet
{
    [ProtoContract(Name = "PetBase")]
    [Serializable]
    public class PetBase : IExtensible
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

        [DefaultValue(0f)]
        [ProtoMember(3, IsRequired = false, Name = "tempid", DataFormat = DataFormat.TwosComplement)]
        public ulong tempid
        {
            get
            {
                return this._tempid;
            }
            set
            {
                this._tempid = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(5, IsRequired = false, Name = "exp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint exp
        {
            get
            {
                return this._exp;
            }
            set
            {
                this._exp = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "hp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint hp
        {
            get
            {
                return this._hp;
            }
            set
            {
                this._hp = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(8, IsRequired = false, Name = "prop", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public PropItem prop
        {
            get
            {
                return this._prop;
            }
            set
            {
                this._prop = value;
            }
        }

        [ProtoMember(9, IsRequired = false, Name = "aptitude", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public PropItem aptitude
        {
            get
            {
                return this._aptitude;
            }
            set
            {
                this._aptitude = value;
            }
        }

        [ProtoMember(10, Name = "state", DataFormat = DataFormat.TwosComplement)]
        public List<PetState> state
        {
            get
            {
                return this._state;
            }
        }

        [ProtoMember(11, IsRequired = false, Name = "passivenum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint passivenum
        {
            get
            {
                return this._passivenum;
            }
            set
            {
                this._passivenum = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(12, IsRequired = false, Name = "activeskillid", DataFormat = DataFormat.TwosComplement)]
        public uint activeskillid
        {
            get
            {
                return this._activeskillid;
            }
            set
            {
                this._activeskillid = value;
            }
        }

        [ProtoMember(13, IsRequired = false, Name = "talentskillid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint talentskillid
        {
            get
            {
                return this._talentskillid;
            }
            set
            {
                this._talentskillid = value;
            }
        }

        private uint _id;

        private string _name = string.Empty;

        private ulong _tempid;

        private uint _level;

        private uint _exp;

        private uint _hp;

        private uint _quality;

        private PropItem _prop;

        private PropItem _aptitude;

        private readonly List<PetState> _state = new List<PetState>();

        private uint _passivenum;

        private uint _activeskillid;

        private uint _talentskillid;

        private IExtension extensionObject;
    }
}
