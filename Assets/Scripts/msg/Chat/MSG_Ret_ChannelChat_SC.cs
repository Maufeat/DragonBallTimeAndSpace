using System;
using System.ComponentModel;
using ProtoBuf;

namespace Chat
{
    [ProtoContract(Name = "MSG_Ret_ChannelChat_SC")]
    [Serializable]
    public class MSG_Ret_ChannelChat_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "str_chat", DataFormat = DataFormat.Default)]
        public string str_chat
        {
            get
            {
                return this._str_chat;
            }
            set
            {
                this._str_chat = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "src_name", DataFormat = DataFormat.Default)]
        public string src_name
        {
            get
            {
                return this._src_name;
            }
            set
            {
                this._src_name = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "textid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint textid
        {
            get
            {
                return this._textid;
            }
            set
            {
                this._textid = value;
            }
        }

        [DefaultValue(ChannelType.ChannelType_None)]
        [ProtoMember(4, IsRequired = false, Name = "channel_type", DataFormat = DataFormat.TwosComplement)]
        public ChannelType channel_type
        {
            get
            {
                return this._channel_type;
            }
            set
            {
                this._channel_type = value;
            }
        }

        private string _str_chat = string.Empty;

        private string _src_name = string.Empty;

        private uint _textid;

        private ChannelType _channel_type;

        private IExtension extensionObject;
    }
}
