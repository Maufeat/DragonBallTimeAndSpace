using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_NoticeClientAllLines_SC")]
    [Serializable]
    public class MSG_NoticeClientAllLines_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "lines", DataFormat = DataFormat.Default)]
        public List<LineItem> lines
        {
            get
            {
                return this._lines;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "your_line", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint your_line
        {
            get
            {
                return this._your_line;
            }
            set
            {
                this._your_line = value;
            }
        }

        private readonly List<LineItem> _lines = new List<LineItem>();

        private uint _your_line;

        private IExtension extensionObject;
    }
}
