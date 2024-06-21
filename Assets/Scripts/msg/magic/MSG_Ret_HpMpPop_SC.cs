using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Ret_HpMpPop_SC")]
    [Serializable]
    public class MSG_Ret_HpMpPop_SC : IExtensible
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

        [ProtoMember(3, IsRequired = false, Name = "hp_change", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0)]
        public int hp_change
        {
            get
            {
                return this._hp_change;
            }
            set
            {
                this._hp_change = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "mp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint mp
        {
            get
            {
                return this._mp;
            }
            set
            {
                this._mp = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "mp_change", DataFormat = DataFormat.TwosComplement)]
        public uint mp_change
        {
            get
            {
                return this._mp_change;
            }
            set
            {
                this._mp_change = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "force", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool force
        {
            get
            {
                return this._force;
            }
            set
            {
                this._force = value;
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

        [DefaultValue(0f)]
        [ProtoMember(8, IsRequired = false, Name = "state_id", DataFormat = DataFormat.TwosComplement)]
        public ulong state_id
        {
            get
            {
                return this._state_id;
            }
            set
            {
                this._state_id = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(9, IsRequired = false, Name = "att", DataFormat = DataFormat.Default)]
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

        private EntryIDType _target;

        private uint _hp;

        private int _hp_change;

        private uint _mp;

        private uint _mp_change;

        private bool _force;

        private ulong _skillstage;

        private ulong _state_id;

        private EntryIDType _att;

        private IExtension extensionObject;
    }
}
