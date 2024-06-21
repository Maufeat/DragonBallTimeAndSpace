using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Req_Delete_Char_CSC")]
    [Serializable]
    public class MSG_Req_Delete_Char_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        public uint retcode
        {
            get
            {
                return this._retcode;
            }
            set
            {
                this._retcode = value;
            }
        }

        private ulong _charid;

        private uint _opcode;

        private uint _retcode;

        private IExtension extensionObject;
    }
}
