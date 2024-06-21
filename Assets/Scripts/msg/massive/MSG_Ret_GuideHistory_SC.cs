using System;
using System.Collections.Generic;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_Ret_GuideHistory_SC")]
    [Serializable]
    public class MSG_Ret_GuideHistory_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "guideids", DataFormat = DataFormat.TwosComplement)]
        public List<uint> guideids
        {
            get
            {
                return this._guideids;
            }
        }

        private readonly List<uint> _guideids = new List<uint>();

        private IExtension extensionObject;
    }
}
