using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "relation_item")]
    [Serializable]
    public class relation_item : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "relationid", DataFormat = DataFormat.TwosComplement)]
        public ulong relationid
        {
            get
            {
                return this._relationid;
            }
            set
            {
                this._relationid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "relationname", DataFormat = DataFormat.Default)]
        public string relationname
        {
            get
            {
                return this._relationname;
            }
            set
            {
                this._relationname = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "career", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint career
        {
            get
            {
                return this._career;
            }
            set
            {
                this._career = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint type
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

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "status", DataFormat = DataFormat.TwosComplement)]
        public uint status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "love_degree", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint love_degree
        {
            get
            {
                return this._love_degree;
            }
            set
            {
                this._love_degree = value;
            }
        }

        [ProtoMember(8, IsRequired = false, Name = "lastchattime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lastchattime
        {
            get
            {
                return this._lastchattime;
            }
            set
            {
                this._lastchattime = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(9, IsRequired = false, Name = "page", DataFormat = DataFormat.Default)]
        public string page
        {
            get
            {
                return this._page;
            }
            set
            {
                this._page = value;
            }
        }

        [ProtoMember(10, IsRequired = false, Name = "nickName", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string nickName
        {
            get
            {
                return this._nickName;
            }
            set
            {
                this._nickName = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(11, IsRequired = false, Name = "offlineTime", DataFormat = DataFormat.TwosComplement)]
        public uint offlineTime
        {
            get
            {
                return this._offlineTime;
            }
            set
            {
                this._offlineTime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(12, IsRequired = false, Name = "headPic", DataFormat = DataFormat.TwosComplement)]
        public uint headPic
        {
            get
            {
                return this._headPic;
            }
            set
            {
                this._headPic = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(13, IsRequired = false, Name = "viplevel", DataFormat = DataFormat.TwosComplement)]
        public uint viplevel
        {
            get
            {
                return this._viplevel;
            }
            set
            {
                this._viplevel = value;
            }
        }

        [ProtoMember(14, IsRequired = false, Name = "power", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint power
        {
            get
            {
                return this._power;
            }
            set
            {
                this._power = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(15, IsRequired = false, Name = "friendrate", DataFormat = DataFormat.TwosComplement)]
        public uint friendrate
        {
            get
            {
                return this._friendrate;
            }
            set
            {
                this._friendrate = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(16, IsRequired = false, Name = "createTime", DataFormat = DataFormat.TwosComplement)]
        public uint createTime
        {
            get
            {
                return this._createTime;
            }
            set
            {
                this._createTime = value;
            }
        }

        private ulong _relationid;

        private string _relationname = string.Empty;

        private uint _level;

        private uint _career;

        private uint _type;

        private uint _status;

        private uint _love_degree;

        private uint _lastchattime;

        private string _page = string.Empty;

        private string _nickName = string.Empty;

        private uint _offlineTime;

        private uint _headPic;

        private uint _viplevel;

        private uint _power;

        private uint _friendrate;

        private uint _createTime;

        private IExtension extensionObject;
    }
}
