using System;
using System.ComponentModel;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Req_MainUserRelive_CS")]
    [Serializable]
    public class MSG_Req_MainUserRelive_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "relivetype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint relivetype
        {
            get
            {
                return this._relivetype;
            }
            set
            {
                this._relivetype = value;
            }
        }

        private uint _relivetype;

        private IExtension extensionObject;
    }
}
