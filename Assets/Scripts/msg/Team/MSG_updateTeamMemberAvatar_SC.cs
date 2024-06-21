using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_updateTeamMemberAvatar_SC")]
    [Serializable]
    public class MSG_updateTeamMemberAvatar_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "mememberid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string mememberid
        {
            get
            {
                return this._mememberid;
            }
            set
            {
                this._mememberid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "hairstyle", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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
        [ProtoMember(3, IsRequired = false, Name = "haircolor", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(4, IsRequired = false, Name = "headstyle", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint headstyle
        {
            get
            {
                return this._headstyle;
            }
            set
            {
                this._headstyle = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "bodystyle", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(6, IsRequired = false, Name = "antenna", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(7, IsRequired = false, Name = "coat", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint coat
        {
            get
            {
                return this._coat;
            }
            set
            {
                this._coat = value;
            }
        }

        [ProtoMember(8, IsRequired = false, Name = "avatarid", DataFormat = DataFormat.TwosComplement)]
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

        private string _mememberid = string.Empty;

        private uint _hairstyle;

        private uint _haircolor;

        private uint _headstyle;

        private uint _bodystyle;

        private uint _antenna;

        private uint _coat;

        private uint _avatarid;

        private IExtension extensionObject;
    }
}
