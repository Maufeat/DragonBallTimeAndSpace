using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_OnUserJump_CSC")]
    [Serializable]
    public class MSG_OnUserJump_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "data", DataFormat = DataFormat.Default)]
        public MoveData data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        public ulong charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        private MoveData _data;

        private ulong _charid;

        private IExtension extensionObject;
    }
}
