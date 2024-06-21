using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqApplyList_SC")]
    [Serializable]
    public class MSG_ReqApplyList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "applyer", DataFormat = DataFormat.Default)]
        public List<Memember> applyer
        {
            get
            {
                return this._applyer;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "leaderid", DataFormat = DataFormat.Default)]
        public string leaderid
        {
            get
            {
                return this._leaderid;
            }
            set
            {
                this._leaderid = value;
            }
        }

        private readonly List<Memember> _applyer = new List<Memember>();

        private string _leaderid = string.Empty;

        private IExtension extensionObject;
    }
}
