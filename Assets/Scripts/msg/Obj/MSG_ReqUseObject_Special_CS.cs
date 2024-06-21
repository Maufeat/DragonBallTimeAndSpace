using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqUseObject_Special_CS")]
    [Serializable]
    public class MSG_ReqUseObject_Special_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
        public string thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        [DefaultValue(1L)]
        [ProtoMember(2, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
        public uint num
        {
            get
            {
                return this._num;
            }
            set
            {
                this._num = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(3, IsRequired = false, Name = "npcid", DataFormat = DataFormat.TwosComplement)]
        public ulong npcid
        {
            get
            {
                return this._npcid;
            }
            set
            {
                this._npcid = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "posx", DataFormat = DataFormat.FixedSize)]
        [DefaultValue(0f)]
        public float posx
        {
            get
            {
                return this._posx;
            }
            set
            {
                this._posx = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "posy", DataFormat = DataFormat.FixedSize)]
        [DefaultValue(0f)]
        public float posy
        {
            get
            {
                return this._posy;
            }
            set
            {
                this._posy = value;
            }
        }

        private string _thisid = string.Empty;

        private uint _num = 1U;

        private ulong _npcid;

        private float _posx;

        private float _posy;

        private IExtension extensionObject;
    }
}
