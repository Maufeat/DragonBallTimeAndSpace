using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_NPCHatredList_SC")]
    [Serializable]
    public class MSG_NPCHatredList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "npctempid", DataFormat = DataFormat.TwosComplement)]
        public ulong npctempid
        {
            get
            {
                return this._npctempid;
            }
            set
            {
                this._npctempid = value;
            }
        }

        [ProtoMember(2, Name = "enemytempid", DataFormat = DataFormat.TwosComplement)]
        public List<ulong> enemytempid
        {
            get
            {
                return this._enemytempid;
            }
        }

        private ulong _npctempid;

        private readonly List<ulong> _enemytempid = new List<ulong>();

        private IExtension extensionObject;
    }
}
