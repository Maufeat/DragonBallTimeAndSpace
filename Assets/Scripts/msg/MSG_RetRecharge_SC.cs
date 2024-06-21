using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_RetRecharge_SC")]
    [Serializable]
    public class MSG_RetRecharge_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        public ERechargeReturnCode retcode
        {
            get
            {
                return this._retcode;
            }
            set
            {
                this._retcode = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "point", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint point
        {
            get
            {
                return this._point;
            }
            set
            {
                this._point = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "balance", DataFormat = DataFormat.TwosComplement)]
        public uint balance
        {
            get
            {
                return this._balance;
            }
            set
            {
                this._balance = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "bonus", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint bonus
        {
            get
            {
                return this._bonus;
            }
            set
            {
                this._bonus = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "hadfilled", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint hadfilled
        {
            get
            {
                return this._hadfilled;
            }
            set
            {
                this._hadfilled = value;
            }
        }

        private ERechargeReturnCode _retcode;

        private uint _point;

        private uint _balance;

        private uint _bonus;

        private uint _hadfilled;

        private IExtension extensionObject;
    }
}
