using System;
using System.ComponentModel;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_ReqStopSellStaff_CS")]
    [Serializable]
    public class MSG_ReqStopSellStaff_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
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

        private string _thisid = string.Empty;

        private IExtension extensionObject;
    }
}
