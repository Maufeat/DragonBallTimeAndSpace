using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_MatchInfo_SC")]
    [Serializable]
    public class MSG_MatchInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint num
        {
            get
            {
                return this._num;
            }
            set
            {
                this._num = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "ready_num", DataFormat = DataFormat.TwosComplement)]
        public uint ready_num
        {
            get
            {
                return this._ready_num;
            }
            set
            {
                this._ready_num = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(3, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        public ulong id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        private uint _num;

        private uint _ready_num;

        private ulong _id;

        private IExtension extensionObject;
    }
}
