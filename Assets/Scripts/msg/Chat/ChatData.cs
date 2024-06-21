using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Chat
{
    [ProtoContract(Name = "ChatData")]
    [Serializable]
    public class ChatData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "channel", DataFormat = DataFormat.TwosComplement)]
        public uint channel
        {
            get
            {
                return this._channel;
            }
            set
            {
                this._channel = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "charname", DataFormat = DataFormat.Default)]
        public string charname
        {
            get
            {
                return this._charname;
            }
            set
            {
                this._charname = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "charcountry", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint charcountry
        {
            get
            {
                return this._charcountry;
            }
            set
            {
                this._charcountry = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "chattime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint chattime
        {
            get
            {
                return this._chattime;
            }
            set
            {
                this._chattime = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(6, IsRequired = false, Name = "content", DataFormat = DataFormat.Default)]
        public string content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
            }
        }

        [ProtoMember(7, Name = "link", DataFormat = DataFormat.Default)]
        public List<ChatLink> link
        {
            get
            {
                return this._link;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "show_type", DataFormat = DataFormat.TwosComplement)]
        public uint show_type
        {
            get
            {
                return this._show_type;
            }
            set
            {
                this._show_type = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(9, IsRequired = false, Name = "tocharid", DataFormat = DataFormat.TwosComplement)]
        public ulong tocharid
        {
            get
            {
                return this._tocharid;
            }
            set
            {
                this._tocharid = value;
            }
        }

        [ProtoMember(10, IsRequired = false, Name = "toname", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string toname
        {
            get
            {
                return this._toname;
            }
            set
            {
                this._toname = value;
            }
        }

        private uint _channel;

        private ulong _charid;

        private string _charname = string.Empty;

        private uint _charcountry;

        private uint _chattime;

        private string _content = string.Empty;

        private readonly List<ChatLink> _link = new List<ChatLink>();

        private uint _show_type;

        private ulong _tocharid;

        private string _toname = string.Empty;

        private IExtension extensionObject;
    }
}
