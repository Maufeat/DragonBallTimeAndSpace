using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_ClearState_SC")]
    [Serializable]
    public class MSG_Ret_ClearState_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(2, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong id
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

        [ProtoMember(3, IsRequired = false, Name = "state", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public StateItem state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        private uint _type;

        private ulong _id;

        private StateItem _state;

        private IExtension extensionObject;
    }
}
