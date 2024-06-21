using System;
using System.ComponentModel;
using ProtoBuf;

namespace apprentice
{
    [ProtoContract(Name = "MSG_Req_OperateClientDatas_CS")]
    [Serializable]
    public class MSG_Req_OperateClientDatas_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "op", DataFormat = DataFormat.TwosComplement)]
        public uint op
        {
            get
            {
                return this._op;
            }
            set
            {
                this._op = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "key", DataFormat = DataFormat.Default)]
        public string key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "value", DataFormat = DataFormat.Default)]
        public string value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(5, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        private uint _op;

        private string _key = string.Empty;

        private string _value = string.Empty;

        private uint _retcode;

        private uint _type;

        private IExtension extensionObject;
    }
}
