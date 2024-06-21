using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Chat
{
    [ProtoContract(Name = "ChatLink")]
    [Serializable]
    public class ChatLink : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "linktype", DataFormat = DataFormat.TwosComplement)]
        public uint linktype
        {
            get
            {
                return this._linktype;
            }
            set
            {
                this._linktype = value;
            }
        }

        [ProtoMember(2, Name = "data_args", DataFormat = DataFormat.Default)]
        public List<string> data_args
        {
            get
            {
                return this._data_args;
            }
        }

        private uint _linktype;

        private readonly List<string> _data_args = new List<string>();

        private IExtension extensionObject;
    }
}
