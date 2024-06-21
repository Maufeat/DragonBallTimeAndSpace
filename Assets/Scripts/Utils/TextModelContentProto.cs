using System;
using System.ComponentModel;
using ProtoBuf;

namespace TextModelpackage
{
    [ProtoContract(Name = "TextModelContentProto")]
    [Serializable]
    public class TextModelContentProto : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "modelBegin", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string modelBegin
        {
            get
            {
                return this._modelBegin;
            }
            set
            {
                this._modelBegin = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "modelEnd", DataFormat = DataFormat.Default)]
        public string modelEnd
        {
            get
            {
                return this._modelEnd;
            }
            set
            {
                this._modelEnd = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "fontSize", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint fontSize
        {
            get
            {
                return this._fontSize;
            }
            set
            {
                this._fontSize = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "fontName", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string fontName
        {
            get
            {
                return this._fontName;
            }
            set
            {
                this._fontName = value;
            }
        }

        private string _modelBegin = string.Empty;

        private string _modelEnd = string.Empty;

        private uint _fontSize;

        private string _fontName = string.Empty;

        private IExtension extensionObject;
    }
}
