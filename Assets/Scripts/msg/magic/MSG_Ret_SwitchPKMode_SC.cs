using System;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Ret_SwitchPKMode_SC")]
    [Serializable]
    public class MSG_Ret_SwitchPKMode_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "newmode", DataFormat = DataFormat.TwosComplement)]
        public PKMode newmode
        {
            get
            {
                return this._newmode;
            }
            set
            {
                this._newmode = value;
            }
        }

        private PKMode _newmode;

        private IExtension extensionObject;
    }
}
