using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Upload_Crash_Info")]
    [Serializable]
    public class MSG_Upload_Crash_Info : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "crashnum", DataFormat = DataFormat.TwosComplement)]
        public uint crashnum
        {
            get
            {
                return this._crashnum;
            }
            set
            {
                this._crashnum = value;
            }
        }

        private uint _crashnum;

        private IExtension extensionObject;
    }
}
