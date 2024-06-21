using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "VitalityUserInfo")]
    [Serializable]
    public class VitalityUserInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "totaldegree", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint totaldegree
        {
            get
            {
                return this._totaldegree;
            }
            set
            {
                this._totaldegree = value;
            }
        }

        [ProtoMember(2, Name = "item", DataFormat = DataFormat.Default)]
        public List<VitalityItem> item
        {
            get
            {
                return this._item;
            }
        }

        [ProtoMember(3, Name = "reward", DataFormat = DataFormat.Default)]
        public List<VitalityReward> reward
        {
            get
            {
                return this._reward;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "attendtime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint attendtime
        {
            get
            {
                return this._attendtime;
            }
            set
            {
                this._attendtime = value;
            }
        }

        private uint _totaldegree;

        private readonly List<VitalityItem> _item = new List<VitalityItem>();

        private readonly List<VitalityReward> _reward = new List<VitalityReward>();

        private uint _attendtime;

        private IExtension extensionObject;
    }
}
