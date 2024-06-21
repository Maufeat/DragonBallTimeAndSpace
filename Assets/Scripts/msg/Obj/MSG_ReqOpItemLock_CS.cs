using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqOpItemLock_CS")]
    [Serializable]
    public class MSG_ReqOpItemLock_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        [DefaultValue(PackType.PACK_TYPE_NONE)]
        [ProtoMember(2, IsRequired = false, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
        public PackType packtype
        {
            get
            {
                return this._packtype;
            }
            set
            {
                this._packtype = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "opcode", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue("")]
        [ProtoMember(4, IsRequired = false, Name = "passwd", DataFormat = DataFormat.Default)]
        public string passwd
        {
            get
            {
                return this._passwd;
            }
            set
            {
                this._passwd = value;
            }
        }

        private string _thisid = string.Empty;

        private PackType _packtype;

        private uint _opcode;

        private string _passwd = string.Empty;

        private IExtension extensionObject;
    }
}
