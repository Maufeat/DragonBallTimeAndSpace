using System;
using System.Collections.Generic;
using ProtoBuf;

namespace TextModelpackage
{
    [ProtoContract(Name = "TextModelContentListProto")]
    [Serializable]
    public class TextModelContentListProto : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "key", DataFormat = DataFormat.Default)]
        public List<string> key
        {
            get
            {
                return this._key;
            }
        }

        [ProtoMember(2, Name = "modelList", DataFormat = DataFormat.Default)]
        public List<TextModelContentProto> modelList
        {
            get
            {
                return this._modelList;
            }
        }

        private readonly List<string> _key = new List<string>();

        private readonly List<TextModelContentProto> _modelList = new List<TextModelContentProto>();

        private IExtension extensionObject;
    }
}
