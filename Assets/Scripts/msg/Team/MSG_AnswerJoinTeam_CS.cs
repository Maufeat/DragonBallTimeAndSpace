using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_AnswerJoinTeam_CS")]
    [Serializable]
    public class MSG_AnswerJoinTeam_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "requesterid", DataFormat = DataFormat.Default)]
        public string requesterid
        {
            get
            {
                return this._requesterid;
            }
            set
            {
                this._requesterid = value;
            }
        }

        [ProtoMember(2, IsRequired = true, Name = "answer_type", DataFormat = DataFormat.TwosComplement)]
        public AnswerType answer_type
        {
            get
            {
                return this._answer_type;
            }
            set
            {
                this._answer_type = value;
            }
        }

        private string _requesterid;

        private AnswerType _answer_type;

        private IExtension extensionObject;
    }
}
