using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_RetGetVIPCardInfo_SC")]
    [Serializable]
    public class MSG_RetGetVIPCardInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "vipcardinfo", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public VIPCardInfo vipcardinfo
        {
            get
            {
                return this._vipcardinfo;
            }
            set
            {
                this._vipcardinfo = value;
            }
        }

        private VIPCardInfo _vipcardinfo;

        private IExtension extensionObject;
    }
}
