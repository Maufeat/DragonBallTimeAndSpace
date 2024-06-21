using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Ret_InterruptSkill_SC")]
    [Serializable]
    public class MSG_Ret_InterruptSkill_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "att", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public EntryIDType att
        {
            get
            {
                return this._att;
            }
            set
            {
                this._att = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "skillstage", DataFormat = DataFormat.TwosComplement)]
        public ulong skillstage
        {
            get
            {
                return this._skillstage;
            }
            set
            {
                this._skillstage = value;
            }
        }

        private EntryIDType _att;

        private ulong _skillstage;

        private IExtension extensionObject;
    }
}
