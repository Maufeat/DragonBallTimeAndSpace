using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_ACCOUNT_SEC_PASSWD_SC")]
    [Serializable]
    public class MSG_ACCOUNT_SEC_PASSWD_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "sec_passwd", DataFormat = DataFormat.Default)]
        public string sec_passwd
        {
            get
            {
                return this._sec_passwd;
            }
            set
            {
                this._sec_passwd = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(2, IsRequired = false, Name = "isonline", DataFormat = DataFormat.Default)]
        public bool isonline
        {
            get
            {
                return this._isonline;
            }
            set
            {
                this._isonline = value;
            }
        }

        private string _sec_passwd = string.Empty;

        private bool _isonline;

        private IExtension extensionObject;
    }
}
