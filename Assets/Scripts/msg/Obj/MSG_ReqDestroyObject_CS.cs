using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqDestroyObject_CS")]
    [Serializable]
    public class MSG_ReqDestroyObject_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        [ProtoMember(2, IsRequired = true, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
        public PackType packtype
        {
            get
            {
                return this._packtype;
            }
            set
            {
                this._packtype = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "passwd", DataFormat = DataFormat.Default)]
        public string passwd
        {
            get
            {
                return this._passwd;
            }
            set
            {
                this._passwd = value;
            }
        }

        private string _thisid = string.Empty;

        private PackType _packtype;

        private string _passwd = string.Empty;

        private IExtension extensionObject;
    }
}
