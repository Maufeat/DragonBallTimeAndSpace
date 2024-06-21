using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_RetChangeMapToLeader_SC")]
    [Serializable]
    public class MSG_RetChangeMapToLeader_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint retcode
        {
            get
            {
                return this._retcode;
            }
            set
            {
                this._retcode = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(2, IsRequired = false, Name = "info", DataFormat = DataFormat.Default)]
        public LeaderPosInfo info
        {
            get
            {
                return this._info;
            }
            set
            {
                this._info = value;
            }
        }

        private uint _retcode;

        private LeaderPosInfo _info;

        private IExtension extensionObject;
    }
}
