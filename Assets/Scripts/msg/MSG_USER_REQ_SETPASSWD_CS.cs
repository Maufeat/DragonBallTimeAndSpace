using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_USER_REQ_SETPASSWD_CS")]
    [Serializable]
    public class MSG_USER_REQ_SETPASSWD_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "old_passwd", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string old_passwd
        {
            get
            {
                return this._old_passwd;
            }
            set
            {
                this._old_passwd = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "new_passwd", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string new_passwd
        {
            get
            {
                return this._new_passwd;
            }
            set
            {
                this._new_passwd = value;
            }
        }

        private string _old_passwd = string.Empty;

        private string _new_passwd = string.Empty;

        private IExtension extensionObject;
    }
}
