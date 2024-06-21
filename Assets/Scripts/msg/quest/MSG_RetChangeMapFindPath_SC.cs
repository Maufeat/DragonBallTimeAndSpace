using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_RetChangeMapFindPath_SC")]
    [Serializable]
    public class MSG_RetChangeMapFindPath_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "info", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public ChangeMapFindWayInfo info
        {
            get
            {
                return this._info;
            }
            set
            {
                this._info = value;
            }
        }

        private ChangeMapFindWayInfo _info;

        private IExtension extensionObject;
    }
}
