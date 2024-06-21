using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_RetTeamPublicDrop_SC")]
    [Serializable]
    public class MSG_RetTeamPublicDrop_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "oneitem", DataFormat = DataFormat.Default)]
        public List<teamDropItem> oneitem
        {
            get
            {
                return this._oneitem;
            }
        }

        private readonly List<teamDropItem> _oneitem = new List<teamDropItem>();

        private IExtension extensionObject;
    }
}
