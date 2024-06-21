using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Req_Move_CS")]
    [Serializable]
    public class MSG_Req_Move_CS : IExtensible
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

        [ProtoMember(3, IsRequired = false, Name = "steplenth", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint steplenth
        {
            get
            {
                return this._steplenth;
            }
            set
            {
                this._steplenth = value;
            }
        }

        private ulong _charid;

        private readonly List<MoveData> _movedata = new List<MoveData>();

        private uint _steplenth;

        private IExtension extensionObject;
    }
}
