using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_Ret_GuideStart_SC")]
    [Serializable]
    public class MSG_Ret_GuideStart_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "guideid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint guideid
        {
            get
            {
                return this._guideid;
            }
            set
            {
                this._guideid = value;
            }
        }

        private uint _guideid;

        private IExtension extensionObject;
    }
}
