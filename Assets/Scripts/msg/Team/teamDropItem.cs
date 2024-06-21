using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "teamDropItem")]
    [Serializable]
    public class teamDropItem : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "objid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint objid
        {
            get
            {
                return this._objid;
            }
            set
            {
                this._objid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "bind", DataFormat = DataFormat.TwosComplement)]
        public uint bind
        {
            get
            {
                return this._bind;
            }
            set
            {
                this._bind = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "duetime", DataFormat = DataFormat.TwosComplement)]
        public uint duetime
        {
            get
            {
                return this._duetime;
            }
            set
            {
                this._duetime = value;
            }
        }

        private string _thisid = string.Empty;

        private uint _objid;

        private uint _num;

        private uint _bind;

        private uint _duetime;

        private IExtension extensionObject;
    }
}
