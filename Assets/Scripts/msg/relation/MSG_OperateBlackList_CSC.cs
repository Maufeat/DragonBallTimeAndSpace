using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_OperateBlackList_CSC")]
    [Serializable]
    public class MSG_OperateBlackList_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "opcode", DataFormat = DataFormat.TwosComplement)]
        public uint opcode
        {
            get
            {
                return this._opcode;
            }
            set
            {
                this._opcode = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "data", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public BlackItem data
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

        [DefaultValue(false)]
        [ProtoMember(4, IsRequired = false, Name = "success", DataFormat = DataFormat.Default)]
        public bool success
        {
            get
            {
                return this._success;
            }
            set
            {
                this._success = value;
            }
        }

        private ulong _charid;

        private uint _opcode;

        private BlackItem _data;

        private bool _success;

        private IExtension extensionObject;
    }
}
