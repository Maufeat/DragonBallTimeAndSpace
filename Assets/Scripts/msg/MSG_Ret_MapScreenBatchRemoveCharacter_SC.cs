using System;
using System.Collections.Generic;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_MapScreenBatchRemoveCharacter_SC")]
    [Serializable]
    public class MSG_Ret_MapScreenBatchRemoveCharacter_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "charids", DataFormat = DataFormat.TwosComplement)]
        public List<ulong> charids
        {
            get
            {
                return this._charids;
            }
        }

        private readonly List<ulong> _charids = new List<ulong>();

        private IExtension extensionObject;
    }
}
