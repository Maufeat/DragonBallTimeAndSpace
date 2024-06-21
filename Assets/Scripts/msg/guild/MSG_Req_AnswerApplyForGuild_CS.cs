using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_AnswerApplyForGuild_CS")]
    [Serializable]
    public class MSG_Req_AnswerApplyForGuild_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "applyinfo", DataFormat = DataFormat.Default)]
        public List<stApplyForItem> applyinfo
        {
            get
            {
                return this._applyinfo;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "accept", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool accept
        {
            get
            {
                return this._accept;
            }
            set
            {
                this._accept = value;
            }
        }

        private readonly List<stApplyForItem> _applyinfo = new List<stApplyForItem>();

        private bool _accept;

        private IExtension extensionObject;
    }
}
