using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_Join_Team_CS")]
    [Serializable]
    public class MSG_Join_Team_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "teamid", DataFormat = DataFormat.TwosComplement)]
        public uint teamid
        {
            get
            {
                return this._teamid;
            }
            set
            {
                this._teamid = value;
            }
        }

        private uint _teamid;

        private IExtension extensionObject;
    }
}
