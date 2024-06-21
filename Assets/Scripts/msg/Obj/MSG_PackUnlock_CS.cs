using System;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_PackUnlock_CS")]
    [Serializable]
    public class MSG_PackUnlock_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
        public PackType packtype
        {
            get
            {
                return this._packtype;
            }
            set
            {
                this._packtype = value;
            }
        }

        private PackType _packtype;

        private IExtension extensionObject;
    }
}
