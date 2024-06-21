using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace mail
{
    [ProtoContract(Name = "MSG_Ret_DeleteMail_SC")]
    [Serializable]
    public class MSG_Ret_DeleteMail_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "mailid", DataFormat = DataFormat.Default)]
        public List<string> mailid
        {
            get
            {
                return this._mailid;
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

        private readonly List<string> _mailid = new List<string>();

        private bool _ret;

        private IExtension extensionObject;
    }
}
