using System;
using System.ComponentModel;
using ProtoBuf;

namespace quiz
{
    [ProtoContract(Name = "MSG_Req_RecvSeekActivityPrize_CS")]
    [Serializable]
    public class MSG_Req_RecvSeekActivityPrize_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        public uint id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        private uint _id;

        private IExtension extensionObject;
    }
}
