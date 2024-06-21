using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_NotifyClientOptional_SC")]
    [Serializable]
    public class MSG_NotifyClientOptional_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "setting", DataFormat = DataFormat.Default)]
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

        private uint _type;

        private string _setting = string.Empty;

        private IExtension extensionObject;
    }
}
