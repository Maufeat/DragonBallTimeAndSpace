using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_RetMemberBackTeam_SC")]
    [Serializable]
    public class MSG_RetMemberBackTeam_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "rettype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint rettype
        {
            get
            {
                return this._rettype;
            }
            set
            {
                this._rettype = value;
            }
        }

        private uint _rettype;

        private IExtension extensionObject;
    }
}
