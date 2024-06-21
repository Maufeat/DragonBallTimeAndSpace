using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqRaffVIPCardPrize_CS")]
    [Serializable]
    public class MSG_ReqRaffVIPCardPrize_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "usetype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(RaffUseType.RAFFUSETYPE_FREETIMES)]
        public RaffUseType usetype
        {
            get
            {
                return this._usetype;
            }
            set
            {
                this._usetype = value;
            }
        }

        private RaffUseType _usetype;

        private IExtension extensionObject;
    }
}
