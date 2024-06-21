using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Chat
{
    [ProtoContract(Name = "MSG_Ret_OfflineChat_SC")]
    [Serializable]
    public class MSG_Ret_OfflineChat_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "datas", DataFormat = DataFormat.Default)]
        public List<ChatData> datas
        {
            get
            {
                return this._datas;
            }
        }

        private readonly List<ChatData> _datas = new List<ChatData>();

        private IExtension extensionObject;
    }
}
