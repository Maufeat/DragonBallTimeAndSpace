using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_RetCardPackInfo_SC")]
    [Serializable]
    public class MSG_RetCardPackInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "data", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public CardPackInfo data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        private CardPackInfo _data;

        private IExtension extensionObject;
    }
}
