using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_MoveFriendToPage_CSC")]
    [Serializable]
    public class MSG_MoveFriendToPage_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        public ulong charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "page", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string page
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

        [ProtoMember(3, IsRequired = false, Name = "success", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool success
        {
            get
            {
                return this._success;
            }
            set
            {
                this._success = value;
            }
        }

        private ulong _charid;

        private string _page = string.Empty;

        private bool _success;

        private IExtension extensionObject;
    }
}
