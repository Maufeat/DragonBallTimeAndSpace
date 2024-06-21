using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_CreateTeam_CS")]
    [Serializable]
    public class MSG_CreateTeam_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "name", DataFormat = DataFormat.Default)]
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
        [ProtoMember(2, IsRequired = false, Name = "activityid", DataFormat = DataFormat.TwosComplement)]
        public uint activityid
        {
            get
            {
                return this._activityid;
            }
            set
            {
                this._activityid = value;
            }
        }

        private string _name;

        private uint _activityid;

        private IExtension extensionObject;
    }
}
