using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_ChangeGuildMaster_CS")]
    [Serializable]
    public class MSG_Req_ChangeGuildMaster_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "newmasterid", DataFormat = DataFormat.Default)]
        public string newmasterid
        {
            get
            {
                return this._newmasterid;
            }
            set
            {
                this._newmasterid = value;
            }
        }

        private string _newmasterid = string.Empty;

        private IExtension extensionObject;
    }
}
