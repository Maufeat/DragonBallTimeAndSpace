using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Req_MagicAttack_CS")]
    [Serializable]
    public class MSG_Req_MagicAttack_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "target", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public EntryIDType target
        {
            get
            {
                return this._target;
            }
            set
            {
                this._target = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "magictype", DataFormat = DataFormat.TwosComplement)]
        public uint magictype
        {
            get
            {
                return this._magictype;
            }
            set
            {
                this._magictype = value;
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

        [ProtoMember(4, IsRequired = false, Name = "desy", DataFormat = DataFormat.FixedSize)]
        [DefaultValue(0f)]
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

        private EntryIDType _target;

        private uint _magictype;

        private float _desx;

        private float _desy;

        private uint _attdir;

        private uint _userdir;

        private IExtension extensionObject;
    }
}
