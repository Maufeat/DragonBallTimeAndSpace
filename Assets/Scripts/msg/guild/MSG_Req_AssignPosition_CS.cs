using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_AssignPosition_CS")]
    [Serializable]
    public class MSG_Req_AssignPosition_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "memberid", DataFormat = DataFormat.TwosComplement)]
        public ulong memberid
        {
            get
            {
                return this._memberid;
            }
            set
            {
                this._memberid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "positionid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint positionid
        {
            get
            {
                return this._positionid;
            }
            set
            {
                this._positionid = value;
            }
        }

        private ulong _memberid;

        private uint _positionid;

        private IExtension extensionObject;
    }
}
