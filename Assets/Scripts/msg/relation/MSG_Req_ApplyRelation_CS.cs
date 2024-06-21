using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_Req_ApplyRelation_CS")]
    [Serializable]
    public class MSG_Req_ApplyRelation_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "relationid", DataFormat = DataFormat.TwosComplement)]
        public ulong relationid
        {
            get
            {
                return this._relationid;
            }
            set
            {
                this._relationid = value;
            }
        }

        private ulong _relationid;

        private IExtension extensionObject;
    }
}
