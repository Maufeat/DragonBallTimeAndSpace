using System;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_Ret_AddHoldNpcData_SC")]
    [Serializable]
    public class MSG_Ret_AddHoldNpcData_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "data", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public HoldNpcData data
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

        private HoldNpcData _data;

        private IExtension extensionObject;
    }
}
