using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "guildListItem")]
    [Serializable]
    public class guildListItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "guild", DataFormat = DataFormat.Default)]
        public guildInfo guild
        {
            get
            {
                return this._guild;
            }
            set
            {
                this._guild = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "guildtype", DataFormat = DataFormat.TwosComplement)]
        public uint guildtype
        {
            get
            {
                return this._guildtype;
            }
            set
            {
                this._guildtype = value;
            }
        }

        private guildInfo _guild;

        private uint _guildtype;

        private IExtension extensionObject;
    }
}
