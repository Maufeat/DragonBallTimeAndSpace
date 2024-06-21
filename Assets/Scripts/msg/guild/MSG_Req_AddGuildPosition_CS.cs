using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_AddGuildPosition_CS")]
    [Serializable]
    public class MSG_Req_AddGuildPosition_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "posinfo", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public GuildPositionInfo posinfo
        {
            get
            {
                return this._posinfo;
            }
            set
            {
                this._posinfo = value;
            }
        }

        private GuildPositionInfo _posinfo;

        private IExtension extensionObject;
    }
}
