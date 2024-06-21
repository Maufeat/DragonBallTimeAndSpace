using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RetRankPKCurStage_SC")]
    [Serializable]
    public class MSG_RetRankPKCurStage_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "curstage", DataFormat = DataFormat.TwosComplement)]
        public StageType curstage
        {
            get
            {
                return this._curstage;
            }
            set
            {
                this._curstage = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "duration", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(3, IsRequired = false, Name = "score", DataFormat = DataFormat.Default)]
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

        private StageType _curstage;

        private uint _duration;

        private MSG_RetTeamCurScore_SC _score;

        private IExtension extensionObject;
    }
}
