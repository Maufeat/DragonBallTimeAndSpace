using System;
using System.ComponentModel;
using ProtoBuf;

namespace Pet
{
    [ProtoContract(Name = "MSG_RetUserPetInfo_SC")]
    [Serializable]
    public class MSG_RetUserPetInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "info", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public SummonPetUseInfo info
        {
            get
            {
                return this._info;
            }
            set
            {
                this._info = value;
            }
        }

        private SummonPetUseInfo _info;

        private IExtension extensionObject;
    }
}
