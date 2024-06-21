using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "SelectUserInfo")]
    [Serializable]
    public class SelectUserInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        public ulong charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
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

        [ProtoMember(4, IsRequired = true, Name = "sex", DataFormat = DataFormat.TwosComplement)]
        public SEX sex
        {
            get
            {
                return this._sex;
            }
            set
            {
                this._sex = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "heroid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint heroid
        {
            get
            {
                return this._heroid;
            }
            set
            {
                this._heroid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "curheroid", DataFormat = DataFormat.TwosComplement)]
        public uint curheroid
        {
            get
            {
                return this._curheroid;
            }
            set
            {
                this._curheroid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "delTime", DataFormat = DataFormat.TwosComplement)]
        public uint delTime
        {
            get
            {
                return this._delTime;
            }
            set
            {
                this._delTime = value;
            }
        }

        [ProtoMember(8, IsRequired = false, Name = "offlinetime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint offlinetime
        {
            get
            {
                return this._offlinetime;
            }
            set
            {
                this._offlinetime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(9, IsRequired = false, Name = "facestyle", DataFormat = DataFormat.TwosComplement)]
        public uint facestyle
        {
            get
            {
                return this._facestyle;
            }
            set
            {
                this._facestyle = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(10, IsRequired = false, Name = "hairstyle", DataFormat = DataFormat.TwosComplement)]
        public uint hairstyle
        {
            get
            {
                return this._hairstyle;
            }
            set
            {
                this._hairstyle = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(11, IsRequired = false, Name = "haircolor", DataFormat = DataFormat.TwosComplement)]
        public uint haircolor
        {
            get
            {
                return this._haircolor;
            }
            set
            {
                this._haircolor = value;
            }
        }

        [ProtoMember(12, IsRequired = false, Name = "bodystyle", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint bodystyle
        {
            get
            {
                return this._bodystyle;
            }
            set
            {
                this._bodystyle = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(13, IsRequired = false, Name = "antenna", DataFormat = DataFormat.TwosComplement)]
        public uint antenna
        {
            get
            {
                return this._antenna;
            }
            set
            {
                this._antenna = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(14, IsRequired = false, Name = "onlinelasttime", DataFormat = DataFormat.TwosComplement)]
        public uint onlinelasttime
        {
            get
            {
                return this._onlinelasttime;
            }
            set
            {
                this._onlinelasttime = value;
            }
        }

        [ProtoMember(15, IsRequired = false, Name = "addictpreuptime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint addictpreuptime
        {
            get
            {
                return this._addictpreuptime;
            }
            set
            {
                this._addictpreuptime = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(16, IsRequired = false, Name = "mapname", DataFormat = DataFormat.Default)]
        public string mapname
        {
            get
            {
                return this._mapname;
            }
            set
            {
                this._mapname = value;
            }
        }

        [ProtoMember(17, IsRequired = false, Name = "avatarid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint avatarid
        {
            get
            {
                return this._avatarid;
            }
            set
            {
                this._avatarid = value;
            }
        }

        private ulong _charid;

        private string _name = string.Empty;

        private uint _level;

        private SEX _sex;

        private uint _heroid;

        private uint _curheroid;

        private uint _delTime;

        private uint _offlinetime;

        private uint _facestyle;

        private uint _hairstyle;

        private uint _haircolor;

        private uint _bodystyle;

        private uint _antenna;

        private uint _onlinelasttime;

        private uint _addictpreuptime;

        private string _mapname = string.Empty;

        private uint _avatarid;

        private IExtension extensionObject;
    }
}
