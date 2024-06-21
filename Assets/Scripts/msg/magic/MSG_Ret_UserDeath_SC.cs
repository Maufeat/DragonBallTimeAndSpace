using System;
using System.ComponentModel;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Ret_UserDeath_SC")]
    [Serializable]
    public class MSG_Ret_UserDeath_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "tempid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong tempid
        {
            get
            {
                return this._tempid;
            }
            set
            {
                this._tempid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "attid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong attid
        {
            get
            {
                return this._attid;
            }
            set
            {
                this._attid = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "lasthitskill", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lasthitskill
        {
            get
            {
                return this._lasthitskill;
            }
            set
            {
                this._lasthitskill = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "atttype", DataFormat = DataFormat.TwosComplement)]
        public uint atttype
        {
            get
            {
                return this._atttype;
            }
            set
            {
                this._atttype = value;
            }
        }

        private ulong _tempid;

        private ulong _attid;

        private uint _lasthitskill;

        private uint _atttype;

        private IExtension extensionObject;
    }
}
