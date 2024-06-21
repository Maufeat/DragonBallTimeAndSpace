using System;
using System.ComponentModel;
using ProtoBuf;

namespace country
{
    [ProtoContract(Name = "MSG_Req_JoinCountry_CS")]
    [Serializable]
    public class MSG_Req_JoinCountry_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "countryid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint countryid
        {
            get
            {
                return this._countryid;
            }
            set
            {
                this._countryid = value;
            }
        }

        private uint _countryid;

        private IExtension extensionObject;
    }
}
