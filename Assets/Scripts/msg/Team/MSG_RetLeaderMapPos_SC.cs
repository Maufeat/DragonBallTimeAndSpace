using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_RetLeaderMapPos_SC")]
    [Serializable]
    public class MSG_RetLeaderMapPos_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(2, IsRequired = false, Name = "pos", DataFormat = DataFormat.Default)]
        public LeaderPosInfo pos
        {
            get
            {
                return this._pos;
            }
            set
            {
                this._pos = value;
            }
        }

        private uint _retcode;

        private LeaderPosInfo _pos;

        private IExtension extensionObject;
    }
}
