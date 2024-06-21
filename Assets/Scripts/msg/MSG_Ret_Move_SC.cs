using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_Move_SC")]
    [Serializable]
    public class MSG_Ret_Move_SC : IExtensible
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

        [ProtoMember(2, Name = "movedata", DataFormat = DataFormat.Default)]
        public List<MoveData> movedata
        {
            get
            {
                return this._movedata;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "steplength", DataFormat = DataFormat.TwosComplement)]
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

        private readonly List<MoveData> _movedata = new List<MoveData>();

        private uint _steplength;

        private IExtension extensionObject;
    }
}
