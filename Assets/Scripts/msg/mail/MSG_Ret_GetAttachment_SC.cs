using System;
using System.ComponentModel;
using ProtoBuf;

namespace mail
{
    [ProtoContract(Name = "MSG_Ret_GetAttachment_SC")]
    [Serializable]
    public class MSG_Ret_GetAttachment_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "mailid", DataFormat = DataFormat.TwosComplement)]
        public ulong mailid
        {
            get
            {
                return this._mailid;
            }
            set
            {
                this._mailid = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(2, IsRequired = false, Name = "ret", DataFormat = DataFormat.Default)]
        public bool ret
        {
            get
            {
                return this._ret;
            }
            set
            {
                this._ret = value;
            }
        }

        private ulong _mailid;

        private bool _ret;

        private IExtension extensionObject;
    }
}
