using System;
using System.ComponentModel;
using ProtoBuf;

namespace country
{
    [ProtoContract(Name = "MSG_Ret_JoinCountry_SC")]
    [Serializable]
    public class MSG_Ret_JoinCountry_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
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
