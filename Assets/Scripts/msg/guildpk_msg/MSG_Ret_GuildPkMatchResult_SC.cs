using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_Ret_GuildPkMatchResult_SC")]
    [Serializable]
    public class MSG_Ret_GuildPkMatchResult_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint retcode
        {
            get
            {
                return this._retcode;
            }
            set
            {
                this._retcode = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "lefttime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        private uint _retcode;

        private uint _lefttime;

        private IExtension extensionObject;
    }
}
