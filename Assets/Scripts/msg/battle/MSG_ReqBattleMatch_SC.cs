using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_ReqBattleMatch_SC")]
    [Serializable]
    public class MSG_ReqBattleMatch_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(BatteMatchCode.BATTLE_MATCH_SUCCESS)]
        [ProtoMember(1, IsRequired = false, Name = "errorCode", DataFormat = DataFormat.TwosComplement)]
        public BatteMatchCode errorCode
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

        [ProtoMember(2, IsRequired = false, Name = "averWaitTime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong averWaitTime
        {
            get
            {
                return this._averWaitTime;
            }
            set
            {
                this._averWaitTime = value;
            }
        }

        private BatteMatchCode _errorCode = BatteMatchCode.BATTLE_MATCH_SUCCESS;

        private ulong _averWaitTime;

        private IExtension extensionObject;
    }
}
