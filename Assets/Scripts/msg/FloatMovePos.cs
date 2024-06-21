using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "FloatMovePos")]
    [Serializable]
    public class FloatMovePos : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "fx", DataFormat = DataFormat.FixedSize)]
        public float fx
        {
            get
            {
                return this._fx;
            }
            set
            {
                this._fx = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "fy", DataFormat = DataFormat.FixedSize)]
        public float fy
        {
            get
            {
                return this._fy;
            }
            set
            {
                this._fy = value;
            }
        }

        private float _fx;

        private float _fy;

        private IExtension extensionObject;
    }
}
