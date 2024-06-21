using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "UserTeamInfo")]
    [Serializable]
    public class UserTeamInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "uid", DataFormat = DataFormat.TwosComplement)]
        public ulong uid
        {
            get
            {
                return this._uid;
            }
            set
            {
                this._uid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "team_color", DataFormat = DataFormat.Default)]
        public string team_color
        {
            get
            {
                return this._team_color;
            }
            set
            {
                this._team_color = value;
            }
        }

        private ulong _uid;

        private string _team_color = string.Empty;

        private IExtension extensionObject;
    }
}
