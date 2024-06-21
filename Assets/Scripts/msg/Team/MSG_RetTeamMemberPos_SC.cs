using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_RetTeamMemberPos_SC")]
    [Serializable]
    public class MSG_RetTeamMemberPos_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "members", DataFormat = DataFormat.Default)]
        public List<MemberPos> members
        {
            get
            {
                return this._members;
            }
        }

        private readonly List<MemberPos> _members = new List<MemberPos>();

        private IExtension extensionObject;
    }
}
