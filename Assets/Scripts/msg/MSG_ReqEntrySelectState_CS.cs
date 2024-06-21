using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_ReqEntrySelectState_CS")]
    [Serializable]
    public class MSG_ReqEntrySelectState_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "oldone", DataFormat = DataFormat.Default)]
        public EntryIDType oldone
        {
            get
            {
                return this._oldone;
            }
            set
            {
                this._oldone = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "newone", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public EntryIDType newone
        {
            get
            {
                return this._newone;
            }
            set
            {
                this._newone = value;
            }
        }

        private EntryIDType _oldone;

        private EntryIDType _newone;

        private IExtension extensionObject;
    }
}
