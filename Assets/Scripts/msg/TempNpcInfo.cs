using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "TempNpcInfo")]
    [Serializable]
    public class TempNpcInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "npcid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint npcid
        {
            get
            {
                return this._npcid;
            }
            set
            {
                this._npcid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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
        [ProtoMember(3, IsRequired = false, Name = "script", DataFormat = DataFormat.TwosComplement)]
        public uint script
        {
            get
            {
                return this._script;
            }
            set
            {
                this._script = value;
            }
        }

        private uint _npcid;

        private uint _num;

        private uint _script;

        private IExtension extensionObject;
    }
}
