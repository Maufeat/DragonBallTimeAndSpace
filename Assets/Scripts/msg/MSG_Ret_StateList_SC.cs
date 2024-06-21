using System;
using System.Collections.Generic;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_StateList_SC")]
    [Serializable]
    public class MSG_Ret_StateList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "states", DataFormat = DataFormat.Default)]
        public List<StateItem> states
        {
            get
            {
                return this._states;
            }
        }

        private readonly List<StateItem> _states = new List<StateItem>();

        private IExtension extensionObject;
    }
}
