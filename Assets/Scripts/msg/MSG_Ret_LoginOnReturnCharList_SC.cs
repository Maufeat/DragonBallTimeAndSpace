using System;
using System.Collections.Generic;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_LoginOnReturnCharList_SC")]
    [Serializable]
    public class MSG_Ret_LoginOnReturnCharList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "charList", DataFormat = DataFormat.Default)]
        public List<SelectUserInfo> charList
        {
            get
            {
                return this._charList;
            }
        }

        private readonly List<SelectUserInfo> _charList = new List<SelectUserInfo>();

        private IExtension extensionObject;
    }
}
