using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_RetHoldFlagEvent_SC")]
    [Serializable]
    public class MSG_RetHoldFlagEvent_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "tipId", DataFormat = DataFormat.TwosComplement)]
        public uint tipId
        {
            get
            {
                return this._tipId;
            }
            set
            {
                this._tipId = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "camp", DataFormat = DataFormat.TwosComplement)]
        public uint camp
        {
            get
            {
                return this._camp;
            }
            set
            {
                this._camp = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "userName", DataFormat = DataFormat.Default)]
        public string userName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
            }
        }

        private uint _tipId;

        private uint _camp;

        private string _userName = string.Empty;

        private IExtension extensionObject;
    }
}
