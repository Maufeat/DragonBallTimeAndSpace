using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_SetMainHero_CSC")]
    [Serializable]
    public class MSG_SetMainHero_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "herothisid", DataFormat = DataFormat.TwosComplement)]
        public ulong herothisid
        {
            get
            {
                return this._herothisid;
            }
            set
            {
                this._herothisid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "opcode", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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
        [ProtoMember(3, IsRequired = false, Name = "errorcode", DataFormat = DataFormat.TwosComplement)]
        public uint errorcode
        {
            get
            {
                return this._errorcode;
            }
            set
            {
                this._errorcode = value;
            }
        }

        private ulong _herothisid;

        private uint _opcode;

        private uint _errorcode;

        private IExtension extensionObject;
    }
}
