using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_RetBattleValid_SC")]
    [Serializable]
    public class MSG_RetBattleValid_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "endTimeStamp", DataFormat = DataFormat.TwosComplement)]
        public uint endTimeStamp
        {
            get
            {
                return this._endTimeStamp;
            }
            set
            {
                this._endTimeStamp = value;
            }
        }

        private uint _endTimeStamp;

        private IExtension extensionObject;
    }
}
