using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_ReqTriggerQTESkill_CS")]
    [Serializable]
    public class MSG_ReqTriggerQTESkill_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "bosstempid", DataFormat = DataFormat.TwosComplement)]
        public ulong bosstempid
        {
            get
            {
                return this._bosstempid;
            }
            set
            {
                this._bosstempid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "warppos", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public Position warppos
        {
            get
            {
                return this._warppos;
            }
            set
            {
                this._warppos = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "dir", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint dir
        {
            get
            {
                return this._dir;
            }
            set
            {
                this._dir = value;
            }
        }

        private ulong _bosstempid;

        private Position _warppos;

        private uint _dir;

        private IExtension extensionObject;
    }
}
