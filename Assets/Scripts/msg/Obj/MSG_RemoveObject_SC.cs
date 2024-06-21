using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_RemoveObject_SC")]
    [Serializable]
    public class MSG_RemoveObject_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "ids", DataFormat = DataFormat.Default)]
        public List<string> ids
        {
            get
            {
                return this._ids;
            }
        }

        private readonly List<string> _ids = new List<string>();

        private IExtension extensionObject;
    }
}
