using System;
using System.Collections.Generic;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_Ret_Survey_Info_SC")]
    [Serializable]
    public class MSG_Ret_Survey_Info_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "survey_id", DataFormat = DataFormat.TwosComplement)]
        public List<uint> survey_id
        {
            get
            {
                return this._survey_id;
            }
        }

        private readonly List<uint> _survey_id = new List<uint>();

        private IExtension extensionObject;
    }
}
