using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "PowerItem")]
    [Serializable]
    public class PowerItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "color", DataFormat = DataFormat.Default)]
        public string color
        {
            get
            {
                return this._color;
            }
            set
            {
                this._color = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "power", DataFormat = DataFormat.TwosComplement)]
        public uint power
        {
            get
            {
                return this._power;
            }
            set
            {
                this._power = value;
            }
        }

        private string _color = string.Empty;

        private uint _power;

        private IExtension extensionObject;
    }
}
