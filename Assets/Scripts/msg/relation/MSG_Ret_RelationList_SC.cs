using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_Ret_RelationList_SC")]
    [Serializable]
    public class MSG_Ret_RelationList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "relations", DataFormat = DataFormat.Default)]
        public List<relation_item> relations
        {
            get
            {
                return this._relations;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        private readonly List<relation_item> _relations = new List<relation_item>();

        private uint _type;

        private IExtension extensionObject;
    }
}
