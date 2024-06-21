using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_NotifyAntiAddict_SC")]
    [Serializable]
    public class MSG_NotifyAntiAddict_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "onlinelasttime", DataFormat = DataFormat.TwosComplement)]
        public uint onlinelasttime
        {
            get
            {
                return this._onlinelasttime;
            }
            set
            {
                this._onlinelasttime = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(2, IsRequired = false, Name = "isAntiAddcit", DataFormat = DataFormat.Default)]
        public bool isAntiAddcit
        {
            get
            {
                return this._isAntiAddcit;
            }
            set
            {
                this._isAntiAddcit = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(3, IsRequired = false, Name = "isLoginPush", DataFormat = DataFormat.Default)]
        public bool isLoginPush
        {
            get
            {
                return this._isLoginPush;
            }
            set
            {
                this._isLoginPush = value;
            }
        }

        private uint _onlinelasttime;

        private bool _isAntiAddcit;

        private bool _isLoginPush;

        private IExtension extensionObject;
    }
}
