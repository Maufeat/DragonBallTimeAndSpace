using System;
using System.ComponentModel;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Ret_MainUserDeath_SC")]
    [Serializable]
    public class MSG_Ret_MainUserDeath_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.Default)]
        public string charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "relivecostid", DataFormat = DataFormat.TwosComplement)]
        public uint relivecostid
        {
            get
            {
                return this._relivecostid;
            }
            set
            {
                this._relivecostid = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "relivecostnum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint relivecostnum
        {
            get
            {
                return this._relivecostnum;
            }
            set
            {
                this._relivecostnum = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "canreliveorigin", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint canreliveorigin
        {
            get
            {
                return this._canreliveorigin;
            }
            set
            {
                this._canreliveorigin = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "canrelive", DataFormat = DataFormat.Default)]
        [DefaultValue(true)]
        public bool canrelive
        {
            get
            {
                return this._canrelive;
            }
            set
            {
                this._canrelive = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "relivetime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint relivetime
        {
            get
            {
                return this._relivetime;
            }
            set
            {
                this._relivetime = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "autorelive", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool autorelive
        {
            get
            {
                return this._autorelive;
            }
            set
            {
                this._autorelive = value;
            }
        }

        [ProtoMember(8, IsRequired = false, Name = "relive_type", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint relive_type
        {
            get
            {
                return this._relive_type;
            }
            set
            {
                this._relive_type = value;
            }
        }

        private string _charid = string.Empty;

        private uint _relivecostid;

        private uint _relivecostnum;

        private uint _canreliveorigin;

        private bool _canrelive = true;

        private uint _relivetime;

        private bool _autorelive;

        private uint _relive_type;

        private IExtension extensionObject;
    }
}
