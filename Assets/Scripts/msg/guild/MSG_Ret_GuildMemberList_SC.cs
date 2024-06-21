using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_GuildMemberList_SC")]
    [Serializable]
    public class MSG_Ret_GuildMemberList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "guildinfo", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public guildInfo guildinfo
        {
            get
            {
                return this._guildinfo;
            }
            set
            {
                this._guildinfo = value;
            }
        }

        [ProtoMember(2, Name = "members", DataFormat = DataFormat.Default)]
        public List<guildMember> members
        {
            get
            {
                return this._members;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "reqtype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(ReqMemberListType.NORMAL)]
        public ReqMemberListType reqtype
        {
            get
            {
                return this._reqtype;
            }
            set
            {
                this._reqtype = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "page", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(5, IsRequired = false, Name = "allpage", DataFormat = DataFormat.TwosComplement)]
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

        private guildInfo _guildinfo;

        private readonly List<guildMember> _members = new List<guildMember>();

        private ReqMemberListType _reqtype = ReqMemberListType.NORMAL;

        private uint _page;

        private uint _allpage;

        private IExtension extensionObject;
    }
}
