using System;
using System.ComponentModel;
using ProtoBuf;

namespace Chat
{
    [ProtoContract(Name = "MSG_Ret_Chat_SC")]
    [Serializable]
    public class MSG_Ret_Chat_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "data", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public ChatData data
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

        [DefaultValue(false)]
        [ProtoMember(2, IsRequired = false, Name = "shake", DataFormat = DataFormat.Default)]
        public bool shake
        {
            get
            {
                return this._shake;
            }
            set
            {
                this._shake = value;
            }
        }

        private ChatData _data;

        private bool _shake;

        private IExtension extensionObject;
    }
}
