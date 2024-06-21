using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_Join_Team_SC")]
    [Serializable]
    public class MSG_Join_Team_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
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

        private uint _retcode;

        private IExtension extensionObject;
    }
}
