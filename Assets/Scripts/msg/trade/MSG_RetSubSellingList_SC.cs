using System;
using System.ComponentModel;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_RetSubSellingList_SC")]
    [Serializable]
    public class MSG_RetSubSellingList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "totalpage", DataFormat = DataFormat.TwosComplement)]
        public uint totalpage
        {
            get
            {
                return this._totalpage;
            }
            set
            {
                this._totalpage = value;
            }
        }

        private uint _totalpage;

        private IExtension extensionObject;
    }
}
