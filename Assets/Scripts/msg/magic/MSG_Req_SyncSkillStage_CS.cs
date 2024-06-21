using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Req_SyncSkillStage_CS")]
    [Serializable]
    public class MSG_Req_SyncSkillStage_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "target", DataFormat = DataFormat.Default)]
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

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "skillstage", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "attdir", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(6, IsRequired = false, Name = "userdir", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(7, IsRequired = false, Name = "stagetype", DataFormat = DataFormat.TwosComplement)]
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

        private EntryIDType _target;

        private ulong _skillstage;

        private float _desx;

        private float _desy;

        private uint _attdir;

        private uint _userdir;

        private uint _stagetype;

        private IExtension extensionObject;
    }
}
