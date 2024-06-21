using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_Ret_SearchRelation_SC")]
    [Serializable]
    public class MSG_Ret_SearchRelation_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "relation", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public relation_item relation
        {
            get
            {
                return this._relation;
            }
            set
            {
                this._relation = value;
            }
        }

        private relation_item _relation;

        private IExtension extensionObject;
    }
}
