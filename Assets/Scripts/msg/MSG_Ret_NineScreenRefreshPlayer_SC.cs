using System;
using System.Collections.Generic;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_NineScreenRefreshPlayer_SC")]
    [Serializable]
    public class MSG_Ret_NineScreenRefreshPlayer_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "data", DataFormat = DataFormat.Default)]
        public List<MapUserData> data
        {
            get
            {
                return this._data;
            }
        }

        private readonly List<MapUserData> _data = new List<MapUserData>();

        private IExtension extensionObject;
    }
}
