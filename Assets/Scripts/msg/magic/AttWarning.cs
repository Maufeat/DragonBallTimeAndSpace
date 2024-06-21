using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "AttWarning")]
    [Serializable]
    public class AttWarning : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "lasttime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lasttime
        {
            get
            {
                return this._lasttime;
            }
            set
            {
                this._lasttime = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "pos", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public Position pos
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
        [ProtoMember(3, IsRequired = false, Name = "dir", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(4, IsRequired = false, Name = "rangetype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint rangetype
        {
            get
            {
                return this._rangetype;
            }
            set
            {
                this._rangetype = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "rangep1", DataFormat = DataFormat.TwosComplement)]
        public uint rangep1
        {
            get
            {
                return this._rangep1;
            }
            set
            {
                this._rangep1 = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "rangep2", DataFormat = DataFormat.TwosComplement)]
        public uint rangep2
        {
            get
            {
                return this._rangep2;
            }
            set
            {
                this._rangep2 = value;
            }
        }

        private uint _lasttime;

        private Position _pos;

        private uint _dir;

        private uint _rangetype;

        private uint _rangep1;

        private uint _rangep2;

        private IExtension extensionObject;
    }
}
