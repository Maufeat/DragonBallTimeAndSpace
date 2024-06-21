using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_PackUnlock_SC")]
    [Serializable]
    public class MSG_PackUnlock_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
        public PackType packtype
        {
            get
            {
                return this._packtype;
            }
            set
            {
                this._packtype = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "unlockcount", DataFormat = DataFormat.TwosComplement)]
        public uint unlockcount
        {
            get
            {
                return this._unlockcount;
            }
            set
            {
                this._unlockcount = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "maxsize", DataFormat = DataFormat.TwosComplement)]
        public uint maxsize
        {
            get
            {
                return this._maxsize;
            }
            set
            {
                this._maxsize = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "result", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }

        private PackType _packtype;

        private uint _unlockcount;

        private uint _maxsize;

        private bool _result;

        private IExtension extensionObject;
    }
}
