using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_Move_Failed_SC")]
    [Serializable]
    public class MSG_Ret_Move_Failed_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(null)]
        [ProtoMember(2, IsRequired = false, Name = "movedata", DataFormat = DataFormat.Default)]
        public MoveData movedata
        {
            get
            {
                return this._movedata;
            }
            set
            {
                this._movedata = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "steplength", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint steplength
        {
            get
            {
                return this._steplength;
            }
            set
            {
                this._steplength = value;
            }
        }

        private ulong _charid;

        private MoveData _movedata;

        private uint _steplength;

        private IExtension extensionObject;
    }
}
