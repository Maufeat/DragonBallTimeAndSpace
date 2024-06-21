using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_ReqUserSysSetting_CS")]
    [Serializable]
    public class MSG_ReqUserSysSetting_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint id
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

        [DefaultValue(false)]
        [ProtoMember(2, IsRequired = false, Name = "set", DataFormat = DataFormat.Default)]
        public bool set
        {
            get
            {
                return this._set;
            }
            set
            {
                this._set = value;
            }
        }

        private uint _id;

        private bool _set;

        private IExtension extensionObject;
    }
}
