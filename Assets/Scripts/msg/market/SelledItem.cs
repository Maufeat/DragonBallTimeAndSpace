using System;
using System.ComponentModel;
using Obj;
using ProtoBuf;

namespace market
{
    [ProtoContract(Name = "SelledItem")]
    [Serializable]
    public class SelledItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "item", DataFormat = DataFormat.Default)]
        public t_Object item
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "selltime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint selltime
        {
            get
            {
                return this._selltime;
            }
            set
            {
                this._selltime = value;
            }
        }

        private t_Object _item;

        private uint _selltime;

        private IExtension extensionObject;
    }
}
