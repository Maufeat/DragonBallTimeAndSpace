using System;
using System.Collections.Generic;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_RetHoldFlagReport_SC")]
    [Serializable]
    public class MSG_RetHoldFlagReport_SC : IExtensible
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

        private readonly List<HoldFlagReport> _reports = new List<HoldFlagReport>();

        private IExtension extensionObject;
    }
}
