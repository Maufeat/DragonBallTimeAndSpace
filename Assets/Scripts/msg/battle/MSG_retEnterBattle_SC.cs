using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_retEnterBattle_SC")]
    [Serializable]
    public class MSG_retEnterBattle_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(EnterBattleCode.BATTLE_ENTER_SUCCESS)]
        [ProtoMember(1, IsRequired = false, Name = "errorCode", DataFormat = DataFormat.TwosComplement)]
        public EnterBattleCode errorCode
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

        private EnterBattleCode _errorCode = EnterBattleCode.BATTLE_ENTER_SUCCESS;

        private IExtension extensionObject;
    }
}
