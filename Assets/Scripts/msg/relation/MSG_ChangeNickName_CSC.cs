using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_ChangeNickName_CSC")]
    [Serializable]
    public class MSG_ChangeNickName_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
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

        [ProtoMember(2, IsRequired = false, Name = "nickname", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string nickname
        {
            get
            {
                return this._nickname;
            }
            set
            {
                this._nickname = value;
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

        private string _nickname = string.Empty;

        private bool _success;

        private IExtension extensionObject;
    }
}
