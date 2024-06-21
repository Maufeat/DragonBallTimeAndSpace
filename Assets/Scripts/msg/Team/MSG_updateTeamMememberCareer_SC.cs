using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_updateTeamMememberCareer_SC")]
    [Serializable]
    public class MSG_updateTeamMememberCareer_SC : IExtensible
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
        [ProtoMember(2, IsRequired = false, Name = "career", DataFormat = DataFormat.TwosComplement)]
        public uint career
        {
            get
            {
                return this._career;
            }
            set
            {
                this._career = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "careerlevel", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint careerlevel
        {
            get
            {
                return this._careerlevel;
            }
            set
            {
                this._careerlevel = value;
            }
        }

        private string _mememberid = string.Empty;

        private uint _career;

        private uint _careerlevel;

        private IExtension extensionObject;
    }
}
