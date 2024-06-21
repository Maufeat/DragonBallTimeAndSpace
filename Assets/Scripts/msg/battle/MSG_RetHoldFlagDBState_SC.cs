using System;
using System.Collections.Generic;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_RetHoldFlagDBState_SC")]
    [Serializable]
    public class MSG_RetHoldFlagDBState_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "DBStates", DataFormat = DataFormat.Default)]
        public List<HoldFlagDBState> DBStates
        {
            get
            {
                return this._DBStates;
            }
        }

        private readonly List<HoldFlagDBState> _DBStates = new List<HoldFlagDBState>();

        private IExtension extensionObject;
    }
}
