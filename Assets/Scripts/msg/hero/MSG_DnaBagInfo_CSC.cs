using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_DnaBagInfo_CSC")]
    [Serializable]
    public class MSG_DnaBagInfo_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "datas", DataFormat = DataFormat.Default)]
        public List<DnaItem> datas
        {
            get
            {
                return this._datas;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "cur_page", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(DNAPage.NONE)]
        public DNAPage cur_page
        {
            get
            {
                return this._cur_page;
            }
            set
            {
                this._cur_page = value;
            }
        }

        private readonly List<DnaItem> _datas = new List<DnaItem>();

        private DNAPage _cur_page;

        private IExtension extensionObject;
    }
}
