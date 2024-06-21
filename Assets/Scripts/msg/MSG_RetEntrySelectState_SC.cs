using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_RetEntrySelectState_SC")]
    [Serializable]
    public class MSG_RetEntrySelectState_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "choosen", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public EntryIDType choosen
        {
            get
            {
                return this._choosen;
            }
            set
            {
                this._choosen = value;
            }
        }

        [ProtoMember(2, Name = "states", DataFormat = DataFormat.Default)]
        public List<StateItem> states
        {
            get
            {
                return this._states;
            }
        }

        private EntryIDType _choosen;

        private readonly List<StateItem> _states = new List<StateItem>();

        private IExtension extensionObject;
    }
}
