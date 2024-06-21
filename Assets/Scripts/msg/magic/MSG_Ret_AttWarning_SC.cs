using System;
using System.Collections.Generic;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Ret_AttWarning_SC")]
    [Serializable]
    public class MSG_Ret_AttWarning_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "attacker", DataFormat = DataFormat.Default)]
        public EntryIDType attacker
        {
            get
            {
                return this._attacker;
            }
            set
            {
                this._attacker = value;
            }
        }

        [ProtoMember(2, Name = "warning", DataFormat = DataFormat.Default)]
        public List<AttWarning> warning
        {
            get
            {
                return this._warning;
            }
        }

        private EntryIDType _attacker;

        private readonly List<AttWarning> _warning = new List<AttWarning>();

        private IExtension extensionObject;
    }
}
