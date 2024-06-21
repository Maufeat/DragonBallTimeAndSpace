using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "RewardsNumber")]
    [Serializable]
    public class RewardsNumber : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "objectid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint objectid
        {
            get
            {
                return this._objectid;
            }
            set
            {
                this._objectid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "number", DataFormat = DataFormat.TwosComplement)]
        public uint number
        {
            get
            {
                return this._number;
            }
            set
            {
                this._number = value;
            }
        }

        private uint _objectid;

        private uint _number;

        private IExtension extensionObject;
    }
}
