using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "EquipRandInfo")]
    [Serializable]
    public class EquipRandInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(Equip8Prop.BASIC_PROP)]
        [ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public Equip8Prop type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "value", DataFormat = DataFormat.TwosComplement)]
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

        private Equip8Prop _type = Equip8Prop.BASIC_PROP;

        private uint _id;

        private uint _value;

        private IExtension extensionObject;
    }
}
