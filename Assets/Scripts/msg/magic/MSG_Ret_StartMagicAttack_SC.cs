using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Ret_StartMagicAttack_SC")]
    [Serializable]
    public class MSG_Ret_StartMagicAttack_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "att", DataFormat = DataFormat.Default)]
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

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "desx", DataFormat = DataFormat.FixedSize)]
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
        [ProtoMember(3, IsRequired = false, Name = "desy", DataFormat = DataFormat.FixedSize)]
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

        [ProtoMember(4, IsRequired = false, Name = "attdir", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(5, IsRequired = false, Name = "userdir", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "skillid", DataFormat = DataFormat.TwosComplement)]
        public uint skillid
        {
            get
            {
                return this._skillid;
            }
            set
            {
                this._skillid = value;
            }
        }

        private EntryIDType _att;

        private float _desx;

        private float _desy;

        private uint _attdir;

        private uint _userdir;

        private uint _skillid;

        private IExtension extensionObject;
    }
}
