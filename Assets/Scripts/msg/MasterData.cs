using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MasterData")]
    [Serializable]
    public class MasterData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "idtype", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public EntryIDType idtype
        {
            get
            {
                return this._idtype;
            }
            set
            {
                this._idtype = value;
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "country", DataFormat = DataFormat.TwosComplement)]
        public uint country
        {
            get
            {
                return this._country;
            }
            set
            {
                this._country = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "guildid", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(5, IsRequired = false, Name = "teamid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        private EntryIDType _idtype;

        private string _name = string.Empty;

        private uint _country;

        private ulong _guildid;

        private uint _teamid;

        private IExtension extensionObject;
    }
}
