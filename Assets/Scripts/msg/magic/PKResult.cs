using System;
using System.Collections.Generic;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "PKResult")]
    [Serializable]
    public class PKResult : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "def", DataFormat = DataFormat.Default)]
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

        [ProtoMember(2, IsRequired = false, Name = "hp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [DefaultValue(0)]
        [ProtoMember(3, IsRequired = false, Name = "changehp", DataFormat = DataFormat.TwosComplement)]
        public int changehp
        {
            get
            {
                return this._changehp;
            }
            set
            {
                this._changehp = value;
            }
        }

        [ProtoMember(4, Name = "attcode", DataFormat = DataFormat.TwosComplement)]
        public List<ATTACKRESULT> attcode
        {
            get
            {
                return this._attcode;
            }
        }

        private EntryIDType _def;

        private uint _hp;

        private int _changehp;

        private readonly List<ATTACKRESULT> _attcode = new List<ATTACKRESULT>();

        private IExtension extensionObject;
    }
}
