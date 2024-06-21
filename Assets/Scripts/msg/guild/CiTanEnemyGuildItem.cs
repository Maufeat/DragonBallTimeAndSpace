using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "CiTanEnemyGuildItem")]
    [Serializable]
    public class CiTanEnemyGuildItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "guildid", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(2, IsRequired = false, Name = "guildlevel", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint guildlevel
        {
            get
            {
                return this._guildlevel;
            }
            set
            {
                this._guildlevel = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "guildname", DataFormat = DataFormat.Default)]
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

        [DefaultValue("")]
        [ProtoMember(4, IsRequired = false, Name = "mastername", DataFormat = DataFormat.Default)]
        public string mastername
        {
            get
            {
                return this._mastername;
            }
            set
            {
                this._mastername = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "isvalid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint isvalid
        {
            get
            {
                return this._isvalid;
            }
            set
            {
                this._isvalid = value;
            }
        }

        private ulong _guildid;

        private uint _guildlevel;

        private string _guildname = string.Empty;

        private string _mastername = string.Empty;

        private uint _isvalid;

        private IExtension extensionObject;
    }
}
