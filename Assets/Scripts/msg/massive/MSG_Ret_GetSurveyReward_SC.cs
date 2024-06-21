using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_Ret_GetSurveyReward_SC")]
    [Serializable]
    public class MSG_Ret_GetSurveyReward_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "survey_id", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        public uint retcode
        {
            get
            {
                return this._retcode;
            }
            set
            {
                this._retcode = value;
            }
        }

        private uint _survey_id;

        private uint _retcode;

        private IExtension extensionObject;
    }
}
