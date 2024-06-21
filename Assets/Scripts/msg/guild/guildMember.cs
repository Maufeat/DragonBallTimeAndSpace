using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "guildMember")]
    [Serializable]
    public class guildMember : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "memberid", DataFormat = DataFormat.TwosComplement)]
        public ulong memberid
        {
            get
            {
                return this._memberid;
            }
            set
            {
                this._memberid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "membername", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string membername
        {
            get
            {
                return this._membername;
            }
            set
            {
                this._membername = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "memberlevel", DataFormat = DataFormat.TwosComplement)]
        public uint memberlevel
        {
            get
            {
                return this._memberlevel;
            }
            set
            {
                this._memberlevel = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "memberstatus", DataFormat = DataFormat.TwosComplement)]
        public uint memberstatus
        {
            get
            {
                return this._memberstatus;
            }
            set
            {
                this._memberstatus = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "contribute", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint contribute
        {
            get
            {
                return this._contribute;
            }
            set
            {
                this._contribute = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "donatesalary", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint donatesalary
        {
            get
            {
                return this._donatesalary;
            }
            set
            {
                this._donatesalary = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "isonline", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool isonline
        {
            get
            {
                return this._isonline;
            }
            set
            {
                this._isonline = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "lastonlinetime", DataFormat = DataFormat.TwosComplement)]
        public uint lastonlinetime
        {
            get
            {
                return this._lastonlinetime;
            }
            set
            {
                this._lastonlinetime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(9, IsRequired = false, Name = "career", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(10, IsRequired = false, Name = "positionid", DataFormat = DataFormat.TwosComplement)]
        public uint positionid
        {
            get
            {
                return this._positionid;
            }
            set
            {
                this._positionid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(11, IsRequired = false, Name = "applytime", DataFormat = DataFormat.TwosComplement)]
        public uint applytime
        {
            get
            {
                return this._applytime;
            }
            set
            {
                this._applytime = value;
            }
        }

        private ulong _memberid;

        private string _membername = string.Empty;

        private uint _memberlevel;

        private uint _memberstatus;

        private uint _contribute;

        private uint _donatesalary;

        private bool _isonline;

        private uint _lastonlinetime;

        private uint _career;

        private uint _positionid;

        private uint _applytime;

        private IExtension extensionObject;
    }
}
