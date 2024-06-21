using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MatchMember")]
    [Serializable]
    public class MatchMember : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "userid", DataFormat = DataFormat.TwosComplement)]
        public ulong userid
        {
            get
            {
                return this._userid;
            }
            set
            {
                this._userid = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "captain", DataFormat = DataFormat.TwosComplement)]
        public ulong captain
        {
            get
            {
                return this._captain;
            }
            set
            {
                this._captain = value;
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

        [DefaultValue("")]
        [ProtoMember(4, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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
        [ProtoMember(5, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(6, IsRequired = false, Name = "camp", DataFormat = DataFormat.TwosComplement)]
        public uint camp
        {
            get
            {
                return this._camp;
            }
            set
            {
                this._camp = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "gid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong gid
        {
            get
            {
                return this._gid;
            }
            set
            {
                this._gid = value;
            }
        }

        private ulong _userid;

        private ulong _captain;

        private uint _heroid;

        private string _name = string.Empty;

        private uint _level;

        private uint _camp;

        private ulong _gid;

        private IExtension extensionObject;
    }
}
