using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_ChangePositionPri_CS")]
    [Serializable]
    public class MSG_Req_ChangePositionPri_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "positionid", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "privilege", DataFormat = DataFormat.TwosComplement)]
        public uint privilege
        {
            get
            {
                return this._privilege;
            }
            set
            {
                this._privilege = value;
            }
        }

        private uint _positionid;

        private uint _privilege;

        private IExtension extensionObject;
    }
}
