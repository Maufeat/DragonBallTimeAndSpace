using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_GuildPk_FinalResult_SC")]
    [Serializable]
    public class MSG_GuildPk_FinalResult_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "iswin", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool iswin
        {
            get
            {
                return this._iswin;
            }
            set
            {
                this._iswin = value;
            }
        }

        [ProtoMember(2, Name = "teamlist", DataFormat = DataFormat.Default)]
        public List<finalresult_guildteam_info> teamlist
        {
            get
            {
                return this._teamlist;
            }
        }

        private bool _iswin;

        private readonly List<finalresult_guildteam_info> _teamlist = new List<finalresult_guildteam_info>();

        private IExtension extensionObject;
    }
}
