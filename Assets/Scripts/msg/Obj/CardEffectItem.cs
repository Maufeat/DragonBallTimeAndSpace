using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "CardEffectItem")]
    [Serializable]
    public class CardEffectItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        public uint id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "trigger", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint trigger
        {
            get
            {
                return this._trigger;
            }
            set
            {
                this._trigger = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "value", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "varname", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string varname
        {
            get
            {
                return this._varname;
            }
            set
            {
                this._varname = value;
            }
        }

        private uint _id;

        private uint _trigger;

        private uint _value;

        private string _varname = string.Empty;

        private IExtension extensionObject;
    }
}
