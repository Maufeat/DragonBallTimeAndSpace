using System;
using System.Collections.Generic;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Ret_MagicAttack_SC")]
    [Serializable]
    public class MSG_Ret_MagicAttack_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "att", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public EntryIDType att
        {
            get
            {
                return this._att;
            }
            set
            {
                this._att = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "def", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public EntryIDType def
        {
            get
            {
                return this._def;
            }
            set
            {
                this._def = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "desx", DataFormat = DataFormat.FixedSize)]
        [DefaultValue(0f)]
        public float desx
        {
            get
            {
                return this._desx;
            }
            set
            {
                this._desx = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(4, IsRequired = false, Name = "desy", DataFormat = DataFormat.FixedSize)]
        public float desy
        {
            get
            {
                return this._desy;
            }
            set
            {
                this._desy = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "attdir", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint attdir
        {
            get
            {
                return this._attdir;
            }
            set
            {
                this._attdir = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "userdir", DataFormat = DataFormat.TwosComplement)]
        public uint userdir
        {
            get
            {
                return this._userdir;
            }
            set
            {
                this._userdir = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(7, IsRequired = false, Name = "skillstage", DataFormat = DataFormat.TwosComplement)]
        public ulong skillstage
        {
            get
            {
                return this._skillstage;
            }
            set
            {
                this._skillstage = value;
            }
        }

        [ProtoMember(8, Name = "pklist", DataFormat = DataFormat.Default)]
        public List<PKResult> pklist
        {
            get
            {
                return this._pklist;
            }
        }

        private EntryIDType _att;

        private EntryIDType _def;

        private float _desx;

        private float _desy;

        private uint _attdir;

        private uint _userdir;

        private ulong _skillstage;

        private readonly List<PKResult> _pklist = new List<PKResult>();

        private IExtension extensionObject;
    }
}
