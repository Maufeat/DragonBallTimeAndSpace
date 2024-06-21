using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_ReqPickObjFromSceneBag_CS")]
    [Serializable]
    public class MSG_ReqPickObjFromSceneBag_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "thisids", DataFormat = DataFormat.TwosComplement)]
        public List<uint> thisids
        {
            get
            {
                return this._thisids;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "tempid", DataFormat = DataFormat.TwosComplement)]
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

        private readonly List<uint> _thisids = new List<uint>();

        private ulong _tempid;

        private IExtension extensionObject;
    }
}
