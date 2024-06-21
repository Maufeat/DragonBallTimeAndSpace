using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_updateTeamMememberHp_SC")]
    [Serializable]
    public class MSG_updateTeamMememberHp_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "memid", DataFormat = DataFormat.Default)]
        public string memid
        {
            get
            {
                return this._memid;
            }
            set
            {
                this._memid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "hp", DataFormat = DataFormat.TwosComplement)]
        public uint hp
        {
            get
            {
                return this._hp;
            }
            set
            {
                this._hp = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "maxhp", DataFormat = DataFormat.TwosComplement)]
        public uint maxhp
        {
            get
            {
                return this._maxhp;
            }
            set
            {
                this._maxhp = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "isdead", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool isdead
        {
            get
            {
                return this._isdead;
            }
            set
            {
                this._isdead = value;
            }
        }

        private string _memid = string.Empty;

        private uint _hp;

        private uint _maxhp;

        private bool _isdead;

        private IExtension extensionObject;
    }
}
