using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_RefreshSceneBag_SC")]
    [Serializable]
    public class MSG_RefreshSceneBag_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "tempid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong tempid
        {
            get
            {
                return this._tempid;
            }
            set
            {
                this._tempid = value;
            }
        }

        [ProtoMember(2, Name = "items", DataFormat = DataFormat.Default)]
        public List<ObjItem> items
        {
            get
            {
                return this._items;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "isrefresh", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool isrefresh
        {
            get
            {
                return this._isrefresh;
            }
            set
            {
                this._isrefresh = value;
            }
        }

        private ulong _tempid;

        private readonly List<ObjItem> _items = new List<ObjItem>();

        private bool _isrefresh;

        private IExtension extensionObject;
    }
}
