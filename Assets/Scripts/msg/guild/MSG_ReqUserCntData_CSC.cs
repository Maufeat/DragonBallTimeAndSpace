using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_ReqUserCntData_CSC")]
    [Serializable]
    public class MSG_ReqUserCntData_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(UserDataType.GUILD_DAILY_COUNTRIBUTE)]
        public UserDataType type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "value", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        private UserDataType _type = UserDataType.GUILD_DAILY_COUNTRIBUTE;

        private uint _value;

        private IExtension extensionObject;
    }
}
