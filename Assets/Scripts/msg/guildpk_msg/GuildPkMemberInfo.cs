using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "GuildPkMemberInfo")]
    [Serializable]
    public class GuildPkMemberInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(3, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(4, IsRequired = false, Name = "teamid", DataFormat = DataFormat.TwosComplement)]
        public uint teamid
        {
            get
            {
                return this._teamid;
            }
            set
            {
                this._teamid = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "posid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint posid
        {
            get
            {
                return this._posid;
            }
            set
            {
                this._posid = value;
            }
        }

        private ulong _charid;

        private string _name = string.Empty;

        private uint _level;

        private uint _teamid;

        private uint _posid;

        private IExtension extensionObject;
    }
}
