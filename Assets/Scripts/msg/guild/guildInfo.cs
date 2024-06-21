using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "guildInfo")]
    [Serializable]
    public class guildInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "guildid", DataFormat = DataFormat.TwosComplement)]
        public ulong guildid
        {
            get
            {
                return this._guildid;
            }
            set
            {
                this._guildid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "guildname", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string guildname
        {
            get
            {
                return this._guildname;
            }
            set
            {
                this._guildname = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "guildlevel", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint guildlevel
        {
            get
            {
                return this._guildlevel;
            }
            set
            {
                this._guildlevel = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "active", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint active
        {
            get
            {
                return this._active;
            }
            set
            {
                this._active = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "mastername", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string mastername
        {
            get
            {
                return this._mastername;
            }
            set
            {
                this._mastername = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "exp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint exp
        {
            get
            {
                return this._exp;
            }
            set
            {
                this._exp = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "nextexp", DataFormat = DataFormat.TwosComplement)]
        public uint nextexp
        {
            get
            {
                return this._nextexp;
            }
            set
            {
                this._nextexp = value;
            }
        }

        [ProtoMember(8, IsRequired = false, Name = "salary", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint salary
        {
            get
            {
                return this._salary;
            }
            set
            {
                this._salary = value;
            }
        }

        [ProtoMember(9, IsRequired = false, Name = "sumcount", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint sumcount
        {
            get
            {
                return this._sumcount;
            }
            set
            {
                this._sumcount = value;
            }
        }

        [ProtoMember(10, IsRequired = false, Name = "onlinecount", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint onlinecount
        {
            get
            {
                return this._onlinecount;
            }
            set
            {
                this._onlinecount = value;
            }
        }

        [ProtoMember(11, IsRequired = false, Name = "countlimit", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint countlimit
        {
            get
            {
                return this._countlimit;
            }
            set
            {
                this._countlimit = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(12, IsRequired = false, Name = "notify", DataFormat = DataFormat.Default)]
        public string notify
        {
            get
            {
                return this._notify;
            }
            set
            {
                this._notify = value;
            }
        }

        [ProtoMember(13, IsRequired = false, Name = "countryid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint countryid
        {
            get
            {
                return this._countryid;
            }
            set
            {
                this._countryid = value;
            }
        }

        [ProtoMember(14, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint rank
        {
            get
            {
                return this._rank;
            }
            set
            {
                this._rank = value;
            }
        }

        [ProtoMember(15, Name = "posinfo", DataFormat = DataFormat.Default)]
        public List<GuildPositionInfo> posinfo
        {
            get
            {
                return this._posinfo;
            }
        }

        [ProtoMember(16, IsRequired = false, Name = "guildsetinfo", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string guildsetinfo
        {
            get
            {
                return this._guildsetinfo;
            }
            set
            {
                this._guildsetinfo = value;
            }
        }

        [ProtoMember(17, IsRequired = false, Name = "lastactive", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lastactive
        {
            get
            {
                return this._lastactive;
            }
            set
            {
                this._lastactive = value;
            }
        }

        [ProtoMember(18, IsRequired = false, Name = "lastcountlimit", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lastcountlimit
        {
            get
            {
                return this._lastcountlimit;
            }
            set
            {
                this._lastcountlimit = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(19, IsRequired = false, Name = "icon", DataFormat = DataFormat.Default)]
        public string icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                this._icon = value;
            }
        }

        [ProtoMember(20, IsRequired = false, Name = "salary_rest_day", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint salary_rest_day
        {
            get
            {
                return this._salary_rest_day;
            }
            set
            {
                this._salary_rest_day = value;
            }
        }

        private ulong _guildid;

        private string _guildname = string.Empty;

        private uint _guildlevel;

        private uint _active;

        private string _mastername = string.Empty;

        private uint _exp;

        private uint _nextexp;

        private uint _salary;

        private uint _sumcount;

        private uint _onlinecount;

        private uint _countlimit;

        private string _notify = string.Empty;

        private uint _countryid;

        private uint _rank;

        private readonly List<GuildPositionInfo> _posinfo = new List<GuildPositionInfo>();

        private string _guildsetinfo = string.Empty;

        private uint _lastactive;

        private uint _lastcountlimit;

        private string _icon = string.Empty;

        private uint _salary_rest_day;

        private IExtension extensionObject;
    }
}
