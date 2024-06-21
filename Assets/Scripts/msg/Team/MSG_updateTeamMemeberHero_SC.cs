using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_updateTeamMemeberHero_SC")]
    [Serializable]
    public class MSG_updateTeamMemeberHero_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "memid", DataFormat = DataFormat.Default)]
        public string memid
        {
            get
            {
                return this._memid;
            }
            set
            {
                this._memid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "heroid", DataFormat = DataFormat.TwosComplement)]
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

        private string _memid = string.Empty;

        private uint _heroid;

        private IExtension extensionObject;
    }
}
