using System;
using System.Collections.Generic;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_RetMatchMember_SC")]
    [Serializable]
    public class MSG_RetMatchMember_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "members", DataFormat = DataFormat.Default)]
        public List<MatchMember> members
        {
            get
            {
                return this._members;
            }
        }

        private readonly List<MatchMember> _members = new List<MatchMember>();

        private IExtension extensionObject;
    }
}
