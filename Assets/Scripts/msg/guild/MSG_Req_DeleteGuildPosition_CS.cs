using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_DeleteGuildPosition_CS")]
    [Serializable]
    public class MSG_Req_DeleteGuildPosition_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "positionid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint positionid
        {
            get
            {
                return this._positionid;
            }
            set
            {
                this._positionid = value;
            }
        }

        private uint _positionid;

        private IExtension extensionObject;
    }
}
