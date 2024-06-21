using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqFixUpDurability_CS")]
    [Serializable]
    public class MSG_ReqFixUpDurability_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(PackType.PACK_TYPE_NONE)]
        [ProtoMember(1, IsRequired = false, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(2, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
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

        private PackType _packtype;

        private string _thisid = string.Empty;

        private IExtension extensionObject;
    }
}
