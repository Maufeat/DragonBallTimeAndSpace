using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_Ret_RefreshRelation_SC")]
    [Serializable]
    public class MSG_Ret_RefreshRelation_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "data", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public relation_item data
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

        private relation_item _data;

        private IExtension extensionObject;
    }
}
