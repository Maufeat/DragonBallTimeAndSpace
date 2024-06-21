using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_NotifyTeamDismiss_SC")]
    [Serializable]
    public class MSG_NotifyTeamDismiss_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "suc", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool suc
        {
            get
            {
                return this._suc;
            }
            set
            {
                this._suc = value;
            }
        }

        private bool _suc;

        private IExtension extensionObject;
    }
}
