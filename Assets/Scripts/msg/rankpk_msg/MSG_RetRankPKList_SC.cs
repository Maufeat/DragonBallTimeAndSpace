using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RetRankPKList_SC")]
    [Serializable]
    public class MSG_RetRankPKList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(RankPKListType.RankPKListType_All)]
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

        [ProtoMember(2, IsRequired = false, Name = "myposition", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint myposition
        {
            get
            {
                return this._myposition;
            }
            set
            {
                this._myposition = value;
            }
        }

        [ProtoMember(3, Name = "data", DataFormat = DataFormat.Default)]
        public List<RankPKListItem> data
        {
            get
            {
                return this._data;
            }
        }

        private RankPKListType _type;

        private uint _myposition;

        private readonly List<RankPKListItem> _data = new List<RankPKListItem>();

        private IExtension extensionObject;
    }
}
