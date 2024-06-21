using System;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_ReqWatchSceneBag_CS")]
    [Serializable]
    public class MSG_ReqWatchSceneBag_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "tempid", DataFormat = DataFormat.TwosComplement)]
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

        private ulong _tempid;

        private IExtension extensionObject;
    }
}
