using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_RetSearchTeam_SC")]
    [Serializable]
    public class MSG_RetSearchTeam_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "totalpage", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint totalpage
        {
            get
            {
                return this._totalpage;
            }
            set
            {
                this._totalpage = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "page", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint page
        {
            get
            {
                return this._page;
            }
            set
            {
                this._page = value;
            }
        }

        [ProtoMember(3, Name = "teamlist", DataFormat = DataFormat.Default)]
        public List<MSG_TeamMemeberList_SC> teamlist
        {
            get
            {
                return this._teamlist;
            }
        }

        private uint _totalpage;

        private uint _page;

        private readonly List<MSG_TeamMemeberList_SC> _teamlist = new List<MSG_TeamMemeberList_SC>();

        private IExtension extensionObject;
    }
}
