using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_TidyPack_SC")]
    [Serializable]
    public class MSG_TidyPack_SC : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "result", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }

        private PackType _packtype;

        private bool _result;

        private IExtension extensionObject;
    }
}
