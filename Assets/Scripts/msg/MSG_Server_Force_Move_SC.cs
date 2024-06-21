using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Server_Force_Move_SC")]
    [Serializable]
    public class MSG_Server_Force_Move_SC : IExtensible
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

        [ProtoMember(2, Name = "movedata", DataFormat = DataFormat.Default)]
        public List<MoveData> movedata
        {
            get
            {
                return this._movedata;
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

        private readonly List<MoveData> _movedata = new List<MoveData>();

        private uint _steplength;

        private IExtension extensionObject;
    }
}
