using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_ResponseSlotOpened_SC")]
    [Serializable]
    public class MSG_ResponseSlotOpened_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(DNAPage.NONE)]
        [ProtoMember(1, IsRequired = false, Name = "page", DataFormat = DataFormat.TwosComplement)]
        public DNAPage page
        {
            get
            {
                return this._page;
            }
            set
            {
                this._page = value;
            }
        }

        [DefaultValue(DNASlotType.ATT)]
        [ProtoMember(2, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public DNASlotType type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "cur_num", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint cur_num
        {
            get
            {
                return this._cur_num;
            }
            set
            {
                this._cur_num = value;
            }
        }

        private DNAPage _page;

        private DNASlotType _type;

        private uint _cur_num;

        private IExtension extensionObject;
    }
}
