using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_Refresh_GuildPkMemberInfo_SC")]
    [Serializable]
    public class MSG_Refresh_GuildPkMemberInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "member", DataFormat = DataFormat.Default)]
        public GuildPkMemberInfo member
        {
            get
            {
                return this._member;
            }
            set
            {
                this._member = value;
            }
        }

        private GuildPkMemberInfo _member;

        private IExtension extensionObject;
    }
}
