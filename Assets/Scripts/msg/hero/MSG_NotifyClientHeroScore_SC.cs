using System;
using System.Collections.Generic;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_NotifyClientHeroScore_SC")]
    [Serializable]
    public class MSG_NotifyClientHeroScore_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "scores", DataFormat = DataFormat.Default)]
        public List<HeroScore> scores
        {
            get
            {
                return this._scores;
            }
        }

        private readonly List<HeroScore> _scores = new List<HeroScore>();

        private IExtension extensionObject;
    }
}
