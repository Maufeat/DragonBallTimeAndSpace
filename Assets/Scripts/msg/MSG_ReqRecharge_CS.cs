using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_ReqRecharge_CS")]
    [Serializable]
    public class MSG_ReqRecharge_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "point", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint point
        {
            get
            {
                return this._point;
            }
            set
            {
                this._point = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public uint type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        private uint _point;

        private uint _type;

        private IExtension extensionObject;
    }
}
