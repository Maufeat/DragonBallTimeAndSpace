using System;
using System.Collections.Generic;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_AnswerApplyForGuild_SC")]
    [Serializable]
    public class MSG_Ret_AnswerApplyForGuild_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "retlist", DataFormat = DataFormat.Default)]
        public List<stApplyForItem> retlist
        {
            get
            {
                return this._retlist;
            }
        }

        private readonly List<stApplyForItem> _retlist = new List<stApplyForItem>();

        private IExtension extensionObject;
    }
}
