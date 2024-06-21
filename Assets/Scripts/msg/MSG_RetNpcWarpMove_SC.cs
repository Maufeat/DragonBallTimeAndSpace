using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_RetNpcWarpMove_SC")]
    [Serializable]
    public class MSG_RetNpcWarpMove_SC : IExtensible
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

        private ulong _tempid;

        private MoveData _movedata;

        private IExtension extensionObject;
    }
}
