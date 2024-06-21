using System;
using System.Collections.Generic;
using ProtoBuf;

namespace career
{
    [ProtoContract(Name = "MSG_NoneChantSkill_SC")]
    [Serializable]
    public class MSG_NoneChantSkill_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "arrskill", DataFormat = DataFormat.TwosComplement)]
        public List<uint> arrskill
        {
            get
            {
                return this._arrskill;
            }
        }

        private readonly List<uint> _arrskill = new List<uint>();

        private IExtension extensionObject;
    }
}
