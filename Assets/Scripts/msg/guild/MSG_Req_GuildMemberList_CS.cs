using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_GuildMemberList_CS")]
    [Serializable]
    public class MSG_Req_GuildMemberList_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "page", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(ReqMemberListType.NORMAL)]
        [ProtoMember(2, IsRequired = false, Name = "reqtype", DataFormat = DataFormat.TwosComplement)]
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

        private uint _page;

        private ReqMemberListType _reqtype = ReqMemberListType.NORMAL;

        private IExtension extensionObject;
    }
}
