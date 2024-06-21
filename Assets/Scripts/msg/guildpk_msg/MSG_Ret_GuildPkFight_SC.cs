using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_Ret_GuildPkFight_SC")]
    [Serializable]
    public class MSG_Ret_GuildPkFight_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "lefttime", DataFormat = DataFormat.TwosComplement)]
        public uint lefttime
        {
            get
            {
                return this._lefttime;
            }
            set
            {
                this._lefttime = value;
            }
        }

        private uint _lefttime;

        private IExtension extensionObject;
    }
}
