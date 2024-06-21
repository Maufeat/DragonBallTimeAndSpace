using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_ReqBattleCancelMatch_SC")]
    [Serializable]
    public class MSG_ReqBattleCancelMatch_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(CancelBatteMatchCode.BATTLE_CANCELMATCH_SUCCESS)]
        [ProtoMember(1, IsRequired = false, Name = "errorCode", DataFormat = DataFormat.TwosComplement)]
        public CancelBatteMatchCode errorCode
        {
            get
            {
                return this._errorCode;
            }
            set
            {
                this._errorCode = value;
            }
        }

        private CancelBatteMatchCode _errorCode = CancelBatteMatchCode.BATTLE_CANCELMATCH_SUCCESS;

        private IExtension extensionObject;
    }
}
