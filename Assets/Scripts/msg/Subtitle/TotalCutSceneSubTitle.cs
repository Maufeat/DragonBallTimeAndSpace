using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Subtitle
{
    [ProtoContract(Name = "TotalCutSceneSubTitle")]
    [Serializable]
    public class TotalCutSceneSubTitle : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "subtitlelist", DataFormat = DataFormat.Default)]
        public List<CutSceneSubTitle> subtitlelist
        {
            get
            {
                return this._subtitlelist;
            }
        }

        private readonly List<CutSceneSubTitle> _subtitlelist = new List<CutSceneSubTitle>();

        private IExtension extensionObject;
    }
}
