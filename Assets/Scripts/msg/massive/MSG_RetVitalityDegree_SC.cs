using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_RetVitalityDegree_SC")]
    [Serializable]
    public class MSG_RetVitalityDegree_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "info", DataFormat = DataFormat.Default)]
        public VitalityUserInfo info
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

        private VitalityUserInfo _info;

        private IExtension extensionObject;
    }
}
