using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_MapScreenRefreshNpc_SC")]
    [Serializable]
    public class MSG_Ret_MapScreenRefreshNpc_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "data", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public MapNpcData data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        private MapNpcData _data;

        private IExtension extensionObject;
    }
}
