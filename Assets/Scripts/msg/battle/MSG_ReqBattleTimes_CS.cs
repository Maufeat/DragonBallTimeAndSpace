using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_ReqBattleTimes_CS")]
    [Serializable]
    public class MSG_ReqBattleTimes_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "battleId", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint battleId
        {
            get
            {
                return this._battleId;
            }
            set
            {
                this._battleId = value;
            }
        }

        private uint _battleId;

        private IExtension extensionObject;
    }
}
