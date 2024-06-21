using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_AttackTargetChange_SC")]
    [Serializable]
    public class MSG_AttackTargetChange_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
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

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "HP", DataFormat = DataFormat.TwosComplement)]
        public uint HP
        {
            get
            {
                return this._HP;
            }
            set
            {
                this._HP = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "relation", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint relation
        {
            get
            {
                return this._relation;
            }
            set
            {
                this._relation = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "choosetype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(ChooseTargetType.CHOOSE_TARGE_TTYPE_SET)]
        public ChooseTargetType choosetype
        {
            get
            {
                return this._choosetype;
            }
            set
            {
                this._choosetype = value;
            }
        }

        private ulong _charid;

        private string _name = string.Empty;

        private uint _level;

        private uint _HP;

        private uint _relation;

        private ChooseTargetType _choosetype = ChooseTargetType.CHOOSE_TARGE_TTYPE_SET;

        private IExtension extensionObject;
    }
}
