using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_updateMemStateToTeam_SC")]
    [Serializable]
    public class MSG_updateMemStateToTeam_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "memid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
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

        [ProtoMember(2, IsRequired = false, Name = "sceneid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string sceneid
        {
            get
            {
                return this._sceneid;
            }
            set
            {
                this._sceneid = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(MemState.NORMAL)]
        public MemState state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        private string _memid = string.Empty;

        private string _sceneid = string.Empty;

        private MemState _state;

        private IExtension extensionObject;
    }
}
