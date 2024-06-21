using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_Ret_GuildPkEnroll_SC")]
    [Serializable]
    public class MSG_Ret_GuildPkEnroll_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "result", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }

        private bool _result;

        private IExtension extensionObject;
    }
}
