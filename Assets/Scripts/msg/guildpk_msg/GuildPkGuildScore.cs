using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "GuildPkGuildScore")]
    [Serializable]
    public class GuildPkGuildScore : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint rank
        {
            get
            {
                return this._rank;
            }
            set
            {
                this._rank = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "guildid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong guildid
        {
            get
            {
                return this._guildid;
            }
            set
            {
                this._guildid = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "guildname", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string guildname
        {
            get
            {
                return this._guildname;
            }
            set
            {
                this._guildname = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "score", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint score
        {
            get
            {
                return this._score;
            }
            set
            {
                this._score = value;
            }
        }

        private uint _rank;

        private ulong _guildid;

        private string _guildname = string.Empty;

        private uint _score;

        private IExtension extensionObject;
    }
}
