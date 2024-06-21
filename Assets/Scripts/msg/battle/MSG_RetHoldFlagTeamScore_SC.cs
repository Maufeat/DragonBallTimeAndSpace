using System;
using System.Collections.Generic;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_RetHoldFlagTeamScore_SC")]
    [Serializable]
    public class MSG_RetHoldFlagTeamScore_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "campScore", DataFormat = DataFormat.Default)]
        public List<HoldFlagCampScore> campScore
        {
            get
            {
                return this._campScore;
            }
        }

        private readonly List<HoldFlagCampScore> _campScore = new List<HoldFlagCampScore>();

        private IExtension extensionObject;
    }
}
