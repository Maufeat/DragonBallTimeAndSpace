using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_GuildList_SC")]
    [Serializable]
    public class MSG_Ret_GuildList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "guildList", DataFormat = DataFormat.Default)]
        public List<guildListItem> guildList
        {
            get
            {
                return this._guildList;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "allpage", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint allpage
        {
            get
            {
                return this._allpage;
            }
            set
            {
                this._allpage = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "page", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "countryid", DataFormat = DataFormat.TwosComplement)]
        public uint countryid
        {
            get
            {
                return this._countryid;
            }
            set
            {
                this._countryid = value;
            }
        }

        private readonly List<guildListItem> _guildList = new List<guildListItem>();

        private uint _allpage;

        private uint _page;

        private uint _countryid;

        private IExtension extensionObject;
    }
}
