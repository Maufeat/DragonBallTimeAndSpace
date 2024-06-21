using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_AnswerJoinTeam_SC")]
    [Serializable]
    public class MSG_AnswerJoinTeam_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "errcode", DataFormat = DataFormat.TwosComplement)]
        public uint errcode
        {
            get
            {
                return this._errcode;
            }
            set
            {
                this._errcode = value;
            }
        }

        [DefaultValue(AnswerType.AnswerType_Yes)]
        [ProtoMember(2, IsRequired = false, Name = "answer_type", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(3, IsRequired = false, Name = "teaminfo", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public MSG_TeamMemeberList_SC teaminfo
        {
            get
            {
                return this._teaminfo;
            }
            set
            {
                this._teaminfo = value;
            }
        }

        private uint _errcode;

        private AnswerType _answer_type = AnswerType.AnswerType_Yes;

        private MSG_TeamMemeberList_SC _teaminfo;

        private IExtension extensionObject;
    }
}
