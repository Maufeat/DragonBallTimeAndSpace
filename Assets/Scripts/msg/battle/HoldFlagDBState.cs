using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "HoldFlagDBState")]
    [Serializable]
    public class HoldFlagDBState : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "tempId", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong tempId
        {
            get
            {
                return this._tempId;
            }
            set
            {
                this._tempId = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "campId", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint campId
        {
            get
            {
                return this._campId;
            }
            set
            {
                this._campId = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "DBState", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool DBState
        {
            get
            {
                return this._DBState;
            }
            set
            {
                this._DBState = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "capUserId", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong capUserId
        {
            get
            {
                return this._capUserId;
            }
            set
            {
                this._capUserId = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(5, IsRequired = false, Name = "isInBase", DataFormat = DataFormat.Default)]
        public bool isInBase
        {
            get
            {
                return this._isInBase;
            }
            set
            {
                this._isInBase = value;
            }
        }

        private ulong _tempId;

        private uint _campId;

        private bool _DBState;

        private ulong _capUserId;

        private bool _isInBase;

        private IExtension extensionObject;
    }
}
