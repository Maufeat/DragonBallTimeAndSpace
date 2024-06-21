using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_Req_GetSurveyReward_CS")]
    [Serializable]
    public class MSG_Req_GetSurveyReward_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "survey_id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint survey_id
        {
            get
            {
                return this._survey_id;
            }
            set
            {
                this._survey_id = value;
            }
        }

        private uint _survey_id;

        private IExtension extensionObject;
    }
}
