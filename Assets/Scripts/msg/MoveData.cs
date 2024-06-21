using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MoveData")]
    [Serializable]
    public class MoveData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "pos", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public FloatMovePos pos
        {
            get
            {
                return this._pos;
            }
            set
            {
                this._pos = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "dir", DataFormat = DataFormat.TwosComplement)]
        public uint dir
        {
            get
            {
                return this._dir;
            }
            set
            {
                this._dir = value;
            }
        }

        private FloatMovePos _pos;

        private uint _dir;

        private IExtension extensionObject;
    }
}
