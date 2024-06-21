using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_MyGuildApply_Result_SC")]
    [Serializable]
    public class MSG_Ret_MyGuildApply_Result_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "result", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public stApplyForItem result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }

        private stApplyForItem _result;

        private IExtension extensionObject;
    }
}
