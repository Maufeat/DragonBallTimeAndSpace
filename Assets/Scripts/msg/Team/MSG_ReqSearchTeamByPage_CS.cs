using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqSearchTeamByPage_CS")]
    [Serializable]
    public class MSG_ReqSearchTeamByPage_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "page", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(2, IsRequired = false, Name = "nearby", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool nearby
        {
            get
            {
                return this._nearby;
            }
            set
            {
                this._nearby = value;
            }
        }

        private uint _page;

        private bool _nearby;

        private IExtension extensionObject;
    }
}
