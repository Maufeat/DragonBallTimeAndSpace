using System;
using System.Collections.Generic;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_StartPray_SC")]
    [Serializable]
    public class MSG_StartPray_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "hopes", DataFormat = DataFormat.Default)]
        public List<string> hopes
        {
            get
            {
                return this._hopes;
            }
        }

        private readonly List<string> _hopes = new List<string>();

        private IExtension extensionObject;
    }
}
