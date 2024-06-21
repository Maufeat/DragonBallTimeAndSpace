using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_Find_Path_SC")]
    [Serializable]
    public class MSG_Ret_Find_Path_SC : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "gridindex", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint gridindex
        {
            get
            {
                return this._gridindex;
            }
            set
            {
                this._gridindex = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "moveres", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint moveres
        {
            get
            {
                return this._moveres;
            }
            set
            {
                this._moveres = value;
            }
        }

        private ulong _charid;

        private uint _gridindex;

        private uint _moveres;

        private IExtension extensionObject;
    }
}
