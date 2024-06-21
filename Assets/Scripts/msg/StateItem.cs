using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "StateItem")]
    [Serializable]
    public class StateItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "uniqid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong uniqid
        {
            get
            {
                return this._uniqid;
            }
            set
            {
                this._uniqid = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "lasttime", DataFormat = DataFormat.TwosComplement)]
        public ulong lasttime
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

        [ProtoMember(3, IsRequired = false, Name = "overtime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong overtime
        {
            get
            {
                return this._overtime;
            }
            set
            {
                this._overtime = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "settime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong settime
        {
            get
            {
                return this._settime;
            }
            set
            {
                this._settime = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(5, IsRequired = false, Name = "configtime", DataFormat = DataFormat.TwosComplement)]
        public ulong configtime
        {
            get
            {
                return this._configtime;
            }
            set
            {
                this._configtime = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "skilluuid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong skilluuid
        {
            get
            {
                return this._skilluuid;
            }
            set
            {
                this._skilluuid = value;
            }
        }

        [ProtoMember(7, Name = "effects", DataFormat = DataFormat.TwosComplement)]
        public List<ulong> effects
        {
            get
            {
                return this._effects;
            }
        }

        private ulong _uniqid;

        private ulong _lasttime;

        private ulong _overtime;

        private ulong _settime;

        private ulong _configtime;

        private ulong _skilluuid;

        private readonly List<ulong> _effects = new List<ulong>();

        private IExtension extensionObject;
    }
}
