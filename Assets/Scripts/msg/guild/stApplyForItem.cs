using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "stApplyForItem")]
    [Serializable]
    public class stApplyForItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "applyid", DataFormat = DataFormat.TwosComplement)]
        public ulong applyid
        {
            get
            {
                return this._applyid;
            }
            set
            {
                this._applyid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "applyname", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string applyname
        {
            get
            {
                return this._applyname;
            }
            set
            {
                this._applyname = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(3, IsRequired = false, Name = "issucc", DataFormat = DataFormat.Default)]
        public bool issucc
        {
            get
            {
                return this._issucc;
            }
            set
            {
                this._issucc = value;
            }
        }

        private ulong _applyid;

        private string _applyname = string.Empty;

        private bool _issucc;

        private IExtension extensionObject;
    }
}
