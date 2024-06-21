using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RetStartFight_SC")]
    [Serializable]
    public class MSG_RetStartFight_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "duration", DataFormat = DataFormat.TwosComplement)]
        public uint duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this._duration = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "score", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public MSG_RetTeamCurScore_SC score
        {
            get
            {
                return this._score;
            }
            set
            {
                this._score = value;
            }
        }

        private uint _duration;

        private MSG_RetTeamCurScore_SC _score;

        private IExtension extensionObject;
    }
}
