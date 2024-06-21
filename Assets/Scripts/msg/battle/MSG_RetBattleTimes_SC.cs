using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_RetBattleTimes_SC")]
    [Serializable]
    public class MSG_RetBattleTimes_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "winBattleTimes", DataFormat = DataFormat.TwosComplement)]
        public uint winBattleTimes
        {
            get
            {
                return this._winBattleTimes;
            }
            set
            {
                this._winBattleTimes = value;
            }
        }

        private uint _winBattleTimes;

        private IExtension extensionObject;
    }
}
