using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_TeamMemeberList_SC")]
    [Serializable]
    public class MSG_TeamMemeberList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "leaderid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string leaderid
        {
            get
            {
                return this._leaderid;
            }
            set
            {
                this._leaderid = value;
            }
        }

        [ProtoMember(4, Name = "mem", DataFormat = DataFormat.Default)]
        public List<Memember> mem
        {
            get
            {
                return this._mem;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "mode", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(RewardMode.Mode_Roll)]
        public RewardMode mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
            }
        }

        [DefaultValue(CapacityType.Capacity_Small)]
        [ProtoMember(6, IsRequired = false, Name = "cap_type", DataFormat = DataFormat.TwosComplement)]
        public CapacityType cap_type
        {
            get
            {
                return this._cap_type;
            }
            set
            {
                this._cap_type = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "maxmember", DataFormat = DataFormat.TwosComplement)]
        public uint maxmember
        {
            get
            {
                return this._maxmember;
            }
            set
            {
                this._maxmember = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(8, IsRequired = false, Name = "note", DataFormat = DataFormat.Default)]
        public string note
        {
            get
            {
                return this._note;
            }
            set
            {
                this._note = value;
            }
        }

        [ProtoMember(9, IsRequired = false, Name = "activityid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint activityid
        {
            get
            {
                return this._activityid;
            }
            set
            {
                this._activityid = value;
            }
        }

        [ProtoMember(10, IsRequired = false, Name = "curmember", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint curmember
        {
            get
            {
                return this._curmember;
            }
            set
            {
                this._curmember = value;
            }
        }

        private uint _id;

        private string _name = string.Empty;

        private string _leaderid = string.Empty;

        private readonly List<Memember> _mem = new List<Memember>();

        private RewardMode _mode = RewardMode.Mode_Roll;

        private CapacityType _cap_type = CapacityType.Capacity_Small;

        private uint _maxmember;

        private string _note = string.Empty;

        private uint _activityid;

        private uint _curmember;

        private IExtension extensionObject;
    }
}
