using System;
using System.Collections.Generic;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_RewardBagInfo_SC")]
    [Serializable]
    public class MSG_RewardBagInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "infos", DataFormat = DataFormat.Default)]
        public List<GetBagInfo> infos
        {
            get
            {
                return this._infos;
            }
        }

        private readonly List<GetBagInfo> _infos = new List<GetBagInfo>();

        private IExtension extensionObject;
    }
}
