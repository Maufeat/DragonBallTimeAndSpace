using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_Ret_DeleteRelation_SC")]
    [Serializable]
    public class MSG_Ret_DeleteRelation_SC : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "issucc", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
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

        private ulong _relationid;

        private bool _issucc;

        private IExtension extensionObject;
    }
}
