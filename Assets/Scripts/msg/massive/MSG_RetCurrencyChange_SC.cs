using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_RetCurrencyChange_SC")]
    [Serializable]
    public class MSG_RetCurrencyChange_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "currencyid", DataFormat = DataFormat.TwosComplement)]
        public uint currencyid
        {
            get
            {
                return this._currencyid;
            }
            set
            {
                this._currencyid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "changenum", DataFormat = DataFormat.TwosComplement)]
        public uint changenum
        {
            get
            {
                return this._changenum;
            }
            set
            {
                this._changenum = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "isadd", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool isadd
        {
            get
            {
                return this._isadd;
            }
            set
            {
                this._isadd = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "everydaynum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint everydaynum
        {
            get
            {
                return this._everydaynum;
            }
            set
            {
                this._everydaynum = value;
            }
        }

        private uint _currencyid;

        private uint _changenum;

        private bool _isadd;

        private uint _everydaynum;

        private IExtension extensionObject;
    }
}
