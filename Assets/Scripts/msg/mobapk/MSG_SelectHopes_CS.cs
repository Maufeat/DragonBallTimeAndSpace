using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_SelectHopes_CS")]
    [Serializable]
    public class MSG_SelectHopes_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "idx_1st", DataFormat = DataFormat.TwosComplement)]
        public uint idx_1st
        {
            get
            {
                return this._idx_1st;
            }
            set
            {
                this._idx_1st = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "idx_2nd", DataFormat = DataFormat.TwosComplement)]
        public uint idx_2nd
        {
            get
            {
                return this._idx_2nd;
            }
            set
            {
                this._idx_2nd = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "idx_3rd", DataFormat = DataFormat.TwosComplement)]
        public uint idx_3rd
        {
            get
            {
                return this._idx_3rd;
            }
            set
            {
                this._idx_3rd = value;
            }
        }

        private uint _idx_1st;

        private uint _idx_2nd;

        private uint _idx_3rd;

        private IExtension extensionObject;
    }
}
