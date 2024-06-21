using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_UpdateExpLevel_SC")]
    [Serializable]
    public class MSG_UpdateExpLevel_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "curexp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint curexp
        {
            get
            {
                return this._curexp;
            }
            set
            {
                this._curexp = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "curlevel", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint curlevel
        {
            get
            {
                return this._curlevel;
            }
            set
            {
                this._curlevel = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(3, IsRequired = false, Name = "mainhero_thisid", DataFormat = DataFormat.TwosComplement)]
        public ulong mainhero_thisid
        {
            get
            {
                return this._mainhero_thisid;
            }
            set
            {
                this._mainhero_thisid = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(4, IsRequired = false, Name = "mainhero_exp", DataFormat = DataFormat.TwosComplement)]
        public ulong mainhero_exp
        {
            get
            {
                return this._mainhero_exp;
            }
            set
            {
                this._mainhero_exp = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "mainhero_lv", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint mainhero_lv
        {
            get
            {
                return this._mainhero_lv;
            }
            set
            {
                this._mainhero_lv = value;
            }
        }

        private uint _curexp;

        private uint _curlevel;

        private ulong _mainhero_thisid;

        private ulong _mainhero_exp;

        private uint _mainhero_lv;

        private IExtension extensionObject;
    }
}
