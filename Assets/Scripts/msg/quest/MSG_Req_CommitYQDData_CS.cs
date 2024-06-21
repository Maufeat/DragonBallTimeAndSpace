using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_Req_CommitYQDData_CS")]
    [Serializable]
    public class MSG_Req_CommitYQDData_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "step", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint step
        {
            get
            {
                return this._step;
            }
            set
            {
                this._step = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "success", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint success
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
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

        private uint _step;

        private uint _success;

        private uint _type;

        private IExtension extensionObject;
    }
}
