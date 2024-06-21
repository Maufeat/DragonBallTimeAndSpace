using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_Team_List_SC")]
    [Serializable]
    public class MSG_Team_List_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "oneteam", DataFormat = DataFormat.Default)]
        public List<MSG_TeamMemeberList_SC> oneteam
        {
            get
            {
                return this._oneteam;
            }
        }

        private readonly List<MSG_TeamMemeberList_SC> _oneteam = new List<MSG_TeamMemeberList_SC>();

        private IExtension extensionObject;
    }
}
