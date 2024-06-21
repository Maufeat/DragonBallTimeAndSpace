using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "BlackItem")]
    [Serializable]
    public class BlackItem : IExtensible
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

        [ProtoMember(3, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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
        [ProtoMember(4, IsRequired = false, Name = "viplevel", DataFormat = DataFormat.TwosComplement)]
        public uint viplevel
        {
            get
            {
                return this._viplevel;
            }
            set
            {
                this._viplevel = value;
            }
        }

        private ulong _charid;

        private string _name = string.Empty;

        private uint _level;

        private uint _viplevel;

        private IExtension extensionObject;
    }
}
