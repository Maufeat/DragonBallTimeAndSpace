using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_Ret_QuestInfo_SC")]
    [Serializable]
    public class MSG_Ret_QuestInfo_SC : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "score", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint score
        {
            get
            {
                return this._score;
            }
            set
            {
                this._score = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "curvalue", DataFormat = DataFormat.TwosComplement)]
        public uint curvalue
        {
            get
            {
                return this._curvalue;
            }
            set
            {
                this._curvalue = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "maxvalue", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint maxvalue
        {
            get
            {
                return this._maxvalue;
            }
            set
            {
                this._maxvalue = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "starttime", DataFormat = DataFormat.TwosComplement)]
        public uint starttime
        {
            get
            {
                return this._starttime;
            }
            set
            {
                this._starttime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "cur_extvalue", DataFormat = DataFormat.TwosComplement)]
        public uint cur_extvalue
        {
            get
            {
                return this._cur_extvalue;
            }
            set
            {
                this._cur_extvalue = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "max_extvalue", DataFormat = DataFormat.TwosComplement)]
        public uint max_extvalue
        {
            get
            {
                return this._max_extvalue;
            }
            set
            {
                this._max_extvalue = value;
            }
        }

        [ProtoMember(9, IsRequired = false, Name = "leftsecs", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0)]
        public int leftsecs
        {
            get
            {
                return this._leftsecs;
            }
            set
            {
                this._leftsecs = value;
            }
        }

        [ProtoMember(10, Name = "extinfo", DataFormat = DataFormat.Default)]
        public List<UnorderQuestBranchInfo> extinfo
        {
            get
            {
                return this._extinfo;
            }
        }

        [ProtoMember(11, IsRequired = false, Name = "show", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool show
        {
            get
            {
                return this._show;
            }
            set
            {
                this._show = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(12, IsRequired = false, Name = "discount", DataFormat = DataFormat.Default)]
        public bool discount
        {
            get
            {
                return this._discount;
            }
            set
            {
                this._discount = value;
            }
        }

        private uint _id;

        private uint _state;

        private uint _score;

        private uint _curvalue;

        private uint _maxvalue;

        private uint _starttime;

        private uint _cur_extvalue;

        private uint _max_extvalue;

        private int _leftsecs;

        private readonly List<UnorderQuestBranchInfo> _extinfo = new List<UnorderQuestBranchInfo>();

        private bool _show;

        private bool _discount;

        private IExtension extensionObject;
    }
}
