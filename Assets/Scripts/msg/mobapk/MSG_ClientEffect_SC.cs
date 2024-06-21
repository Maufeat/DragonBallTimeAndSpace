using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_ClientEffect_SC")]
    [Serializable]
    public class MSG_ClientEffect_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "uid", DataFormat = DataFormat.TwosComplement)]
        public ulong uid
        {
            get
            {
                return this._uid;
            }
            set
            {
                this._uid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "effectid", DataFormat = DataFormat.TwosComplement)]
        public uint effectid
        {
            get
            {
                return this._effectid;
            }
            set
            {
                this._effectid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "posx", DataFormat = DataFormat.TwosComplement)]
        public uint posx
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

        [ProtoMember(4, IsRequired = false, Name = "posy", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint posy
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

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "r", DataFormat = DataFormat.TwosComplement)]
        public uint r
        {
            get
            {
                return this._r;
            }
            set
            {
                this._r = value;
            }
        }

        private ulong _uid;

        private uint _effectid;

        private uint _posx;

        private uint _posy;

        private uint _r;

        private IExtension extensionObject;
    }
}
