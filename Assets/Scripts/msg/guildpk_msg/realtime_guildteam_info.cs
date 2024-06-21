using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "realtime_guildteam_info")]
    [Serializable]
    public class realtime_guildteam_info : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "guildid", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "guildname", DataFormat = DataFormat.Default)]
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "leftnum", DataFormat = DataFormat.TwosComplement)]
        public uint leftnum
        {
            get
            {
                return this._leftnum;
            }
            set
            {
                this._leftnum = value;
            }
        }

        private ulong _guildid;

        private string _guildname = string.Empty;

        private uint _leftnum;

        private IExtension extensionObject;
    }
}
