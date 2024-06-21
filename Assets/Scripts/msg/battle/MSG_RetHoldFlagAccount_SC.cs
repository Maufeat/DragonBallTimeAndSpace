using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_RetHoldFlagAccount_SC")]
    [Serializable]
    public class MSG_RetHoldFlagAccount_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "reports", DataFormat = DataFormat.Default)]
        public List<HoldFlagReport> reports
        {
            get
            {
                return this._reports;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "winCampId", DataFormat = DataFormat.TwosComplement)]
        public uint winCampId
        {
            get
            {
                return this._winCampId;
            }
            set
            {
                this._winCampId = value;
            }
        }

        private readonly List<HoldFlagReport> _reports = new List<HoldFlagReport>();

        private uint _winCampId;

        private IExtension extensionObject;
    }
}
