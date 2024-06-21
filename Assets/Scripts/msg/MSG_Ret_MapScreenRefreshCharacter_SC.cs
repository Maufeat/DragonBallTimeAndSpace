using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_MapScreenRefreshCharacter_SC")]
    [Serializable]
    public class MSG_Ret_MapScreenRefreshCharacter_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "data", DataFormat = DataFormat.Default)]
        public MapUserData data
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

        private MapUserData _data;

        private IExtension extensionObject;
    }
}
