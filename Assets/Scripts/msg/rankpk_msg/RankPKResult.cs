using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "RankPKResult")]
    [Serializable]
    public class RankPKResult : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        public uint charid
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "heroid", DataFormat = DataFormat.TwosComplement)]
        public uint heroid
        {
            get
            {
                return this._heroid;
            }
            set
            {
                this._heroid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "cure", DataFormat = DataFormat.TwosComplement)]
        public uint cure
        {
            get
            {
                return this._cure;
            }
            set
            {
                this._cure = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "hurt", DataFormat = DataFormat.TwosComplement)]
        public uint hurt
        {
            get
            {
                return this._hurt;
            }
            set
            {
                this._hurt = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "dead", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint dead
        {
            get
            {
                return this._dead;
            }
            set
            {
                this._dead = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "kill", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint kill
        {
            get
            {
                return this._kill;
            }
            set
            {
                this._kill = value;
            }
        }

        private uint _charid;

        private string _name = string.Empty;

        private uint _heroid;

        private uint _cure;

        private uint _hurt;

        private uint _dead;

        private uint _kill;

        private IExtension extensionObject;
    }
}
