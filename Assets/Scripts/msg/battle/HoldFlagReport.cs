using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "HoldFlagReport")]
    [Serializable]
    public class HoldFlagReport : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "campId", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(3, IsRequired = false, Name = "teamId", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint teamId
        {
            get
            {
                return this._teamId;
            }
            set
            {
                this._teamId = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(4, IsRequired = false, Name = "userid", DataFormat = DataFormat.TwosComplement)]
        public ulong userid
        {
            get
            {
                return this._userid;
            }
            set
            {
                this._userid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "hurtNum", DataFormat = DataFormat.TwosComplement)]
        public uint hurtNum
        {
            get
            {
                return this._hurtNum;
            }
            set
            {
                this._hurtNum = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "cureNum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint cureNum
        {
            get
            {
                return this._cureNum;
            }
            set
            {
                this._cureNum = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "killNum", DataFormat = DataFormat.TwosComplement)]
        public uint killNum
        {
            get
            {
                return this._killNum;
            }
            set
            {
                this._killNum = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "deadNum", DataFormat = DataFormat.TwosComplement)]
        public uint deadNum
        {
            get
            {
                return this._deadNum;
            }
            set
            {
                this._deadNum = value;
            }
        }

        [ProtoMember(9, IsRequired = false, Name = "backDBNum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint backDBNum
        {
            get
            {
                return this._backDBNum;
            }
            set
            {
                this._backDBNum = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(10, IsRequired = false, Name = "captureDBNum", DataFormat = DataFormat.TwosComplement)]
        public uint captureDBNum
        {
            get
            {
                return this._captureDBNum;
            }
            set
            {
                this._captureDBNum = value;
            }
        }

        private string _name = string.Empty;

        private uint _campId;

        private uint _teamId;

        private ulong _userid;

        private uint _hurtNum;

        private uint _cureNum;

        private uint _killNum;

        private uint _deadNum;

        private uint _backDBNum;

        private uint _captureDBNum;

        private IExtension extensionObject;
    }
}
