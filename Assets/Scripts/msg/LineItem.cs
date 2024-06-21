using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "LineItem")]
    [Serializable]
    public class LineItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "index", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint index
        {
            get
            {
                return this._index;
            }
            set
            {
                this._index = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "user_num", DataFormat = DataFormat.TwosComplement)]
        public uint user_num
        {
            get
            {
                return this._user_num;
            }
            set
            {
                this._user_num = value;
            }
        }

        private uint _index;

        private uint _user_num;

        private IExtension extensionObject;
    }
}
