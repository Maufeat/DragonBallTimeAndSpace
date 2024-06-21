using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_UserMatchReq_CS")]
    [Serializable]
    public class MSG_UserMatchReq_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(false)]
        [ProtoMember(1, IsRequired = false, Name = "is_match", DataFormat = DataFormat.Default)]
        public bool is_match
        {
            get
            {
                return this._is_match;
            }
            set
            {
                this._is_match = value;
            }
        }

        private bool _is_match;

        private IExtension extensionObject;
    }
}
