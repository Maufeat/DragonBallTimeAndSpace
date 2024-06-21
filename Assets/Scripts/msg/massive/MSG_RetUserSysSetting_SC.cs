using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_RetUserSysSetting_SC")]
    [Serializable]
    public class MSG_RetUserSysSetting_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "setting", DataFormat = DataFormat.Default)]
        public string setting
        {
            get
            {
                return this._setting;
            }
            set
            {
                this._setting = value;
            }
        }

        private string _setting = string.Empty;

        private IExtension extensionObject;
    }
}
