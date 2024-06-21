using System;
using System.Collections.Generic;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_ReqSubSellingList_CS")]
    [Serializable]
    public class MSG_ReqSubSellingList_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "itemtype", DataFormat = DataFormat.TwosComplement)]
        public SELLTYPE itemtype
        {
            get
            {
                return this._itemtype;
            }
            set
            {
                this._itemtype = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "levelstar", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint levelstar
        {
            get
            {
                return this._levelstar;
            }
            set
            {
                this._levelstar = value;
            }
        }

        [ProtoMember(3, Name = "idlist", DataFormat = DataFormat.TwosComplement)]
        public List<uint> idlist
        {
            get
            {
                return this._idlist;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(4, IsRequired = false, Name = "checkshow", DataFormat = DataFormat.Default)]
        public bool checkshow
        {
            get
            {
                return this._checkshow;
            }
            set
            {
                this._checkshow = value;
            }
        }

        private SELLTYPE _itemtype;

        private uint _levelstar;

        private readonly List<uint> _idlist = new List<uint>();

        private bool _checkshow;

        private IExtension extensionObject;
    }
}
