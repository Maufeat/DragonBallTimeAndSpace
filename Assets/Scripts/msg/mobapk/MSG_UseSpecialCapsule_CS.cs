using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_UseSpecialCapsule_CS")]
    [Serializable]
    public class MSG_UseSpecialCapsule_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "x", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint x
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "y", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }

        private string _thisid = string.Empty;

        private uint _x;

        private uint _y;

        private IExtension extensionObject;
    }
}
