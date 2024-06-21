using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_RetHoldFlagCountDown_SC")]
    [Serializable]
    public class MSG_RetHoldFlagCountDown_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "stage", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(HoldFlagStage.HOLD_FLAG_STAGE_NONE)]
        public HoldFlagStage stage
        {
            get
            {
                return this._stage;
            }
            set
            {
                this._stage = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "endTimeStamp", DataFormat = DataFormat.TwosComplement)]
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

        private HoldFlagStage _stage;

        private uint _endTimeStamp;

        private IExtension extensionObject;
    }
}
