using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqDelMemember_SC")]
    [Serializable]
    public class MSG_ReqDelMemember_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "charid", DataFormat = DataFormat.Default)]
        public string charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        [ProtoMember(2, IsRequired = true, Name = "outtype", DataFormat = DataFormat.TwosComplement)]
        public OutType outtype
        {
            get
            {
                return this._outtype;
            }
            set
            {
                this._outtype = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        private string _charid;

        private OutType _outtype;

        private string _name = string.Empty;

        private IExtension extensionObject;
    }
}
