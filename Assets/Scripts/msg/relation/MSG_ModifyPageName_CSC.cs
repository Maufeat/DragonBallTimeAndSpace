using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_ModifyPageName_CSC")]
    [Serializable]
    public class MSG_ModifyPageName_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "page", DataFormat = DataFormat.Default)]
        public string page
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

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "new_page", DataFormat = DataFormat.Default)]
        public string new_page
        {
            get
            {
                return this._new_page;
            }
            set
            {
                this._new_page = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "opcode", DataFormat = DataFormat.TwosComplement)]
        public uint opcode
        {
            get
            {
                return this._opcode;
            }
            set
            {
                this._opcode = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(4, IsRequired = false, Name = "success", DataFormat = DataFormat.Default)]
        public bool success
        {
            get
            {
                return this._success;
            }
            set
            {
                this._success = value;
            }
        }

        private string _page = string.Empty;

        private string _new_page = string.Empty;

        private uint _opcode;

        private bool _success;

        private IExtension extensionObject;
    }
}
