using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_RetNpcMove_SC")]
    [Serializable]
    public class MSG_Ret_RetNpcMove_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "tempid", DataFormat = DataFormat.TwosComplement)]
        public ulong tempid
        {
            get
            {
                return this._tempid;
            }
            set
            {
                this._tempid = value;
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

        [ProtoMember(3, IsRequired = false, Name = "speed", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint speed
        {
            get
            {
                return this._speed;
            }
            set
            {
                this._speed = value;
            }
        }

        private ulong _tempid;

        private readonly List<MoveData> _movedata = new List<MoveData>();

        private uint _speed;

        private IExtension extensionObject;
    }
}
