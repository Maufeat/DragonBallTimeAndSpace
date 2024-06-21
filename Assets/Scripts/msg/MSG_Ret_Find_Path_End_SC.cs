using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_Find_Path_End_SC")]
    [Serializable]
    public class MSG_Ret_Find_Path_End_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "endcode", DataFormat = DataFormat.TwosComplement)]
        public uint endcode
        {
            get
            {
                return this._endcode;
            }
            set
            {
                this._endcode = value;
            }
        }

        private uint _endcode;

        private IExtension extensionObject;
    }
}
