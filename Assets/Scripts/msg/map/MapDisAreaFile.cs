using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace map
{
    [ProtoContract(Name = "MapDisAreaFile")]
    [Serializable]
    public class MapDisAreaFile : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "fileHeader", DataFormat = DataFormat.Default)]
        public MapDisAreaFileHeader fileHeader
        {
            get
            {
                return this._fileHeader;
            }
            set
            {
                this._fileHeader = value;
            }
        }

        [ProtoMember(2, Name = "areainfos", DataFormat = DataFormat.Default)]
        public List<AreaInfo> areainfos
        {
            get
            {
                return this._areainfos;
            }
        }

        private MapDisAreaFileHeader _fileHeader;

        private readonly List<AreaInfo> _areainfos = new List<AreaInfo>();

        private IExtension extensionObject;
    }
}
