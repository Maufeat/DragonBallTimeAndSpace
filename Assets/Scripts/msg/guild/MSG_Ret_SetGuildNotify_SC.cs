using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_SetGuildNotify_SC")]
    [Serializable]
    public class MSG_Ret_SetGuildNotify_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(false)]
        [ProtoMember(1, IsRequired = false, Name = "issucc", DataFormat = DataFormat.Default)]
        public bool issucc
        {
            get
            {
                return this._issucc;
            }
            set
            {
                this._issucc = value;
            }
        }

        private bool _issucc;

        private IExtension extensionObject;
    }
}
