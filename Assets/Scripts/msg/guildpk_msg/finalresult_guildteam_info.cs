using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "finalresult_guildteam_info")]
    [Serializable]
    public class finalresult_guildteam_info : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint rank
        {
            get
            {
                return this._rank;
            }
            set
            {
                this._rank = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "killnum", DataFormat = DataFormat.TwosComplement)]
        public uint killnum
        {
            get
            {
                return this._killnum;
            }
            set
            {
                this._killnum = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "totaldmg", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint totaldmg
        {
            get
            {
                return this._totaldmg;
            }
            set
            {
                this._totaldmg = value;
            }
        }

        private uint _rank;

        private string _name = string.Empty;

        private uint _killnum;

        private uint _totaldmg;

        private IExtension extensionObject;
    }
}
