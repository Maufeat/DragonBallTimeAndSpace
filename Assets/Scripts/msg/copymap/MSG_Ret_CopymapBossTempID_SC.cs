using System;
using System.Collections.Generic;
using ProtoBuf;

namespace copymap
{
    [ProtoContract(Name = "MSG_Ret_CopymapBossTempID_SC")]
    [Serializable]
    public class MSG_Ret_CopymapBossTempID_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "tempid", DataFormat = DataFormat.Default)]
        public List<string> tempid
        {
            get
            {
                return this._tempid;
            }
        }

        private readonly List<string> _tempid = new List<string>();

        private IExtension extensionObject;
    }
}
