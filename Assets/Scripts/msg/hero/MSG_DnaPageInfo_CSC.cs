using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_DnaPageInfo_CSC")]
    [Serializable]
    public class MSG_DnaPageInfo_CSC : IExtensible
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

        [ProtoMember(2, Name = "att_holes", DataFormat = DataFormat.Default)]
        public List<Hole> att_holes
        {
            get
            {
                return this._att_holes;
            }
        }

        [ProtoMember(3, Name = "def_holes", DataFormat = DataFormat.Default)]
        public List<Hole> def_holes
        {
            get
            {
                return this._def_holes;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "att_opened_num", DataFormat = DataFormat.TwosComplement)]
        public uint att_opened_num
        {
            get
            {
                return this._att_opened_num;
            }
            set
            {
                this._att_opened_num = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "def_opened_num", DataFormat = DataFormat.TwosComplement)]
        public uint def_opened_num
        {
            get
            {
                return this._def_opened_num;
            }
            set
            {
                this._def_opened_num = value;
            }
        }

        private DNAPage _page;

        private readonly List<Hole> _att_holes = new List<Hole>();

        private readonly List<Hole> _def_holes = new List<Hole>();

        private uint _att_opened_num;

        private uint _def_opened_num;

        private IExtension extensionObject;
    }
}
