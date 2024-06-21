using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_ReqRankPKList_CS")]
    [Serializable]
    public class MSG_ReqRankPKList_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(RankPKListType.RankPKListType_All)]
        [ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public RankPKListType type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        private RankPKListType _type;

        private IExtension extensionObject;
    }
}
