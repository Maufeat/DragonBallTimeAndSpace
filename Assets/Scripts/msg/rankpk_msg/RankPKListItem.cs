using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "RankPKListItem")]
    [Serializable]
    public class RankPKListItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "position", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(3, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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

        [ProtoMember(4, IsRequired = false, Name = "ranklevel", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint ranklevel
        {
            get
            {
                return this._ranklevel;
            }
            set
            {
                this._ranklevel = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "guildname", DataFormat = DataFormat.Default)]
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

        [ProtoMember(6, IsRequired = false, Name = "winbattle", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint winbattle
        {
            get
            {
                return this._winbattle;
            }
            set
            {
                this._winbattle = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "winrate", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint winrate
        {
            get
            {
                return this._winrate;
            }
            set
            {
                this._winrate = value;
            }
        }

        private uint _position;

        private ulong _charid;

        private string _name = string.Empty;

        private uint _ranklevel;

        private string _guildname = string.Empty;

        private uint _winbattle;

        private uint _winrate;

        private IExtension extensionObject;
    }
}
