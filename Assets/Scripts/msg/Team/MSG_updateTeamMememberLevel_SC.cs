using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_updateTeamMememberLevel_SC")]
    [Serializable]
    public class MSG_updateTeamMememberLevel_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "mememberid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
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
        [ProtoMember(2, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
        public uint level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "membername", DataFormat = DataFormat.Default)]
        public string membername
        {
            get
            {
                return this._membername;
            }
            set
            {
                this._membername = value;
            }
        }

        private string _mememberid = string.Empty;

        private uint _level;

        private string _membername = string.Empty;

        private IExtension extensionObject;
    }
}
