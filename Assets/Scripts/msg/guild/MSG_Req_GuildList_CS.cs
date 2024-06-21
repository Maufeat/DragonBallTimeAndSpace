using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_GuildList_CS")]
    [Serializable]
    public class MSG_Req_GuildList_CS : IExtensible
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "countryid", DataFormat = DataFormat.TwosComplement)]
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

        private uint _page;

        private uint _countryid;

        private IExtension extensionObject;
    }
}
