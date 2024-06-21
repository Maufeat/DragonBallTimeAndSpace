using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_updateTeamMemberFight_SC")]
    [Serializable]
    public class MSG_updateTeamMemberFight_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "mememberid", DataFormat = DataFormat.Default)]
        public string mememberid
        {
            get
            {
                return this._mememberid;
            }
            set
            {
                this._mememberid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "fight", DataFormat = DataFormat.TwosComplement)]
        public uint fight
        {
            get
            {
                return this._fight;
            }
            set
            {
                this._fight = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "heroid", DataFormat = DataFormat.TwosComplement)]
        public uint heroid
        {
            get
            {
                return this._heroid;
            }
            set
            {
                this._heroid = value;
            }
        }

        private string _mememberid = string.Empty;

        private uint _fight;

        private uint _heroid;

        private IExtension extensionObject;
    }
}
