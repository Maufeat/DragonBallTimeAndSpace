using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_RetHpMpToSelects_SC")]
    [Serializable]
    public class MSG_RetHpMpToSelects_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "choosen", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public EntryIDType choosen
        {
            get
            {
                return this._choosen;
            }
            set
            {
                this._choosen = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "curhp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint curhp
        {
            get
            {
                return this._curhp;
            }
            set
            {
                this._curhp = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "maxhp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(4, IsRequired = false, Name = "curmp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint curmp
        {
            get
            {
                return this._curmp;
            }
            set
            {
                this._curmp = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "maxmp", DataFormat = DataFormat.TwosComplement)]
        public uint maxmp
        {
            get
            {
                return this._maxmp;
            }
            set
            {
                this._maxmp = value;
            }
        }

        private EntryIDType _choosen;

        private uint _curhp;

        private uint _maxhp;

        private uint _curmp;

        private uint _maxmp;

        private IExtension extensionObject;
    }
}
