using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Ret_SyncSkillStage_SC")]
    [Serializable]
    public class MSG_Ret_SyncSkillStage_SC : IExtensible
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

        [DefaultValue(null)]
        [ProtoMember(2, IsRequired = false, Name = "def", DataFormat = DataFormat.Default)]
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

        [ProtoMember(3, IsRequired = false, Name = "skillstage", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
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

        [DefaultValue(0f)]
        [ProtoMember(4, IsRequired = false, Name = "desx", DataFormat = DataFormat.FixedSize)]
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
        [ProtoMember(5, IsRequired = false, Name = "desy", DataFormat = DataFormat.FixedSize)]
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

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "attdir", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(7, IsRequired = false, Name = "userdir", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(8, IsRequired = false, Name = "stagetype", DataFormat = DataFormat.TwosComplement)]
        public uint stagetype
        {
            get
            {
                return this._stagetype;
            }
            set
            {
                this._stagetype = value;
            }
        }

        private EntryIDType _att;

        private EntryIDType _def;

        private ulong _skillstage;

        private float _desx;

        private float _desy;

        private uint _attdir;

        private uint _userdir;

        private uint _stagetype;

        private IExtension extensionObject;
    }
}
