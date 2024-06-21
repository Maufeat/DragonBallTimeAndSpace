using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Subtitle
{
    [ProtoContract(Name = "CutSceneSubTitle")]
    [Serializable]
    public class CutSceneSubTitle : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "key", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        [ProtoMember(2, Name = "subtitlelist", DataFormat = DataFormat.Default)]
        public List<SubtitleContent> subtitlelist
        {
            get
            {
                return this._subtitlelist;
            }
        }

        private string _key = string.Empty;

        private readonly List<SubtitleContent> _subtitlelist = new List<SubtitleContent>();

        private IExtension extensionObject;
    }
}
